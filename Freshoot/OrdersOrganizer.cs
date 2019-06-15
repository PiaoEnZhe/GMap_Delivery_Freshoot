using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace Freshoot
{
    public partial class OrdersOrganizer : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd,
                         int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        DeliverManager manager = new DeliverManager();
        GMapOverlay markers = new GMapOverlay("markers");
        GMapOverlay poly = new GMapOverlay("polygons");
        GMapOverlay routes = new GMapOverlay("routes");

        Bitmap bmpnumbers = new Bitmap(Properties.Resources.newnumbers);
        Bitmap[] markerbitmps = { Properties.Resources.marker_a, Properties.Resources.marker_b, Properties.Resources.marker_c, Properties.Resources.marker_d,
                                Properties.Resources.marker_e, Properties.Resources.marker_f, Properties.Resources.marker_g, Properties.Resources.marker_h,
                                Properties.Resources.marker_i, Properties.Resources.marker_j, Properties.Resources.marker_k, Properties.Resources.marker_l,
                                Properties.Resources.marker_m, Properties.Resources.marker_n, Properties.Resources.marker_o, Properties.Resources.marker_p,
                                Properties.Resources.marker_q, Properties.Resources.marker_r, Properties.Resources.marker_s, Properties.Resources.marker_t};
        
        Bitmap[] markerbitmps1 = { Properties.Resources.marker_a_sec, Properties.Resources.marker_b_sec, Properties.Resources.marker_c_sec, Properties.Resources.marker_d_sec,
                                Properties.Resources.marker_e_sec, Properties.Resources.marker_f_sec, Properties.Resources.marker_g_sec, Properties.Resources.marker_h_sec,
                                Properties.Resources.marker_i_sec, Properties.Resources.marker_j_sec, Properties.Resources.marker_k_sec, Properties.Resources.marker_l_sec,
                                Properties.Resources.marker_m_sec, Properties.Resources.marker_n_sec, Properties.Resources.marker_o_sec, Properties.Resources.marker_p_sec,
                                Properties.Resources.marker_q_sec, Properties.Resources.marker_r_sec, Properties.Resources.marker_s_sec, Properties.Resources.marker_t_sec};

    
        string apikey = "AIzaSyBYRn4-66u_0wwM6SJwuZhVE_BjeKZ0AmY";

        public int clickedorderid = 0;
        public int clickedtruckid = 0;

        public static string origin_addr = "Canada, Calgary,6812 6 St SE";
        public static double ori_lat, ori_lng;

        Random random = new Random();

        public static SQLiteConnection dbConnection { get; set; }

        public static Color[] routecolor = { Color.Red, Color.Blue, Color.Green, Color.Black, Color.DarkGoldenrod, Color.Aqua, Color.Aquamarine, Color.Crimson, Color.DarkOrchid, Color.Gainsboro, Color.HotPink, Color.Brown, Color.DarkViolet, Color.DarkCyan, Color.Crimson, Color.Honeydew };

        public OrdersOrganizer()
        {
            InitializeComponent();

            GMapProviders.GoogleMap.ApiKey = apikey;
            gmap.MapProvider = GMap.NET.MapProviders.BingMapProvider.Instance;

            gmap.SetPositionByKeywords("Calgary, Cananda");
            gmap.Position = new GMap.NET.PointLatLng(50.998317, -114.1174799);

            var dbPath = Directory.GetCurrentDirectory() + "\\map.db";

            if (!File.Exists(dbPath))
            {
                SQLiteConnection.CreateFile(dbPath);
                dbConnection = new SQLiteConnection("Data Source = map.db;Version = 3;");
                dbConnection.Open();

                string sql = "create table geocode (address varchar(200), lat float, lng float)";
                SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
                command.ExecuteNonQuery();

                sql = "create table route (_order int, lat_ori float, lng_ori float, lat_des float, lng_des float, lat float, lng float)";
                command = new SQLiteCommand(sql, dbConnection);
                command.ExecuteNonQuery();

                sql = "create table dist (lat_ori float, lng_ori float, lat_des float, lng_des float, distance float)";
                command = new SQLiteCommand(sql, dbConnection);
                command.ExecuteNonQuery();
            }
            else
            {
                dbConnection = new SQLiteConnection("Data Source = map.db;Version = 3;");
                dbConnection.Open();
            }
        }

        private void OnLoadOrder(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Title = "Select the order list file";
                dlg.DefaultExt = "txt";
                if (dlg.ShowDialog() == DialogResult.Cancel) return;
                string listfilename = dlg.FileName;
                if (!manager.ReadOrdersFromFile(listfilename))
                {
                    MessageBox.Show("can not load the order list file!");
                    return;
                }

                comboTruck.Items.Clear();
                listTruck.ClearSelected();
                ordercntcontrol.Value = 30;
                Cursor.Current = Cursors.WaitCursor;

                // load the shop's original address
                {
                    string json = string.Empty;
                    string url = @"https://maps.googleapis.com/maps/api/geocode/json?address=";
                    string addressforsql = "'" + origin_addr + "'";
                    string sql = "select * from geocode where address = " + addressforsql;
                    SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
                    SQLiteDataReader sqlreader = command.ExecuteReader();
                    if (sqlreader.Read())
                    {
                        ori_lat = Convert.ToDouble(sqlreader["lat"].ToString());
                        ori_lng = Convert.ToDouble(sqlreader["lng"].ToString());
                    }
                    else
                    {
                        url = url + origin_addr + "&key=" + apikey;
                        url.Replace(' ', '+');
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                        request.AutomaticDecompression = DecompressionMethods.GZip;
                        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                        using (Stream stream = response.GetResponseStream())
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            json = reader.ReadToEnd();
                        }

                        int latstart = json.IndexOf("\"lat\""); json = json.Substring(latstart + 7); int latend = json.IndexOf(','); string slat = json.Substring(0, latend); json = json.Substring(latend);
                        int lngstart = json.IndexOf("\"lng\""); json = json.Substring(lngstart + 7); int lngend = json.IndexOf('\n'); string slng = json.Substring(0, lngend);

                        ori_lng = Convert.ToDouble(slng);
                        ori_lat = Convert.ToDouble(slat);

                        sql = "insert into geocode (address,lat,lng) values(" + addressforsql + " , " + Convert.ToString(ori_lat) + " , " + Convert.ToString(ori_lng) + ")";
                        command = new SQLiteCommand(sql, dbConnection);
                        command.ExecuteNonQuery();
                    }
                }


                for (int i = 0; i < DeliverManager.maxordercnt; i++)
                    for (int j = 0; j < DeliverManager.maxordercnt; j++)
                        manager.distanceMatrix[i, j] = 0;

                int ordercnt = DeliverManager.orderlist.Count;
                for (int i = 0; i < ordercnt; i++)
                {
                    string json = string.Empty;
                    string url = @"https://maps.googleapis.com/maps/api/geocode/json?address=";
                    string address = "Canada, " + DeliverManager.orderlist[i].city + ", " + DeliverManager.orderlist[i].streetname;
                    string addressforsql = "'" + address + "'";
                    string sql = "select * from geocode where address = " + addressforsql;
                    SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
                    SQLiteDataReader sqlreader = command.ExecuteReader();
                    double lat, lng;
                    if (sqlreader.Read())
                    {
                        lat = Convert.ToDouble(sqlreader["lat"].ToString());
                        lng = Convert.ToDouble(sqlreader["lng"].ToString());
                    }
                    else
                    {
                        url = url + address + "&key=" + apikey;
                        url.Replace(' ', '+');
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                        request.AutomaticDecompression = DecompressionMethods.GZip;
                        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                        using (Stream stream = response.GetResponseStream())
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            json = reader.ReadToEnd();
                        }

                        int latstart = json.IndexOf("\"lat\""); if (latstart < 0) continue; json = json.Substring(latstart + 7); int latend = json.IndexOf(','); string slat = json.Substring(0, latend); json = json.Substring(latend);
                        int lngstart = json.IndexOf("\"lng\""); if (lngstart < 0) continue; json = json.Substring(lngstart + 7); int lngend = json.IndexOf('\n'); string slng = json.Substring(0, lngend);

                        lng = Convert.ToDouble(slng);
                        lat = Convert.ToDouble(slat);

                        sql = "insert into geocode (address,lat,lng) values(" + addressforsql + " , " + Convert.ToString(lat) + " , " + Convert.ToString(lng) + ")";
                        command = new SQLiteCommand(sql, dbConnection);
                        command.ExecuteNonQuery();
                    }

                    DeliverManager.orderlist[i].lat = lat;
                    DeliverManager.orderlist[i].lng = lng;
                }

                manager.orderspertruck = Convert.ToInt32(ordercntcontrol.Value);
                manager.AutoDeploy();

                for (int i = 0; i < DeliverManager.trucklist.Count; i++)
                {
                    string str = DeliverManager.trucklist[i].trucknumber + " : " + Convert.ToString(DeliverManager.trucklist[i].orderidlist.Count) + " orders.";
                    comboTruck.Items.Add(str);
                }

                comboTruck.SelectedIndex = 0;

                RefreshOrderList();
                Cursor.Current = Cursors.Default;
            }
            catch
            {
                MessageBox.Show("Network status may not good, Try again later.");
                Close();
            }
            RedrawMap();
        }

        public void RedrawMap()
        {
            markers.Clear();
            poly.Clear();
            routes.Clear();

            try
            {
                Cursor.Current = Cursors.WaitCursor;
                // draw shop mark
                {
                    Bitmap bmpmarker = new Bitmap(Properties.Resources.shop);
                    GMapMarker marker = new GMarkerGoogle(new PointLatLng(ori_lat, ori_lng), bmpmarker);
                    markers.Markers.Add(marker);
                }
                int indexselected = comboTruck.SelectedIndex;
               // Color color;
                for (int k = 0; k < listTruck.CheckedItems.Count; k++)
                {
                    int indexChecked = listTruck.CheckedIndices[k];
                    int ranks = DeliverManager.trucklist[indexChecked].orderidlist.Count;
                    int color_r = Convert.ToInt32(255 * Math.Abs(Math.Cos(360 / DeliverManager.trucklist.Count * k)));
                    int color_g = Convert.ToInt32(255 * Math.Abs(Math.Sin(360 / DeliverManager.trucklist.Count * k)));
                    int color_b = Convert.ToInt32(255 * Math.Abs(Math.Cos(360 / DeliverManager.trucklist.Count * k)));
                    Color color_main = Color.FromArgb(255, color_r, color_g, color_b);
                    Color color_sub = Color.FromArgb(25, color_r, color_g, color_b);
                    Pen pen;
                    int fontsize;

                    if (indexChecked == indexselected)
                        pen = new Pen(color_main, 5);
                    else
                        pen = new Pen(color_sub,3);

                    for (int i = 0; i < ranks; i++)
                    {
                        Bitmap bmpmarker;
                        RectangleF rectf;
                        if (i < 9)
                            rectf = new RectangleF(18, 8, 30, 30);
                        else
                            rectf = new RectangleF(13, 8, 30, 30);

                        Brush brush;
                        if (indexChecked == indexselected)
                        {
                            bmpmarker = new Bitmap(markerbitmps[indexChecked], new Size(50, 50));
                            brush = Brushes.Black;
                            fontsize = 13;
                        }
                        else
                        {
                            bmpmarker = new Bitmap(markerbitmps1[indexChecked], new Size(50, 50));
                            brush = Brushes.Gray;
                            fontsize = 10;
                        }

                        GMapMarker marker = new GMarkerGoogle(new PointLatLng(DeliverManager.orderlist[DeliverManager.trucklist[indexChecked].orderidlist[i]].lat, DeliverManager.orderlist[DeliverManager.trucklist[indexChecked].orderidlist[i]].lng), (GMarkerGoogleType)(indexChecked + 1));

                       
                        Graphics g = Graphics.FromImage(bmpmarker);
                        g.SmoothingMode = SmoothingMode.AntiAlias;
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        g.DrawString(Convert.ToString(i+1), new Font("Tahoma", fontsize), brush, rectf);
                        g.Flush();

                        marker = new GMarkerGoogle(new GMap.NET.PointLatLng(DeliverManager.orderlist[DeliverManager.trucklist[indexChecked].orderidlist[i]].lat, DeliverManager.orderlist[DeliverManager.trucklist[indexChecked].orderidlist[i]].lng), bmpmarker);

                        marker.Tag = Convert.ToString(DeliverManager.trucklist[indexChecked].truckid + 1) + "orderid:" + Convert.ToString(DeliverManager.trucklist[indexChecked].orderidlist[i]);
                        markers.Markers.Add(marker);
                    }

                    DeliverManager.trucklist[indexChecked].distance = 0;
                    DeliverManager.trucklist[indexChecked].isdistcalced = true;

                    //draw the subroute from the shop to the first point
                    {
                        PointLatLng point_ori = new PointLatLng(ori_lat, ori_lng);
                        PointLatLng point_des = new PointLatLng(DeliverManager.orderlist[DeliverManager.trucklist[indexChecked].orderidlist[0]].lat, DeliverManager.orderlist[DeliverManager.trucklist[indexChecked].orderidlist[0]].lng);

                        MapRoute path = GMap.NET.MapProviders.GoogleMapProvider.Instance.GetRoute(point_ori, point_des, false, false, 15);
                        GMapRoute route = new GMapRoute(path.Points, "My route" + Convert.ToString(indexChecked));
                        route.Stroke = pen;
                        routes.Routes.Add(route);
                        DeliverManager.trucklist[indexChecked].distance += route.Distance; // for kilometer
                    }

                    // draw the main routine includes the points
                    for (int i = 0; i < ranks - 1; i++)
                    {
                        PointLatLng point_ori = new PointLatLng(DeliverManager.orderlist[DeliverManager.trucklist[indexChecked].orderidlist[i]].lat, DeliverManager.orderlist[DeliverManager.trucklist[indexChecked].orderidlist[i]].lng);
                        PointLatLng point_des = new PointLatLng(DeliverManager.orderlist[DeliverManager.trucklist[indexChecked].orderidlist[i+1]].lat, DeliverManager.orderlist[DeliverManager.trucklist[indexChecked].orderidlist[i+1]].lng);

                        MapRoute path = GMap.NET.MapProviders.GoogleMapProvider.Instance.GetRoute(point_ori, point_des, false, false, 15);
                        GMapRoute route = new GMapRoute(path.Points, "My route" + Convert.ToString(indexChecked));
                        route.Stroke = pen;
                        routes.Routes.Add(route);
                        DeliverManager.trucklist[indexChecked].distance += route.Distance; // for kilometer
                    }

                    // draw the last subroutine from the last point to shop
                    {
                        PointLatLng point_ori = new PointLatLng(DeliverManager.orderlist[DeliverManager.trucklist[indexChecked].orderidlist[ranks-1]].lat, DeliverManager.orderlist[DeliverManager.trucklist[indexChecked].orderidlist[ranks-1]].lng);
                        PointLatLng point_des = new PointLatLng(ori_lat, ori_lng);

                        MapRoute path = GMap.NET.MapProviders.GoogleMapProvider.Instance.GetRoute(point_ori, point_des, false, false, 15);
                        GMapRoute route = new GMapRoute(path.Points, "My route" + Convert.ToString(indexChecked));
                        route.Stroke = pen;
                        DeliverManager.trucklist[indexChecked].distance += route.Distance; // for kilometer
                        routes.Routes.Add(route);
                    }
                }
                Cursor.Current = Cursors.Default;
            }
            catch
            {
                MessageBox.Show("Network status may not good, Try again later.");
                Close();
            }
            gmap.Overlays.Add(markers);
            gmap.Overlays.Add(routes);
            gmap.Refresh();

            List<string> list = new List<string>();
            for (int i = 0; i < DeliverManager.trucklist.Count; i++)
            {
                double time = DeliverManager.trucklist[i].distance / DeliverManager.trucklist[i].speed * 3600 + 300 * DeliverManager.trucklist[i].orderidlist.Count; // time unit: s
                int ntime = Convert.ToInt32(time);
                string stime;
                if (DeliverManager.trucklist[i].isdistcalced)
                    stime = Convert.ToString(ntime / 3600) + " hr:" + Convert.ToString((ntime % 3600) / 60) + " min:" + Convert.ToString(ntime % 60) + "s";
                else
                    stime = "--:--:--";
                string str = DeliverManager.trucklist[i].trucknumber + " : " + Convert.ToString(DeliverManager.trucklist[i].orderidlist.Count) + " orders. " + stime;
                list.Add(str);
            }
            listTruck.DataSource = list;

        }

        private void ListTruck_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            this.BeginInvoke(new Action(() =>
            {
                RedrawMap();
            }));
        }

        public void MarkerMenuHandler(object sender, ToolStripItemClickedEventArgs e)
        {
            string str = e.ClickedItem.Name.ToString();
            int totruckid = Convert.ToInt32(str.Substring(13)) - 1;
            manager.DeleteOrderFromTruck(clickedtruckid - 1, clickedorderid);
            manager.AddOrderToTruck(totruckid, clickedorderid);
            RedrawMap();
        }
        private void Gmap_OnMarkerClick(GMapMarker item, MouseEventArgs e)
        {
            ContextMenuStrip contextMenu = new ContextMenuStrip();
            string strtag = item.Tag.ToString();
            int orderidstartidx = strtag.IndexOf("orderid:");
            if (orderidstartidx < 0) return;
            clickedtruckid = Convert.ToInt32(strtag.Substring(0, orderidstartidx));
            clickedorderid = Convert.ToInt32(strtag.Substring(orderidstartidx + 8));

            for (int i = 0; i < DeliverManager.trucklist.Count; i++)
            {
                if (DeliverManager.trucklist[i].truckid == clickedtruckid - 1) continue;
                string name = String.Format("move to truck{0}", i + 1);
                contextMenu.Items.Add(name).Name = name;
            }

            contextMenu.Show(e.X + DesktopLocation.X + gmap.Location.X, e.Y + DesktopLocation.Y + gmap.Location.Y);
            contextMenu.ItemClicked += new ToolStripItemClickedEventHandler(MarkerMenuHandler);
        }

        private void ListOrder_MouseClick(object sender, MouseEventArgs e)
        {
            return;
            int orderidx = listOrder.SelectedIndex;
            if (orderidx < 0) return;

            ContextMenuStrip contextMenu = new ContextMenuStrip();

            clickedtruckid = listTruck.SelectedIndex + 1;
            clickedorderid = DeliverManager.trucklist[clickedtruckid - 1].orderidlist[orderidx];
            for (int i = 0; i < DeliverManager.trucklist.Count; i++)
            {
                if (DeliverManager.trucklist[i].truckid == clickedtruckid - 1) continue;
                string name = String.Format("move to truck{0}", i + 1);
                contextMenu.Items.Add(name).Name = name;
            }

            contextMenu.Show(e.X + DesktopLocation.X + listOrder.Location.X, e.Y + DesktopLocation.Y + listOrder.Location.Y);
            contextMenu.ItemClicked += new ToolStripItemClickedEventHandler(MarkerMenuHandler);
        }

        private void ListTruck_MouseClick(object sender, MouseEventArgs e)
        {
            return;
        }

        private void OnExit(object sender, EventArgs e)
        {
            System.Environment.Exit(1);
        }

        private void OnChangeOrderCnt(object sender, EventArgs e)
        {
            int truckidx = comboTruck.SelectedIndex;
            int oldordercnt = DeliverManager.trucklist[truckidx].orderidlist.Count;
            int currentordercnt = Convert.ToInt32(ordercntcontrol.Value);
            if(currentordercnt > oldordercnt)
            {
                if (truckidx < DeliverManager.trucklist.Count - 1)
                {
                    int removedidfromnexttruck = DeliverManager.trucklist[truckidx + 1].MinusOrder();
                    DeliverManager.trucklist[truckidx].PlusOrder(removedidfromnexttruck);
                    RedrawMap();
                }
                else
                    ordercntcontrol.Value = Convert.ToDecimal(oldordercnt);
            }
            else if(currentordercnt < oldordercnt)
            {
                if(truckidx > 0)
                {
                    int removedidfromcurrenttruck = DeliverManager.trucklist[truckidx].MinusOrder();
                    DeliverManager.trucklist[truckidx - 1].PlusOrder(removedidfromcurrenttruck);
                    RedrawMap();
                }
                else
                    ordercntcontrol.Value = Convert.ToDecimal(oldordercnt);
            }
            else
                ordercntcontrol.Value = Convert.ToDecimal(oldordercnt);


            int temp = comboTruck.SelectedIndex;
            comboTruck.Items.Clear();
            for (int i = 0; i < DeliverManager.trucklist.Count; i++)
            {
                string str = DeliverManager.trucklist[i].trucknumber + " : " + Convert.ToString(DeliverManager.trucklist[i].orderidlist.Count) + " orders.";
                comboTruck.Items.Add(str);
            }
            comboTruck.SelectedIndex = temp;
        }

        private void OnLoadGeoInformation(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Select the order list file";
            dlg.DefaultExt = "txt";
            if (dlg.ShowDialog() == DialogResult.Cancel) return;
            string listfilename = dlg.FileName;
            if (!manager.ReadOrdersFromFile(listfilename))
            {
                MessageBox.Show("can not load the order list file!");
                return;
            }
            int ordercnt = DeliverManager.orderlist.Count;

            try
            {
                for (int i = 0; i < ordercnt; i++)
                {
                    string json = string.Empty;
                    string url = @"https://maps.googleapis.com/maps/api/geocode/json?address=";
                    string address = "Canada, " + DeliverManager.orderlist[i].city + ", " + DeliverManager.orderlist[i].streetname;
                    string addressforsql = "'" + address + "'";
                    string sql = "select * from geocode where address = " + addressforsql;
                    SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
                    SQLiteDataReader sqlreader = command.ExecuteReader();
                    double lat, lng;
                    if (sqlreader.Read())
                    {
                     //   lat = Convert.ToDouble(sqlreader["lat"].ToString());
                     //   lng = Convert.ToDouble(sqlreader["lng"].ToString());
                    }
                    else
                    {
                        url = url + address + "&key=" + apikey;
                        url.Replace(' ', '+');
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                        request.AutomaticDecompression = DecompressionMethods.GZip;
                        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                        using (Stream stream = response.GetResponseStream())
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            json = reader.ReadToEnd();
                        }

                        int latstart = json.IndexOf("\"lat\""); if (latstart < 0) continue; json = json.Substring(latstart + 7); int latend = json.IndexOf(','); string slat = json.Substring(0, latend); json = json.Substring(latend);
                        int lngstart = json.IndexOf("\"lng\""); if (lngstart < 0) continue; json = json.Substring(lngstart + 7); int lngend = json.IndexOf('\n'); string slng = json.Substring(0, lngend);

                        lng = Convert.ToDouble(slng);
                        lat = Convert.ToDouble(slat);

                        sql = "insert into geocode (address,lat,lng) values(" + addressforsql + " , " + Convert.ToString(lat) + " , " + Convert.ToString(lng) + ")";
                        command = new SQLiteCommand(sql, dbConnection);
                        command.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Geo Information successfully loaded!");
            }
            catch
            {
                MessageBox.Show("Network status may not good, Try again later.");
                Close();
            }
        }
        public void RefreshOrderList()
        {
            listOrder.Items.Clear();
            int truckidx = comboTruck.SelectedIndex;
            
            if (truckidx >= 0 && truckidx < DeliverManager.trucklist.Count)
            {
                for (int i = 0; i < DeliverManager.trucklist[truckidx].orderidlist.Count; i++)
                {
                    Order order = DeliverManager.orderlist[DeliverManager.trucklist[truckidx].orderidlist[i]];
                    string str = "- order " + Convert.ToString(i + 1) + " : " + order.firstname + " " + order.lastname + " : " + order.addressnumber + ", " + order.streetname;
                    listOrder.Items.Add(str);
                }
            }
        }

        private void OrderUp_Click(object sender, EventArgs e)
        {
            int idx = listOrder.SelectedIndex;
            if (idx <= 0) return;
            int truckidx = comboTruck.SelectedIndex;
            int temp = DeliverManager.trucklist[truckidx].orderidlist[idx - 1];
            DeliverManager.trucklist[truckidx].orderidlist[idx - 1] = DeliverManager.trucklist[truckidx].orderidlist[idx];
            DeliverManager.trucklist[truckidx].orderidlist[idx] = temp;
            RefreshOrderList();
            listOrder.SelectedIndex = idx - 1;
        }

        private void OrderDown_Click(object sender, EventArgs e)
        {
            int idx = listOrder.SelectedIndex;
            int truckidx = comboTruck.SelectedIndex;
            if (idx < 0 || idx >= DeliverManager.trucklist[truckidx].orderidlist.Count - 1) return;
            int temp = DeliverManager.trucklist[truckidx].orderidlist[idx];
            DeliverManager.trucklist[truckidx].orderidlist[idx] = DeliverManager.trucklist[truckidx].orderidlist[idx + 1];
            DeliverManager.trucklist[truckidx].orderidlist[idx + 1] = temp;
            RefreshOrderList();
            listOrder.SelectedIndex = idx + 1;
        }

        private void OrderRefresh_Click(object sender, EventArgs e)
        {
            RedrawMap();
        }

        private void ComboTruck_SelectedIndexChanged(object sender, EventArgs e)
        {
            int truckidx = comboTruck.SelectedIndex;
            pictureTruckNumber.Image = new Bitmap(markerbitmps[truckidx]);
            ordercntcontrol.Value = Convert.ToDecimal(DeliverManager.trucklist[truckidx].orderidlist.Count);
            RefreshOrderList();     
            RedrawMap();
        }
    }
}
