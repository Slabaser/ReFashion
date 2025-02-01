using ECommerceApp.Models;
using ECommerceApp.Repositories;
using MongoDB.Bson;
using System.Linq;
using MongoDB.Driver;


namespace ECommerceApp.Services
{
    public class WishlistService
    {
        private readonly WishlistRepository _wishlistRepository;
        private readonly ProductRepository _productRepository;

        public WishlistService(WishlistRepository wishlistRepository, ProductRepository productRepository)
        {
            _wishlistRepository = wishlistRepository;
            _productRepository = productRepository;
        }

        public Wishlist GetWishlistByUserId(string userId)
        {
            return _wishlistRepository.GetWishlistByUserId(userId);
        }

        public Wishlist GetWishlistByUserIdWithDetails(string userId)
        {
            // Kullanıcının wishlist'ini getir
            var wishlist = _wishlistRepository.GetWishlistByUserId(userId);

            if (wishlist != null && wishlist.Items != null)
            {
                foreach (var item in wishlist.Items)
                {
                    // Ürünün detaylarını Products koleksiyonundan getir
                    var product = _productRepository.GetProductById(item.ProductId);
                    if (product != null)
                    {
                        item.ProductName = product.Name;
                        item.ProductPrice = product.Price;
                        item.ImageUrl = product.ImageUrl; // Resim URL'si dinamik olarak ekleniyor
                    }
                }
            }


            return wishlist;
        }

        public void AddToWishlist(string userId, string productId, string productName, decimal productPrice)
        {
            var wishlist = _wishlistRepository.GetWishlistByUserId(userId);

            if (wishlist == null)
            {
                wishlist = new Wishlist
                {
                    UserId = userId,
                    Items = new List<WishlistItem>()
                };

                _wishlistRepository.CreateWishlist(wishlist);
            }

            var existingItem = wishlist.Items.FirstOrDefault(i => i.ProductId == productId);
            if (existingItem == null)
            {
                var newItem = new WishlistItem
                {
                    ProductId = productId,
                    ProductName = productName,
                    ProductPrice = productPrice,
                    AddedAt = DateTime.UtcNow
                };

                _wishlistRepository.AddItemToWishlist(userId, newItem);
            }
        }

        public void RemoveFromWishlist(string userId, string productId)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(productId))
            {
                throw new ArgumentException("UserId or ProductId is missing.");
            }

            _wishlistRepository.RemoveItemFromWishlist(userId, productId);
        }

        public Dictionary<string, int> GetTotalFavoritesPerUser()
        {
            // MongoDB Aggregation Pipeline
            var pipeline = new[]
            {
        new BsonDocument
        {
            { "$unwind", "$Items" } // Her bir favoriyi ayrı bir belge olarak açar
        },
        new BsonDocument
        {
            { "$group", new BsonDocument
                {
                    { "_id", "$UserId" }, // Her kullanıcı için gruplama
                    { "TotalFavorites", new BsonDocument { { "$sum", 1 } } } // Toplam favori sayısını hesaplar
                }
            }
        }
    };

            // Pipeline çalıştır
            var result = _wishlistRepository.Aggregate(pipeline);

            // Sonuçları Dictionary'e dönüştür
            return result.ToDictionary(
                doc => doc["_id"].AsString,
                doc => doc["TotalFavorites"].AsInt32
            );
        }
    }

}

