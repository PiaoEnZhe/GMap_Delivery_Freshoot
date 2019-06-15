using GMap.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Freshoot
{
    class Truck
    {
        public int truckid;
        public string trucknumber;
        public double distance;
        public double speed;
        public bool isdistcalced;
        public List<int> orderidlist = new List<int>();

        public Truck(int _truckid)
        {
            truckid = _truckid;
            char ch = Convert.ToChar(65 + _truckid);
            trucknumber = "" + ch;
            distance = 0;
            speed = 40;
            isdistcalced = false;
        }

        public void PlusOrder(int orderid) // add order minused from next truck to the tail of order list
        {
            AddOrder(orderid);
        }

        public int MinusOrder() // remove the first order to the previous truck
        {
            int res = orderidlist[0];
            for(int i = 1; i < orderidlist.Count; i++)
            {
                orderidlist[i - 1] = orderidlist[i];
            }
            orderidlist.RemoveAt(orderidlist.Count - 1); // delete the last item
            return res;
         }

        public void AddOrder(int orderid)
        {
            int idx = orderidlist.IndexOf(orderid);
            if(idx >= 0)
            {
                MessageBox.Show("The order is already in the truck!");
                return;
            }
            orderidlist.Add(orderid);
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
            }
        }
    }
}
