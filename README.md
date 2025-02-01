# ASP.NET Core E-Commerce Platform | ReFashion

ReFashion is a sustainable and ethical e-commerce platform built with ASP.NET Core MVC and MongoDB.
This project aims to promote eco-friendly shopping by providing a marketplace for sustainable and recycled products.

## Features
- **Eco-Friendly & Recycled Products** – Encouraging responsible consumption.
- **User Authentication & Secure Transactions** – Ensuring safe and private shopping.
- **Product Catalog & Wishlist** – Browse, filter, and save your favorite products.
- **Shopping Cart & Checkout** – Seamless order processing with multiple payment options.
- **Sustainable Shipping & Order Tracking** – Transparent delivery updates.
- **MongoDB Integration** – Scalable NoSQL database for flexible data handling.

## Technology Stack
- **Frontend:** ASP.NET Core MVC, JavaScript, HTML, CSS
- **Backend:** ASP.NET Core
- **Database:** MongoDB
- **Authentication & Security:** JWT, Identity

## Installation & Setup
### Clone the Repository
```bash
git clone https://github.com/YOUR-USERNAME/ASP.NET-Core-E-Commerce-Platform-ReFashion.git  
cd ASP.NET-Core-E-Commerce-Platform-ReFashion  
```

### Restore Required Packages
```bash
dotnet restore  
```

### Configure Environment Settings
Since `appsettings.json` and `appsettings.Development.json` are not included in the repository for security reasons,
you need to create these files in the root of the project.
Use the following template as a reference:
```json
{
  "MongoDBSettings": {
    "ConnectionString": "mongodb+srv://username:password@cluster.mongodb.net/",
    "DatabaseName": "ECommerceDB"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Warning"
    }
  },
  "AllowedHosts": "*"
}

```
Replace `mongodb+srv://username:password@cluster.mongodb.net/` with your MongoDB connection string.

### Run the Project
```bash
dotnet run  
```
The application will start running at:
http://localhost:7122

## Contributing
We welcome contributions! If you'd like to contribute, please fork the repository and submit a pull request.

## Support the Project
If you like this project, please consider giving it a star on GitHub!

**"Shop Sustainably. Make a Difference."** 🌿
