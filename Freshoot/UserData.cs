namespace Freshoot
{
    internal class UserData
    {
        public static string ROLE_ADMIN = "admin";
        public static string ROLE_ORGANIZER = "organizer";
        public static string ROLE_FULFILLMENT = "fulfillment";
        public static string ROLE_DRIVER = "driver";

        public string uid { get; set; }
        public string full_name { get; set; }
        public string role { get; set; }
    }
}