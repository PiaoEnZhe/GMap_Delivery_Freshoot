using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using GMap.NET.MapProviders;
using System.Net;
using System.IO;
using System.Web;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices;
using System.Web.Script.Serialization;
using System.Data.SQLite;
using Google.OrTools.ConstraintSolver;
using Freshoot.Properties;

namespace DelveryManager_CSharp
{
    public partial class DeliveryApp : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        private Panel panel1;
        private Panel panel4;
        private Panel panel2;
        private Panel panel3;
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

        Bitmap bmpnumbers = new Bitmap(Resources.newnumbers);
        string apikey = "AIzaSyBYRn4-66u_0wwM6SJwuZhVE_BjeKZ0AmY";

        public int clickedorderid = 0;
        public int clickedtruckid = 0;

        Random random = new Random();

        public static SQLiteConnection dbConnection { get; set; }

        public static Color[] routecolor = { Color.Red, Color.Blue, Color.Green, Color.Black, Color.DarkGoldenrod, Color.Aqua, Color.Aquamarine, Color.Crimson, Color.DarkOrchid, Color.Gainsboro, Color.HotPink, Color.Brown, Color.DarkViolet, Color.DarkCyan, Color.Crimson, Color.Honeydew };

        public class jwaypoint
        {
            public string geocoder_status { get; set; }
            public string place_id { get; set; }
            public string type { get; set; }
        }

        public class jlocation
        {
            public double lat { get; set; }
            public double lng { get; set; }

        }
        public class jdata
        {
            public string text { get; set; }
            public int value { get; set; }
        }

        public class jpolyline
        {
            public string points { get; set; }
        }
        public class jstep
        {
            public jdata distance { get; set; }
            public jdata duration { get; set; }
            public jlocation end_location { get; set; }
            public string html_instructions { get; set; }
            public jpolyline polyline { get; set; }
            public jlocation start_location { get; set; }
            public string travel_mode { get; set; }
        }
        public class jvia_waypoint
        {
            public jlocation location { get; set; }
            public int step_index { get; set; }
            public int step_interpolation { get; set; }
        }

        public class jleg
        {
            public jdata distance { get; set; }
            public jdata duration { get; set; }
            public string end_address { get; set; }
            public jlocation end_location { get; set; }
            public string start_address { get; set; }
            public jlocation start_location { get; set; }
            public List<jstep> steps { get; set; }
            public List<jdata> traffic_speed_entry { get; set; }
            public List<jvia_waypoint> via_viewpoint { get; set; }
        }

        public class joverview
        {
            public string points { get; set; }
        }
        public class jbound
        {
            public jlocation northeast { get; set; }
            public jlocation southwest { get; set; }
            public string copyrights { get; set; }
            public List<jleg> legs { get; set; }
            public joverview overview_polyline { get; set; }
            public string summary { get; set; }
            public List<string> warnings { get; set; }
            public List<string> waypoint_order { get; set; }
        }

        public class jroute
        {
            public List<jwaypoint> geocoded_waypoints { get; set; }
            public List<jbound> routes { get; set; }
            public string status { get; set; }
        }
        private List<PointLatLng> DecodePolyline(string encodedPoints)
        {
            if (string.IsNullOrWhiteSpace(encodedPoints))
            {
                return null;
            }

            int index = 0;
            var polylineChars = encodedPoints.ToCharArray();
            var poly = new List<PointLatLng>();
            int currentLat = 0;
            int currentLng = 0;
            int next5Bits;

            while (index < polylineChars.Length)
            {
                // calculate next latitude
                int sum = 0;
                int shifter = 0;

                do
                {
                    next5Bits = polylineChars[index++] - 63;
                    sum |= (next5Bits & 31) << shifter;
                    shifter += 5;
                }
                while (next5Bits >= 32 && index < polylineChars.Length);

                if (index >= polylineChars.Length)
                {
                    break;
                }

                currentLat += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

                // calculate next longitude
                sum = 0;
                shifter = 0;

                do
                {
                    next5Bits = polylineChars[index++] - 63;
                    sum |= (next5Bits & 31) << shifter;
                    shifter += 5;
                }
                while (next5Bits >= 32 && index < polylineChars.Length);

                if (index >= polylineChars.Length && next5Bits >= 32)
                {
                    break;
                }

                currentLng += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

                var mLatLng = new PointLatLng(Convert.ToDouble(currentLat) / 100000.0, Convert.ToDouble(currentLng) / 100000.0);
                poly.Add(mLatLng);
            }

            return poly;
        }

