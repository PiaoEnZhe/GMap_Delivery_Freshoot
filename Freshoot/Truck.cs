using GMap.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DelveryManager_CSharp
{
    class Truck
    {
        public int truckid;
        public string trucknumber;
        public List<int> orderidlist = new List<int>();
        public List<PointLatLng> points = new List<PointLatLng>();
        public void AddOrder(int orderid, double lat, double lng)
        {
            int idx = orderidlist.IndexOf(orderid);
            if(idx >= 0)
            {
                MessageBox.Show("The order is already in the truck!");
                return;
            }
            orderidlist.Add(orderid);
            points.Add(new PointLatLng(lat, lng));
        }

        public void ArrangeOrders()
        {
            // arrange the orders using dijskstra algorithm to make the best path....

        }

        public void DelOrder(int orderid)
        {
            int idx = orderidlist.IndexOf(orderid);
            if(idx >= 0)
            {
                orderidlist.RemoveAt(idx);
                points.RemoveAt(idx);
            }
        }
    }
}
