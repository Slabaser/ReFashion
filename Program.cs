using ECommerceApp.Data;
using ECommerceApp.Models;
using ECommerceApp.Repositories;
using ECommerceApp.Services;
using Microsoft.Extensions.Options;
using ECommerceApp.Filters;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// MongoDB Ayarlarını Yapılandırma
builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection("MongoDBSettings"));

// Servis ve Bağımlılıkları Kaydetme
builder.Services.AddSingleton<MongoDbContext>(); // MongoDB bağlamı
builder.Services.AddTransient<UserService>(); // Kullanıcı servisleri
builder.Services.AddTransient<ProductService>(); // Ürün servisleri
builder.Services.AddTransient<ProductRepository>(); // Ürün repository
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<WishlistService>();
builder.Services.AddScoped<WishlistRepository>();
builder.Services.AddScoped<CartService>();
builder.Services.AddScoped<CartRepository>();
builder.Services.AddScoped<AddressRepository>(); // Adres repository
builder.Services.AddScoped<ShippingRepository>(); // Shipping repository
builder.Services.AddScoped<PaymentService>(); // Payment servisleri
builder.Services.AddScoped<PaymentRepository>(); // Payment repository
builder.Services.AddScoped<OrderService>(); // Order servisleri
builder.Services.AddScoped<OrderRepository>(); // Order repository
builder.Services.AddScoped<CheckoutService>();
builder.Services.AddScoped<ProductRepository>();
builder.Services.AddScoped<ReviewService>();
builder.Services.AddScoped<ReviewRepository>();
builder.Services.AddScoped<ShippingService>();
builder.Services.AddScoped<AddressService>();
builder.Services.AddScoped<AdminAuthService>();




// Authentication ve Authorization Ayarları
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.LoginPath = "/Account/Login"; // Login sayfası
        options.AccessDeniedPath = "/Account/AccessDenied"; // Yetkisiz erişim sayfası
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Çerez süresi
        options.SlidingExpiration = true; // Oturum süresini yenile
    });

builder.Services.AddAuthorization(); // Yetkilendirme ekle

// MVC ve Controller Ayarları
builder.Services.AddControllersWithViews();

// Oturum Verilerini Bellekte Tutma
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Oturum süresi
    options.Cookie.HttpOnly = true; // Güvenlik
    options.Cookie.IsEssential = true; // Zorunlu çerez
});

// Özel Filtreler
builder.Services.AddScoped<AuthorizeFilter>();

var app = builder.Build();

// Middleware Sıralaması
app.UseStaticFiles(); // CSS, JS gibi statik dosyalar için
app.UseRouting();
app.UseSession(); // Oturum desteği
app.UseAuthentication(); // Authentication middleware
app.UseAuthorization(); // Authorization middleware

// Route Tanımlamaları
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "admin",
        pattern: "{controller=Admin}/{action=Login}/{id?}");
});

// Uygulama Başlatma
app.Run();