        public DeliveryApp()
        {
            InitializeComponent();

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
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Select the order list file";
            dlg.DefaultExt = "txt";
            if(dlg.ShowDialog() == DialogResult.Cancel) return;
            string listfilename = dlg.FileName;
            progressBar.Value = 0;
            if (!manager.ReadOrdersFromFile(listfilename))
            {
                MessageBox.Show("can not load the order list file!");
                return;
            }
            progressBar.Value = 10;
            for (int i = 0; i < DeliverManager.maxordercnt; i++)
                for (int j = 0; j < DeliverManager.maxordercnt; j++)
                    manager.distanceMatrix[i, j] = 0;

            int ordercnt = DeliverManager.orderlist.Count;
            for (int i = 0; i < ordercnt; i++)
            {
                progressBar.Value = 10 + Convert.ToInt32(90.0/(double)ordercnt * (double)i);
                string json = string.Empty;
                string url = @"https://maps.googleapis.com/maps/api/geocode/json?address=";
                string address = "Canada, " + DeliverManager.orderlist[i].city + ", " + DeliverManager.orderlist[i].streetname;
                string addressforsql = "'" + address + "'";
                string sql = "select * from geocode where address = " + addressforsql;
                SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
                SQLiteDataReader sqlreader = command.ExecuteReader();
                double lat, lng;
                if(sqlreader.Read())
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
                GMap.NET.WindowsForms.GMapMarker marker =
                  new GMap.NET.WindowsForms.Markers.GMarkerGoogle(
                  new GMap.NET.PointLatLng(lat, lng),
                  GMap.NET.WindowsForms.Markers.GMarkerGoogleType.blue_pushpin);
                markers.Markers.Add(marker);
            }

          //  manager.LoadDistance();
            manager.AutoDeploy();
            progressBar.Value = 100;
            RedrawMap();
        }

