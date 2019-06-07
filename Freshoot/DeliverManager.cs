using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace DelveryManager_CSharp
{
    class DeliverManager
    {
        public int truckcnt = 0;
        public int ordercnt = 0;
        public int orderspertruck = 30;
        public static List<Order> orderlist = new List<Order>();
        public List<Truck> trucklist = new List<Truck>();
        public List<bool> isorderdeployed = new List<bool>();
        public static string apikey = "AIzaSyBYRn4-66u_0wwM6SJwuZhVE_BjeKZ0AmY";

        public static int maxorderspertruck = 40;
        public static int maxordercnt = 500;
        public double[,] distanceMatrix = new double[maxordercnt, maxordercnt];

        public class distance
        {
            public int orderid { get; set; }
            public double dist { get; set; }
        }

        public class jdata
        {
            public string text { get; set; }
            public int value { get; set; }
        }
        public class jelement
        {
            public jdata distance { get; set; }
            public jdata duration { get; set; }
            public string status { get; set; }
        }

        public class jelements
        {
            public List<jelement> elements { get; set; }
        }

        public class distmatrix
        {
            public List<string> destination_addresses { get; set; }
            public List<string> origin_addresses { get; set; }
            public List<jelements> rows { get; set; }
            public string status { get; set; }
        }

        public void InitManager()
        {
            isorderdeployed.Clear();
            orderlist.Clear();
            truckcnt = 0;
            ordercnt = 0;
            orderspertruck = 30;
            orderlist.Clear();
            trucklist.Clear();
            distanceMatrix = new double[maxordercnt, maxordercnt];
        }

        public bool ReadOrdersFromFile(string filename)
        {
            try
            {
                InitManager();
                System.IO.StreamReader file = new System.IO.StreamReader(filename);
                string line;
                int cnt = 0;
                line = file.ReadLine(); // read out the header row
                while ((line = file.ReadLine()) != null)
                {
                    Order order = new Order();
                    string temp;
                    int index = 0;
                    index = line.IndexOf(','); if (index < 0) continue; temp = line.Substring(0, index); order.firstname = temp; line = line.Substring(index + 1);
                    index = line.IndexOf(','); if (index < 0) continue; temp = line.Substring(0, index); order.lastname = temp; line = line.Substring(index + 1);
                    index = line.IndexOf(','); if (index < 0) continue; temp = line.Substring(0, index); order.customerid = temp; line = line.Substring(index + 1);
                    index = line.IndexOf(','); if (index < 0) continue; temp = line.Substring(0, index); order.unitnumber = temp; line = line.Substring(index + 1);
                    index = line.IndexOf(','); if (index < 0) continue; temp = line.Substring(0, index); order.addressnumber = temp; line = line.Substring(index + 1);
                    index = line.IndexOf(','); if (index < 0) continue; temp = line.Substring(0, index); order.streetname = temp; line = line.Substring(index + 1);
                    index = line.IndexOf(','); if (index < 0) continue; temp = line.Substring(0, index); order.postalcode = temp; line = line.Substring(index + 1);
                    index = line.IndexOf(','); if (index < 0) continue; temp = line.Substring(0, index); order.city = temp; line = line.Substring(index + 1);
                    order.note = line;
                    order.orderid = cnt++;
                    orderlist.Add(order);
                    isorderdeployed.Add(false);
                }
                ordercnt = cnt;
            }
            catch(Exception e1)
            {
                return false;
            }
            return true;
        }

        public bool ReadTrucksFromFile(string filename)
        {

            return true;
        }

        public void AutoDeploy()
        {
            truckcnt = (ordercnt - 1) / orderspertruck + 1;
            double r = 500;
            double theta = 360 / truckcnt;
            int orderstodeploy = ordercnt;
            if(truckcnt > 16)
            {
                MessageBox.Show("The Maximum Truck Count is 16.");
                return; 
            }

            for(int i = 0; i < truckcnt; i++)
            {
                Truck truck = new Truck();
                truck.truckid = i;
                truck.trucknumber = "truck" + Convert.ToString(i + 1);

                double x0 = r * Math.Cos(theta * i);
                double y0 = r * Math.Sin(theta * i);

                List<distance> distancelist = new List<distance>();
                for(int j = 0; j < ordercnt; j++)
                {
                    if(!isorderdeployed[j])
                    {
                        distance dis = new distance();
                        dis.orderid = j;
                        dis.dist = (orderlist[j].lat - x0) * (orderlist[j].lat - x0) + (orderlist[j].lng - y0) * (orderlist[j].lng - y0);
                        distancelist.Add(dis);
                    }
                }

                // sort the distance list according to the dist

                for(int j = 0; j < orderstodeploy; j++)
                {
                    for(int k = j + 1; k < orderstodeploy; k++)
                    {
                        if(distancelist[j].dist > distancelist[k].dist)
                        {
                            distance dis = new distance();
                            dis = distancelist[j]; distancelist[j] = distancelist[k]; distancelist[k] = dis;
                        }
                    }
                }

                // take ordespertruck orders from the sorted result.

                int cnt = Math.Min(orderstodeploy, orderspertruck);
                for(int j = 0; j < cnt; j++)
                {
                    truck.AddOrder(distancelist[j].orderid, orderlist[distancelist[j].orderid].lat, orderlist[distancelist[j].orderid].lng);
                    orderlist[distancelist[j].orderid].truckid = i;
                    isorderdeployed[distancelist[j].orderid] = true;
                    orderstodeploy--;
                }

                trucklist.Add(truck);
            }
        }

        public bool AddOrderToTruck(int truckid, int orderid)
        {
            trucklist[truckid].AddOrder(orderid, orderlist[orderid].lat, orderlist[orderid].lng);
            return true;
        }

        public bool DeleteOrderFromTruck(int truckid, int orderid)
        {
            trucklist[truckid].DelOrder(orderid);
            return true;
        }

        public void LoadDistance()
        {
            for(int i = 0; i < ordercnt; i++)
                for(int j = 0; j < ordercnt; j++)
                {
                    string lat_ori = Convert.ToString(orderlist[i].lat);
                    string lng_ori = Convert.ToString(orderlist[i].lng);

                    string lat_des = Convert.ToString(orderlist[j].lat);
                    string lng_des = Convert.ToString(orderlist[j].lng);

                    string sql = "select * from dist where lat_ori = " + lat_ori + " and lng_ori = " + lng_ori + " and lat_des = " + lat_des + " and lng_des = " + lng_des;
                    SQLiteCommand command = new SQLiteCommand(sql, DeliveryApp.dbConnection);
                    SQLiteDataReader sqlreader = command.ExecuteReader();
                    if(sqlreader.Read())
                    {
                        distanceMatrix[i, j] = Convert.ToDouble(sqlreader["distance"]);
                    }
                    else
                    {
                        string url = "https://maps.googleapis.com/maps/api/distancematrix/json?units=imperial&origins=";
                        url += Convert.ToString(orderlist[i].lat) + " , " + Convert.ToString(orderlist[i].lng) + "&destinations=";
                        url += Convert.ToString(orderlist[j].lat) + " , " + Convert.ToString(orderlist[j].lng) + "&key=" + apikey;
                        string json = "";
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                        request.AutomaticDecompression = DecompressionMethods.GZip;
                        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                        using (Stream stream = response.GetResponseStream())
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            json = reader.ReadToEnd();
                        }
                        var matrix = new JavaScriptSerializer().Deserialize<distmatrix>(json);
                        distanceMatrix[i, j] = matrix.rows[0].elements[0].distance.value;

                        sql = "insert into dist (lat_ori, lng_ori ,lat_des,lng_des, distance) values(" + lat_ori + " , " + lng_ori + " , " + lat_des + " , " + lng_des + " , " + Convert.ToString(distanceMatrix[i, j]) + ")";
                        command = new SQLiteCommand(sql, DeliveryApp.dbConnection);
                        command.ExecuteNonQuery();
                    }
                }

            //    int blocks = ordercnt / 10; // google api can accept 10 address at one time
            //    int remain = ordercnt % 10;

            //    for (int i = 0; i < blocks; i++)
            //    {
            //        for(int j = 0; j < blocks; j++)
            //        {
            //            string url = "https://maps.googleapis.com/maps/api/distancematrix/json?units=imperial&origins=";
            //            int k = 0, l = 0;
            //            for (k = i * 10; k < (i + 1) * 10 - 1; k++)
            //                url += Convert.ToString(orderlist[k].lat) + " , " + Convert.ToString(orderlist[k].lng) + '|';
            //            url += Convert.ToString(orderlist[k].lat) + " , " + Convert.ToString(orderlist[k].lng) + "&destinations=";
            //            for (l = j * 10; l < (j + 1) * 10 - 1; l++)
            //                url += Convert.ToString(orderlist[l].lat) + " , " + Convert.ToString(orderlist[l].lng) + '|';
            //            url += Convert.ToString(orderlist[l].lat) + " , " + Convert.ToString(orderlist[l].lng) + "&key=" + apikey;
            //            string json = "";
            //            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            //            request.AutomaticDecompression = DecompressionMethods.GZip;
            //            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            //            using (Stream stream = response.GetResponseStream())
            //            using (StreamReader reader = new StreamReader(stream))
            //            {
            //                json = reader.ReadToEnd();
            //            }
            //            var matrix = new JavaScriptSerializer().Deserialize<distmatrix>(json);
            //            for(int s = 0; s < matrix.rows.Count; s++)
            //            {
            //                for(int t = 0; t < matrix.rows[s].elements.Count; t++)
            //                {
            //                    distanceMatrix[i*10+s, j*10+t] = matrix.rows[s].elements[t].distance.value;
            //                }
            //            }
            //        }

            //       {
            //            string url = "https://maps.googleapis.com/maps/api/distancematrix/json?units=imperial&origins=";
            //            int k = 0, l = 0;
            //            for (k = i * 10; k < (i + 1) * 10 - 1; k++)
            //                url += Convert.ToString(orderlist[k].lat) + " , " + Convert.ToString(orderlist[k].lng) + '|';
            //            url += Convert.ToString(orderlist[k].lat) + " , " + Convert.ToString(orderlist[k].lng) + "&destinations=";
            //            for (l = 10 * blocks; l < ordercnt - 1; l++)
            //                url += Convert.ToString(orderlist[l].lat) + " , " + Convert.ToString(orderlist[l].lng) + '|';
            //            url += Convert.ToString(orderlist[l].lat) + " , " + Convert.ToString(orderlist[l].lng) + "&key=" + apikey;
            //            string json = "";
            //            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            //            request.AutomaticDecompression = DecompressionMethods.GZip;
            //            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            //            using (Stream stream = response.GetResponseStream())
            //            using (StreamReader reader = new StreamReader(stream))
            //            {
            //                json = reader.ReadToEnd();
            //            }
            //            var matrix = new JavaScriptSerializer().Deserialize<distmatrix>(json);
            //            for (int s = 0; s < matrix.rows.Count; s++)
            //            {
            //                for (int t = 0; t < matrix.rows[s].elements.Count; t++)
            //                {
            //                    distanceMatrix[i * 10 + s, blocks * 10 + t] = matrix.rows[s].elements[t].distance.value;
            //                }
            //            }
            //        }
            //    }
            //    for (int j = 0; j < blocks; j++)
            //    {
            //        string url = "https://maps.googleapis.com/maps/api/distancematrix/json?units=imperial&origins=";
            //        int k = 0, l = 0;
            //        for (k = 10 * blocks; k < ordercnt - 1; k++)
            //            url += Convert.ToString(orderlist[k].lat) + " , " + Convert.ToString(orderlist[k].lng) + '|';
            //        url += Convert.ToString(orderlist[k].lat) + " , " + Convert.ToString(orderlist[k].lng) + "&destinations=";
            //        for (l = j * 10; l < (j + 1) * 10 - 1; l++)
            //            url += Convert.ToString(orderlist[l].lat) + " , " + Convert.ToString(orderlist[l].lng) + '|';
            //        url += Convert.ToString(orderlist[l].lat) + " , " + Convert.ToString(orderlist[l].lng) + "&key=" + apikey;
            //        string json = "";
            //        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            //        request.AutomaticDecompression = DecompressionMethods.GZip;
            //        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            //        using (Stream stream = response.GetResponseStream())
            //        using (StreamReader reader = new StreamReader(stream))
            //        {
            //            json = reader.ReadToEnd();
            //        }
            //        var matrix = new JavaScriptSerializer().Deserialize<distmatrix>(json);
            //        for (int s = 0; s < matrix.rows.Count; s++)
            //        {
            //            for (int t = 0; t < matrix.rows[s].elements.Count; t++)
            //            {
            //                distanceMatrix[blocks * 10 + s, j * 10 + t] = matrix.rows[s].elements[t].distance.value;
            //            }
            //        }
            //    }
            //    {
            //        string url = "https://maps.googleapis.com/maps/api/distancematrix/json?units=imperial&origins=";
            //        int k = 0, l = 0;
            //        for (k = blocks * 10; k < ordercnt - 1; k++)
            //            url += Convert.ToString(orderlist[k].lat) + " , " + Convert.ToString(orderlist[k].lng) + '|';
            //        url += Convert.ToString(orderlist[k].lat) + " , " + Convert.ToString(orderlist[k].lng) + "&destinations=";
            //       // url += orderlist[k].city + "+" + orderlist[k].addressnumber + "+" + orderlist[k].streetname + "&destinations=";

            //        for (l = blocks * 10; l < ordercnt - 1; l++)
            //            url += orderlist[l].city + "+" + orderlist[l].addressnumber + "+" + orderlist[l].streetname + "|";

            //        url += Convert.ToString(orderlist[l].lat) + " , " + Convert.ToString(orderlist[l].lng) + "&key=" + apikey;

            //        string json = "";
            //        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            //        request.AutomaticDecompression = DecompressionMethods.GZip;
            //        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            //        using (Stream stream = response.GetResponseStream())
            //        using (StreamReader reader = new StreamReader(stream))
            //        {
            //            json = reader.ReadToEnd();
            //        }
            //        var matrix = new JavaScriptSerializer().Deserialize<distmatrix>(json);
            //        for (int s = 0; s < matrix.rows.Count; s++)
            //        {
            //            for (int t = 0; t < matrix.rows[s].elements.Count; t++)
            //            {
            //                distanceMatrix[blocks * 10 + s, blocks * 10 + t] = matrix.rows[s].elements[t].distance.value;
            //            }
            //        }
            //    }

        }
    }
}
