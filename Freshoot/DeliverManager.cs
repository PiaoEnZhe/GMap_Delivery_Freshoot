using GMap.NET;
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

namespace Freshoot
{
    class DeliverManager
    {
        public int orderspertruck = 30;
        public static List<Order> orderlist = new List<Order>();
        public static List<Truck> trucklist = new List<Truck>();
        public static List<bool> isorderdeployed = new List<bool>();
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
            int ranks = DeliverManager.orderlist.Count;
            double[,] tempdistanceMatrix = new double[DeliverManager.maxordercnt, DeliverManager.maxordercnt];
            string lat_ori, lng_ori, lat_des, lng_des, sql;
            for (int i = 0; i < ranks; i++)
            {
                for (int j = 0; j < ranks; j++)
                {
                    int fromorderidx = i; // trucklist[indexChecked].orderidlist[i];
                    int toorderidx = j; // trucklist[indexChecked].orderidlist[j];
                    if (distanceMatrix[fromorderidx, toorderidx] == 0)
                    {
                        lat_ori = Convert.ToString(DeliverManager.orderlist[fromorderidx].lat);
                        lng_ori = Convert.ToString(DeliverManager.orderlist[fromorderidx].lng);

                        lat_des = Convert.ToString(DeliverManager.orderlist[toorderidx].lat);
                        lng_des = Convert.ToString(DeliverManager.orderlist[toorderidx].lng);

                        sql = "select * from dist where lat_ori = " + lat_ori + " and lng_ori = " + lng_ori + " and lat_des = " + lat_des + " and lng_des = " + lng_des;
                        SQLiteCommand command = new SQLiteCommand(sql, OrdersOrganizer.dbConnection);
                        SQLiteDataReader sqlreader = command.ExecuteReader();
                        if (sqlreader.Read())
                        {
                            distanceMatrix[fromorderidx, toorderidx] = Convert.ToDouble(sqlreader["distance"]);
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
                            distanceMatrix[fromorderidx, toorderidx] = matrix.rows[0].elements[0].distance.value;

                            sql = "insert into dist (lat_ori, lng_ori ,lat_des,lng_des, distance) values(" + lat_ori + " , " + lng_ori + " , " + lat_des + " , " + lng_des + " , " + Convert.ToString(distanceMatrix[fromorderidx, toorderidx]) + ")";
                            command = new SQLiteCommand(sql, OrdersOrganizer.dbConnection);
                            command.ExecuteNonQuery();
                        }
                    }
                    tempdistanceMatrix[i, j] = distanceMatrix[fromorderidx, toorderidx];
                }
            }

          
            TSP.setdistMat(tempdistanceMatrix, ranks);
            int[] sortresult = TSP.Solve();
            //re-arrange the points of truck accoring to the sort result
            int nTruckId = 0;
            int nOrdersInTruck = 0;
            Truck truck = new Truck(0);
            trucklist.Add(truck);
            for (int i = 0; i < ranks; i++, nOrdersInTruck++)
            {
                if(nOrdersInTruck == orderspertruck)
                {
                    nOrdersInTruck = 0;
                    nTruckId++;

                    Truck newtruck = new Truck(nTruckId);
                    trucklist.Add(newtruck);
                }
                trucklist[nTruckId].AddOrder(sortresult[i]);
            }
        }

        public bool AddOrderToTruck(int truckid, int orderid)
        {
            trucklist[truckid].AddOrder(orderid);
            return true;
        }

        public bool DeleteOrderFromTruck(int truckid, int orderid)
        {
            trucklist[truckid].DelOrder(orderid);
            return true;
        }

        public void LoadDistance()
        {
            for(int i = 0; i < DeliverManager.orderlist.Count; i++)
            {
                for (int j = 0; j < DeliverManager.orderlist.Count; j++)
                {
                    string lat_ori = Convert.ToString(orderlist[i].lat);
                    string lng_ori = Convert.ToString(orderlist[i].lng);

                    string lat_des = Convert.ToString(orderlist[j].lat);
                    string lng_des = Convert.ToString(orderlist[j].lng);

                    string sql = "select * from dist where lat_ori = " + lat_ori + " and lng_ori = " + lng_ori + " and lat_des = " + lat_des + " and lng_des = " + lng_des;
                    SQLiteCommand command = new SQLiteCommand(sql, OrdersOrganizer.dbConnection);
                    SQLiteDataReader sqlreader = command.ExecuteReader();
                    if (sqlreader.Read())
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
                        command = new SQLiteCommand(sql, OrdersOrganizer.dbConnection);
                        command.ExecuteNonQuery();
                    }
                }
             }
        }
    }
}
