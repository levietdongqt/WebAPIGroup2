namespace WebAPIGroup2.Models
{
    public enum Enum
    {

    }
    public static class UserStatus
    {
        public const string Pending = "Pending";
        public const string Enabled = "Enabled";
        public const string Disabled = "Disabled";

    }

    public static class PurchaseStatus
    {
        public const string Temporary = "Temporary";
        public const string OrderPlaced = "Order Placed";
        public const string OrderPaid = "Order Paid";
        public const string ToShip = "To Ship";
    }


}