        public void RedrawMap()
        {
            progressBar.Value = 0;
            markers.Clear();
            poly.Clear();
            routes.Clear();

            for (int k = 0; k < listTruck.CheckedItems.Count; k++)
            {
                int indexChecked = listTruck.CheckedIndices[k];
                int ranks = manager.trucklist[indexChecked].points.Count;
                double progress = 100 / listTruck.CheckedItems.Count * (k+1);
                Color color = routecolor[manager.trucklist[indexChecked].truckid];

                for (int i = 0; i < ranks; i++)
                {
                    GMapMarker marker = new GMarkerGoogle(new  PointLatLng(manager.trucklist[indexChecked].points[i].Lat, manager.trucklist[indexChecked].points[i].Lng),(GMarkerGoogleType)(indexChecked + 1));

                    if (manager.trucklist[indexChecked].truckid < 16)
                    {
                        int row = manager.trucklist[indexChecked].truckid / 4;
                        int col = manager.trucklist[indexChecked].truckid % 4;
                        Bitmap bmpmarker = bmpnumbers.Clone(new Rectangle(new Point(20+col*42, 20+42*row), new Size(32, 32)), bmpnumbers.PixelFormat);
                        marker = new GMarkerGoogle(new GMap.NET.PointLatLng(manager.trucklist[indexChecked].points[i].Lat, manager.trucklist[indexChecked].points[i].Lng), bmpmarker);
                    }

                    marker.Tag = Convert.ToString(manager.trucklist[indexChecked].truckid + 1) + "orderid:" + Convert.ToString(manager.trucklist[indexChecked].orderidlist[i]);
                   // marker.ToolTipText = DeliverManager.orderlist[i].note;
                    markers.Markers.Add(marker);
                }

                progressBar.Value = Convert.ToInt32(progress * 0.2);

                double[,] tempdistanceMatrix = new double[DeliverManager.maxorderspertruck, DeliverManager.maxorderspertruck];

                for (int i = 0; i < ranks; i++)
                {
                    for (int j = 0; j < ranks; j++)
                    {
                        int fromorderidx = manager.trucklist[indexChecked].orderidlist[i];
                        int toorderidx = manager.trucklist[indexChecked].orderidlist[j];
                        if (manager.distanceMatrix[fromorderidx, toorderidx] == 0)
                        {
                            string lat_ori = Convert.ToString(DeliverManager.orderlist[fromorderidx].lat);
                            string lng_ori = Convert.ToString(DeliverManager.orderlist[fromorderidx].lng);

                            string lat_des = Convert.ToString(DeliverManager.orderlist[toorderidx].lat);
                            string lng_des = Convert.ToString(DeliverManager.orderlist[toorderidx].lng);

                            string sql = "select * from dist where lat_ori = " + lat_ori + " and lng_ori = " + lng_ori + " and lat_des = " + lat_des + " and lng_des = " + lng_des;
                            SQLiteCommand command = new SQLiteCommand(sql, DeliveryApp.dbConnection);
                            SQLiteDataReader sqlreader = command.ExecuteReader();
                            if (sqlreader.Read())
                            {
                                manager.distanceMatrix[fromorderidx, toorderidx] = Convert.ToDouble(sqlreader["distance"]);
                            }
                            else
                            {
                                string url = "https://maps.googleapis.com/maps/api/distancematrix/json?units=imperial&origins=";
                                url += Convert.ToString(DeliverManager.orderlist[i].lat) + " , " + Convert.ToString(DeliverManager.orderlist[fromorderidx].lng) + "&destinations=";
                                url += Convert.ToString(DeliverManager.orderlist[j].lat) + " , " + Convert.ToString(DeliverManager.orderlist[toorderidx].lng) + "&key=" + apikey;
                                string json = "";
                                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                                request.AutomaticDecompression = DecompressionMethods.GZip;
                                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                                using (Stream stream = response.GetResponseStream())
                                using (StreamReader reader = new StreamReader(stream))
                                {
                                    json = reader.ReadToEnd();
                                }
                                var matrix = new JavaScriptSerializer().Deserialize<DeliverManager.distmatrix>(json);
                                manager.distanceMatrix[fromorderidx, toorderidx] = matrix.rows[0].elements[0].distance.value;

                                sql = "insert into dist (lat_ori, lng_ori ,lat_des,lng_des, distance) values(" + lat_ori + " , " + lng_ori + " , " + lat_des + " , " + lng_des + " , " + Convert.ToString(manager.distanceMatrix[fromorderidx, toorderidx]) + ")";
                                command = new SQLiteCommand(sql, DeliveryApp.dbConnection);
                                command.ExecuteNonQuery();
                            }
                        }
                        tempdistanceMatrix[i, j] = manager.distanceMatrix[fromorderidx, toorderidx];
                    }
                    progressBar.Value = Convert.ToInt32(progress * 0.2 + progress * 0.4 / (double)ranks * (i+1));
                }
                progressBar.Value = Convert.ToInt32(progress * 0.6);
                
                if (ranks < 1) continue;

                TSP.setdistMat(tempdistanceMatrix, ranks);
                int[] sortresult = TSP.Solve();
                int[] templist = new int[DeliverManager.maxorderspertruck];
                PointLatLng[] temppoint = new PointLatLng[DeliverManager.maxorderspertruck];
                //re-arrange the points of truck accoring to the sort result
                for (int i = 0; i < ranks; i++)
                {
                    templist[i] = manager.trucklist[indexChecked].orderidlist[i];
                    temppoint[i] = manager.trucklist[indexChecked].points[i];
                }   
                for(int i = 0; i < ranks; i++)
                {
                    manager.trucklist[indexChecked].orderidlist[i] = templist[sortresult[i]];
                    manager.trucklist[indexChecked].points[i] = temppoint[sortresult[i]];
                }

                //draw route of orders to this truck

                List<PointLatLng> pointroute = new List<PointLatLng>();
                for (int i = 0; i < ranks; i++)
                {
                    progressBar.Value = Convert.ToInt32(progress * 0.6 + progress * 0.4 / (double)ranks * i);

                    string lat_ori, lng_ori, lat_des, lng_des;
                    if (i < ranks - 1)
                    {
                        lat_ori = Convert.ToString(manager.trucklist[indexChecked].points[i].Lat);
                        lng_ori = Convert.ToString(manager.trucklist[indexChecked].points[i].Lng);
                        lat_des = Convert.ToString(manager.trucklist[indexChecked].points[i+1].Lat);
                        lng_des = Convert.ToString(manager.trucklist[indexChecked].points[i+1].Lng);
                    }
                    else
                    {
                        lat_ori = Convert.ToString(manager.trucklist[indexChecked].points[i].Lat);
                        lng_ori = Convert.ToString(manager.trucklist[indexChecked].points[i].Lng);
                        lat_des = Convert.ToString(manager.trucklist[indexChecked].points[0].Lat);
                        lng_des = Convert.ToString(manager.trucklist[indexChecked].points[0].Lng);
                    }

                    string sql = "select * from route where lat_ori=" + lat_ori + " and lng_ori=" + lng_ori + " and lat_des=" + lat_des + " and lng_des=" + lng_des + " order by _order";
                    SQLiteCommand commander = new SQLiteCommand(sql, dbConnection);
                    SQLiteDataReader sqlreader = commander.ExecuteReader();
                    if(sqlreader.HasRows)
                    {
                        while(sqlreader.Read())
                        {
                            double lat = Convert.ToDouble(sqlreader["lat"]);
                            double lng = Convert.ToDouble(sqlreader["lng"]);
                            PointLatLng point = new PointLatLng(lat, lng);
                            pointroute.Add(point);
                        }
                    }
                    else
                    {
                        string json11 = string.Empty;
                        string url1 = @"https://maps.googleapis.com/maps/api/directions/json?origin=";
                        url1 = url1 + lat_ori + " , " + lng_ori + "&destination=" + lat_des + " , " + lng_des + "&key=" + apikey;
                        
                        HttpWebRequest request1 = (HttpWebRequest)WebRequest.Create(url1);
                        request1.AutomaticDecompression = DecompressionMethods.GZip;
                        using (HttpWebResponse response = (HttpWebResponse)request1.GetResponse())
                        using (Stream stream = response.GetResponseStream())
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            json11 = reader.ReadToEnd();
                        }
                        var route = new JavaScriptSerializer().Deserialize<jroute>(json11);
                        List<PointLatLng> localpoints = new List<PointLatLng>();
                        for (int s = 0; s < route.routes.Count; s++)
                        {
                            for (int t = 0; t < route.routes[s].legs.Count; t++)
                            {
                                PointLatLng startPoint = new PointLatLng(route.routes[s].legs[t].start_location.lat, route.routes[s].legs[t].start_location.lng);
                                pointroute.Add(startPoint);
                                for (int u = 0; u < route.routes[s].legs[t].steps.Count; u++)
                                {
                                    PointLatLng point = new PointLatLng(route.routes[s].legs[t].steps[u].start_location.lat, route.routes[s].legs[t].steps[u].start_location.lng);
                                    localpoints.Add(point);
                                    string polyline = route.routes[s].legs[t].steps[u].polyline.points;
                                    List<PointLatLng> pointArray = DecodePolyline(polyline);
                                    localpoints.AddRange(pointArray.ToArray());
                                }
                                PointLatLng endPoint = new PointLatLng(route.routes[s].legs[t].end_location.lat, route.routes[s].legs[t].end_location.lng);
                                localpoints.Add(endPoint);
                            }
                        }
                        pointroute.AddRange(localpoints.ToArray());
                        for(int t = 0; t < localpoints.Count; t++)
                        {
                            string lat = Convert.ToString(localpoints[t].Lat);
                            string lng = Convert.ToString(localpoints[t].Lng);

                            string insertsql = "insert into route (_order, lat_ori, lng_ori, lat_des, lng_des,lat, lng)  values(" + Convert.ToString(t) + " , " + lat_ori + " , " + lng_ori + " , " + lat_des + " , " + lng_des + " , " + lat + " , " + lng + ")";
                            SQLiteCommand insertcommand = new SQLiteCommand(insertsql, dbConnection);
                            insertcommand.ExecuteNonQuery();
                        }
                    }
                }

                GMapRoute r = new GMapRoute(pointroute.ToArray(), "MyRout" + Convert.ToString(random.Next()));
                r.Stroke = new Pen(color, 3);
                routes.Routes.Add(r);
            }

