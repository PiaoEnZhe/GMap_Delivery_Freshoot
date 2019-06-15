namespace Freshoot
{
    class UserData
    {
        public static string ROLE_ADMIN = "admin";
        public static string ROLE_ORGANIZER = "organizer";
        public static string ROLE_FULFILLMENT = "fulfillment";
        public static string ROLE_DRIVER = "driver";

        public string uid { get; set; }
        public string full_name { get; set; }
        public string role { get; set; }
    }
    
    class Order
    {
        public int orderid;
        public string firstname, lastname;
        public string customerid;
        public string unitnumber;
        public string addressnumber;
        public string streetname;
        public string postalcode;
        public string city;
        public string note;
        public double lat;
        public double lng;

        public string order_code = "";

        public string description;

        public int truckid;
        public int truckNo;

        public OrderBin[] bins = new OrderBin[] { };

        public int getCheckedCount()
        {
            var result = 0;
            for (int i = 0; i < bins.Length; i++)
            {
                if (bins[i].isChecked) {
                    result++;
                }
            }
            return result;
        }

        public int getRemainingCount()
        {
            var result = 0;
            for (int i = 0; i < bins.Length; i++)
            {
                if (!bins[i].isChecked)
                {
                    result++;
                }
            }
            return result;
        }

        public Product[] getProducts() {
            var productCount = 0;
            for (int i = 0; i < bins.Length; i++) {
                productCount += bins[i].products.Length;
            }
            var result = new Product[productCount];
            var index = 0;
            for (int i = 0; i < bins.Length; i++)
            {
                for (int j = 0; j < bins[i].products.Length; j++) {
                    result[index] = bins[i].products[j];
                    index++;
                }
            }

            return result;
        }

        public static Order[] getSampleData()
        {
            Order item1 = new Order();
            item1.orderid = 0;
            item1.firstname = "Neil";
            item1.lastname = "Contreras";
            item1.customerid = "NEICON01";
            item1.unitnumber = "108";
            item1.addressnumber = "5320";
            item1.streetname = "Lakeview Dr SW";
            item1.postalcode = "T3E 6L5";
            item1.city = "Calgary";
            item1.note = "Please Put the bins in the back door 403 999 3094";
            item1.order_code = "20190612PB22";

            item1.bins = OrderBin.getSampleData1(item1.orderid);

            Order item2 = new Order();
            item2.orderid = 0;
            item2.firstname = "Reid";
            item2.lastname = "Moon";
            item2.customerid = "REIMOO01";
            item2.unitnumber = "312";
            item1.addressnumber = "2528";
            item2.streetname = " 66 Ave SW";
            item2.postalcode = "T3E 5K3";
            item2.city = "Calgary";
            item2.note = "Please Put the bins in the back door 403 999 3105";
            item2.order_code = "20190612PA32";

            item2.bins = OrderBin.getSampleData2(item2.orderid);

            return new Order[] { item1, item2};
        }

        public static Order getOrder(Order[] data, string bin_barcode) {
            for (int i = 0; i < data.Length; i++) {
                for (int j = 0; j < data[i].bins.Length; j++) {
                    if (bin_barcode.Equals(data[i].bins[j].barcode)) {
                        return data[i];
                    }
                }
            }
            return null;
        }

        public static OrderBin getOrderBin(Order[] data, string bin_barcode)
        {
            for (int i = 0; i < data.Length; i++)
            {
                for (int j = 0; j < data[i].bins.Length; j++)
                {
                    if (bin_barcode.Equals(data[i].bins[j].barcode))
                    {
                        return data[i].bins[j];
                    }
                }
            }
            return null;
        }
    }

    class OrderBin
    {
        public string barcode = "";
        public int orderId = 0;
        public int binNo = 0;
        public int laneNo = 0;
        public string date = "";

        public bool isChecked = false;

        public Product[] products = new Product[] { };

        public static OrderBin[] getSampleData1(int order_id)
        {
            OrderBin item1 = new OrderBin();
            item1.barcode = "8676000590005";
            item1.orderId = order_id;
            item1.binNo = 1;
            item1.laneNo = 5;
            item1.products = Product.getSampleData();

            OrderBin item2 = new OrderBin();
            item2.barcode = "6937131700018";
            item2.orderId = order_id;
            item2.binNo = 2;
            item2.laneNo = 3;
            item2.products = Product.getSampleData();

            return new OrderBin[] { item1, item2};
        }
        public static OrderBin[] getSampleData2(int order_id)
        {
            OrderBin item1 = new OrderBin();
            item1.barcode = "6949123301851";
            item1.orderId = order_id;
            item1.binNo = 1;
            item1.laneNo = 5;
            item1.products = Product.getSampleData();

            OrderBin item2 = new OrderBin();
            item2.barcode = "6949123301852";
            item2.orderId = order_id;
            item2.binNo = 2;
            item2.laneNo = 3;
            item2.products = Product.getSampleData();

            return new OrderBin[] { item1, item2};
        }
    }

    class Product
    {
        public static int STATUS_NORMAL = 0;
        public static int STATUS_LOW_QUALITY = 1;
        public static int STATUS_SEC_REPLACE = 2;

        public string barcode = "";
        public string title = "";
        public string image_url = "";

        public float price = 0;
        public float price_l_quality = 0.4f;
        public float price_secondary = 0.8f;

        public int status = STATUS_NORMAL;
        public string secondary_barcode = "";

        public float weight_by_pkg = 1.0f;
        public int pkg_count = 1;

        public float getPrice() {
            return price;
        }

        public static Product[] getSampleData() {
            Product product1 = new Product();
            product1.barcode = "33455ffe66";
            product1.title = "Apples";
            product1.price = 4.0f;

            Product product2 = new Product();
            product2.barcode = "33455ffe66";
            product2.title = "Cucumbers";
            product2.price = 4.0f;

            return new Product[] { product1, product2, product1 };
        }
    }
}