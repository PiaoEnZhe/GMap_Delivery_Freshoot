
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Google.OrTools.ConstraintSolver;

namespace DelveryManager_CSharp
{
    public class TSP
    {
        public static int VehicleNumber = 1;
        public static int Depot = 0;
        public static int Ranks = 0;
        public static double[,] distMat = new double[200,200];
        /// <summary>
        ///   Euclidean distance implemented as a callback. It uses an array of
        ///   positions and computes the Euclidean distance between the two
        ///   positions of two different indices.
        /// </summary>
        public static void setdistMat(double[,] array, int rank)
        {
            for (int i = 0; i < rank; i++)
                for (int j = 0; j < rank; j++)
                    distMat[i, j] = array[i, j];
            Ranks = rank;
        }

        /// <summary>
        ///   Print the solution.
        /// </summary>
  

        public static int[] Solve()
        {
            int[] result = new int[200];
            int cnt = 0;
            // Create Routing Index Manager
            RoutingIndexManager manager = new RoutingIndexManager(Ranks, VehicleNumber, Depot);

            // Create Routing Model.
            RoutingModel routing = new RoutingModel(manager);

            int transitCallbackIndex = routing.RegisterTransitCallback(
              (long fromIndex, long toIndex) => {
              // Convert from routing variable Index to distance matrix NodeIndex.
              var fromNode = manager.IndexToNode(fromIndex);
                  var toNode = manager.IndexToNode(toIndex);
                  return (long)distMat[fromNode, toNode];
              }
            );

            routing.SetArcCostEvaluatorOfAllVehicles(transitCallbackIndex);

            // Setting first solution heuristic.
            RoutingSearchParameters searchParameters =
              operations_research_constraint_solver.DefaultRoutingSearchParameters();
            searchParameters.FirstSolutionStrategy =
              FirstSolutionStrategy.Types.Value.PathCheapestArc;

            // Solve the problem.
            Assignment solution = routing.SolveWithParameters(searchParameters);

            // Inspect solution.
            var index = routing.Start(0);
            while (routing.IsEnd(index) == false)
            {
                result[cnt++] = Convert.ToInt32(index);
                var previousIndex = index;
                index = solution.Value(routing.NextVar(index));
            }
            return result;
        }
    }
}

//namespace DelveryManager_CSharp
//{
//    public class tspParams : NodeEvaluator2
//    {
//        public static int size = 0;
//        public static double[,] distMat = new double[100, 100];     

//        public static void setdistMat(double[,] array, int rank)
//        {
//            for (int i = 0; i < rank; i++)
//                for (int j = 0; j < rank; j++)
//                    distMat[i, j] = array[i, j];
//            size = rank;
//        }
//        public static int tsp_size
//        {
//            get { return size; }
//        }

//        public static int num_routes
//        {
//            get { return 1; }
//        }

//        public static int depot
//        {
//            get { return 0; }
//        }

//        public override long Run(int i, int j) // i : fromidx, j: toidx
//        {

//            return (long)distMat[i, j];

//            string lat_ori = Convert.ToString(DeliverManager.orderlist[i].lat);
//            string lng_ori = Convert.ToString(DeliverManager.orderlist[i].lng);

//            string lat_des = Convert.ToString(DeliverManager.orderlist[j].lat);
//            string lng_des = Convert.ToString(DeliverManager.orderlist[j].lng);

//            string sql = "select * from dist where lat_ori = " + lat_ori + " and lng_ori = " + lng_ori + " and lat_des = " + lat_des + " and lng_des = " + lng_des;
//            SQLiteCommand command = new SQLiteCommand(sql, Form1.dbConnection);
//            SQLiteDataReader sqlreader = command.ExecuteReader();
//            double dist = 0;
//            if (sqlreader.Read())
//            {
//                dist = Convert.ToDouble(sqlreader["distance"]);
//            }
//            else
//            {
//                string url = "https://maps.googleapis.com/maps/api/distMat/json?units=imperial&origins=";
//                url += Convert.ToString(DeliverManager.orderlist[i].lat) + " , " + Convert.ToString(DeliverManager.orderlist[i].lng) + "&destinations=";
//                url += Convert.ToString(DeliverManager.orderlist[j].lat) + " , " + Convert.ToString(DeliverManager.orderlist[j].lng) + "&key=" + DeliverManager.apikey;
//                string json = "";
//                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
//                request.AutomaticDecompression = DecompressionMethods.GZip;
//                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
//                using (Stream stream = response.GetResponseStream())
//                using (StreamReader reader = new StreamReader(stream))
//                {
//                    json = reader.ReadToEnd();
//                }
//                var matrix = new JavaScriptSerializer().Deserialize<DeliverManager.distmatrix>(json);
//                dist = matrix.rows[0].elements[0].distance.value;

//                sql = "insert into dist (lat_ori, lng_ori ,lat_des,lng_des, distance) values(" + lat_ori + " , " + lng_ori + " , " + lat_des + " , " + lng_des + " , " + Convert.ToString(dist) + ")";
//                command = new SQLiteCommand(sql, Form1.dbConnection);
//                command.ExecuteNonQuery();
//            }
//            return (long)dist;
//        }
//    }

//    public class TSP
//    {
//        public static int[] PrintSolution(RoutingModel routing, Assignment solution)
//        {
//            int[] result = new int[100];
//            Console.WriteLine("Distance of the route: {0}", solution.ObjectiveValue());

//            var index = routing.Start(0);

//            Console.WriteLine("Route for Vehicle 0:");
//            int cnt = 0;
//            while (!routing.IsEnd(index))
//            {
//                result[cnt++] = routing.IndexToNode(index);
//                Console.Write("{0} -> ", routing.IndexToNode(index));
//                var previousIndex = index;
//                index = solution.Value(routing.NextVar(index));
//            }
//            Console.WriteLine("{0}", routing.IndexToNode(index));
//            //Console.WriteLine("Calculated optimal route!");
//            return result;
//        }

//        public static int[] Solve()
//        {
//            // Create Routing Model
//            RoutingModel routing = new RoutingModel(tspParams.tsp_size, tspParams.num_routes, tspParams.depot);

//            // Define weight of each edge
//            NodeEvaluator2 distanceEvaluator = new tspParams();

//            //protect callbacks from the GC
//            GC.KeepAlive(distanceEvaluator);
//            routing.SetArcCostEvaluatorOfAllVehicles(distanceEvaluator);

//            // Setting first solution heuristic (cheapest addition).
//            RoutingSearchParameters searchParameters = RoutingModel.DefaultSearchParameters();
//            searchParameters.FirstSolutionStrategy = FirstSolutionStrategy.Types.Value.PathCheapestArc;

//            Assignment solution = routing.SolveWithParameters(searchParameters);
//            return PrintSolution(routing, solution);


//        }
//    }
//}
