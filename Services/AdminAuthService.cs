using ECommerceApp.Models;

namespace ECommerceApp.Services
{
    public class AdminAuthService
    {
        private const string AdminUsername = "admin";
        private const string AdminPassword = "admin123";

        public bool ValidateUser(AdminLoginModel model)
        {
            return model.Username == AdminUsername && model.Password == AdminPassword;
        }
    }
}
