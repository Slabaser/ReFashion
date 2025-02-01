namespace ECommerceApp.Services
{
    public class MongoDBSettings : IMongoDBSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string ProductsCollection { get; set; }
        public string UsersCollection { get; set; }
        public string CategoryCollection { get; set; }
        public string WishlistsCollection { get; set; } // Wishlists koleksiyonu eklendi
        public string CartsCollection { get; set; }
        public string ReviewsCollection { get; set; }
        public string AddressesCollection { get; set; }
        public string OrdersCollection { get; set; }
        public string PaymentsCollection { get; set; }
        public string ShippingsCollection { get; set; }

        // Parametresiz yapıcı metot (Varsayılan değerler atanabilir)
        public MongoDBSettings()
        {
            CategoryCollection = "Categories";
            WishlistsCollection = "Wishlists";
            CartsCollection = "Carts";
            AddressesCollection = "Addresses";
            OrdersCollection = "Orders";
            PaymentsCollection = "Payments";
            ShippingsCollection = "Shippings";
            ReviewsCollection = "Reviews";
        }

        // Parametreli yapıcı metot
        public MongoDBSettings(
            string connectionString,
            string databaseName,
            string productsCollection,
            string usersCollection,
            string categoryCollection = "Categories",
            string wishlistsCollection = "Wishlists",
            string cartsCollection = "Carts",
            string addressesCollection = "Addresses",
            string ordersCollection = "Orders",
            string paymentsCollection = "Payments",
            string shippingsCollection = "Shippings",
            string reviewsCollection = "Reviews")
        {
            ConnectionString = connectionString;
            DatabaseName = databaseName;
            ProductsCollection = productsCollection;
            UsersCollection = usersCollection;
            CategoryCollection = categoryCollection;
            WishlistsCollection = wishlistsCollection;
            CartsCollection = cartsCollection;
            AddressesCollection = addressesCollection;
            OrdersCollection = ordersCollection;
            PaymentsCollection = paymentsCollection;
            ShippingsCollection = shippingsCollection;
            ReviewsCollection = reviewsCollection;

        }
    }
}
