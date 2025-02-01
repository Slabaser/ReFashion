namespace ECommerceApp.Services
{
    public interface IMongoDBSettings
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
        string ProductsCollection { get; set; }
        string CategoryCollection { get; set; }
        string UsersCollection { get; set; }
        string WishlistsCollection { get; set; }
        string CartsCollection { get; set; }
        string AddressesCollection { get; set; }
        string OrdersCollection { get; set; }
        string PaymentsCollection { get; set; }
        string ShippingsCollection { get; set; }
        string ReviewsCollection { get; set; }
    }
}