            gmap.Overlays.Add(markers);
            gmap.Overlays.Add(routes);
            gmap.Refresh();

            // update the orderlist
            listOrder.Items.Clear();
            int truckidx = listTruck.SelectedIndex;

            if (truckidx >= 0 && truckidx < manager.trucklist.Count)
            {
                for (int i = 0; i < manager.trucklist[truckidx].orderidlist.Count; i++)
                {
                    Order order = DeliverManager.orderlist[manager.trucklist[truckidx].orderidlist[i]];
                    string str = "- order " + Convert.ToString(i + 1) + " : " + order.firstname + " " + order.lastname + " : " + order.addressnumber + ", " + order.streetname;
                    listOrder.Items.Add(str);
                }
            }

            List<string> list = new List<string>();
            for (int i = 0; i < manager.trucklist.Count; i++)
            {
                string str = manager.trucklist[i].trucknumber + " : " + Convert.ToString(manager.trucklist[i].orderidlist.Count) + " orders.";
                list.Add(str);
            }
            listTruck.DataSource = list;

            progressBar.Value = Convert.ToInt32(100);
        }

        private void frmMain_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void listTruck_ItemCheck(object sender, ItemCheckEventArgs e)
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

        private void gmap_OnMarkerClick(GMapMarker item, MouseEventArgs e)
        {
            ContextMenuStrip contextMenu = new ContextMenuStrip();
            string strtag = item.Tag.ToString();
            int orderidstartidx = strtag.IndexOf("orderid:");
            if (orderidstartidx < 0) return;
            clickedtruckid = Convert.ToInt32(strtag.Substring(0, orderidstartidx));
            clickedorderid = Convert.ToInt32(strtag.Substring(orderidstartidx + 8));

            for(int i = 0; i < manager.truckcnt; i++)
            {
                if (manager.trucklist[i].truckid == clickedtruckid - 1) continue;
                string name = String.Format("move to truck{0}", i + 1);
                contextMenu.Items.Add(name).Name = name;
            }

            contextMenu.Show(e.X + DesktopLocation.X + gmap.Location.X, e.Y + DesktopLocation.Y + gmap.Location.Y);
            contextMenu.ItemClicked += new ToolStripItemClickedEventHandler(MarkerMenuHandler);

        }

        private void ListOrder_MouseClick(object sender, MouseEventArgs e)
        {
            int orderidx = listOrder.SelectedIndex;
            if (orderidx < 0) return;

            ContextMenuStrip contextMenu = new ContextMenuStrip();

            clickedtruckid = listTruck.SelectedIndex + 1;
            clickedorderid = manager.trucklist[clickedtruckid - 1].orderidlist[orderidx];
            for (int i = 0; i < manager.truckcnt; i++)
            {
                if (manager.trucklist[i].truckid == clickedtruckid - 1) continue;
                string name = String.Format("move to truck{0}", i + 1);
                contextMenu.Items.Add(name).Name = name;
            }

            contextMenu.Show(e.X + DesktopLocation.X + listOrder.Location.X, e.Y + DesktopLocation.Y + listOrder.Location.Y);
            contextMenu.ItemClicked += new ToolStripItemClickedEventHandler(MarkerMenuHandler);
        }

        private void ListTruck_MouseClick(object sender, MouseEventArgs e)
        {
            listOrder.Items.Clear();
            int truckidx = listTruck.SelectedIndex;

            if (truckidx >= 0 && truckidx < manager.trucklist.Count)
            {
                for (int i = 0; i < manager.trucklist[truckidx].orderidlist.Count; i++)
                {
                    Order order = DeliverManager.orderlist[manager.trucklist[truckidx].orderidlist[i]];
                    string str = "- order " + Convert.ToString(i + 1) + " : " + order.firstname + " " + order.lastname + " : " + order.addressnumber + ", " + order.streetname;
                    listOrder.Items.Add(str);
                }

            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            OnLoadOrder(sender, e);
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(1);
        }

        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 681);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(200, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1064, 100);
            this.panel2.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(200, 581);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1064, 100);
            this.panel3.TabIndex = 2;
            // 
            // panel4
            // 
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(200, 100);
            this.panel4.TabIndex = 3;
            // 
            // DeliveryApp
            // 
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "DeliveryApp";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Orders Organizer";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
    }
}
