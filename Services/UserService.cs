using ECommerceApp.Models;
using ECommerceApp.Data;
using MongoDB.Driver;

namespace ECommerceApp.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _users;

        public UserService(MongoDbContext dbContext)
        {
            _users = dbContext.GetCollection<User>("Users");
        }

        public List<User> GetAllUsers()
        {
            return _users.Find(_ => true).ToList();
        }

        public User GetUserById(string id)
        {
            return _users.Find(user => user.Id == id).FirstOrDefault();
        }

        public User GetUserByEmail(string email)
        {
            return _users.Find(user => user.Email == email).FirstOrDefault();
        }

        public void AddUser(User user)
        {
            _users.InsertOne(user);
        }

        public void UpdateUser(string id, User updatedUser)
        {
            _users.ReplaceOne(user => user.Id == id, updatedUser);
        }

        public void DeleteUser(string id)
        {
            _users.DeleteOne(user => user.Id == id);
        }

        public bool ValidatePassword(string email, string password)
        {
            var user = GetUserByEmail(email);
            if (user == null)
                return false;

            return BCrypt.Net.BCrypt.Verify(password, user.Password);
        }

        public void SetPassword(User user, string password)
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(password);
        }

        // Yeni Kullanıcı Oluştur
        public bool CreateUser(User user, string password)
        {
            if (GetUserByEmail(user.Email) != null)
            {
                return false; // Email zaten kayıtlı
            }

            SetPassword(user, password);
            AddUser(user);
            return true;
        }

        
    }
}
