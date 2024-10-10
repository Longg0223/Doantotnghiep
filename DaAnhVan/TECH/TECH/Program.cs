using Microsoft.EntityFrameworkCore;
using TECH.Data.DatabaseEntity;
using TECH.Reponsitory;
using TECH.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvc().AddJsonOptions(o =>
{
    o.JsonSerializerOptions.PropertyNamingPolicy = null;
    o.JsonSerializerOptions.DictionaryKeyPolicy = null;
});
builder.Services.AddControllers();
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();

builder.Services.AddDbContext<DataBaseEntityContext>(options =>
{
    // Đọc chuỗi kết nối
    string connectstring = builder.Configuration.GetConnectionString("AppDbContext");
    options.UseSqlServer(connectstring);
});
builder.Services.AddScoped(typeof(IUnitOfWork), typeof(EFUnitOfWork));
builder.Services.AddScoped(typeof(IRepository<,>), typeof(EFRepository<,>));

builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IContractsRepository, ContractsRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductsRepository, ProductsRepository>();
builder.Services.AddScoped<IPostsRepository, PostsRepository>();
builder.Services.AddScoped<IOrdersRepository, OrdersRepository>();
builder.Services.AddScoped<IReviewsRepository, ReviewsRepository>();
builder.Services.AddScoped<ICartsRepository, CartsRepository>();
builder.Services.AddScoped<IOrderDetailsRepository, OrderDetailsRepository>();
builder.Services.AddScoped<IImageRepository, ImageRepository>();
builder.Services.AddScoped<IProductsImageRepository, ProductsImageRepository>();
builder.Services.AddScoped<IProductQuantityRepository, ProductQuantityRepository>();

builder.Services.AddScoped<IVnPayService, VnPayService>();
builder.Services.AddScoped<IProductQuantityService, ProductQuantityService>();
builder.Services.AddScoped<IProductsImagesService, ProductsImagesService>();
builder.Services.AddScoped<IImagesService, ImagesService>();
builder.Services.AddScoped<IAppUserService, AppUserService>();
builder.Services.AddScoped<IContractsService, ContractsService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductsService, ProductsService>();
builder.Services.AddScoped<IPostsService, PostsService>();
builder.Services.AddScoped<IOrdersService, OrdersService>();
builder.Services.AddScoped<IReviewsService, ReviewsService>();
builder.Services.AddScoped<ICartsService, CartsService>();



//builder.Services.AddMemoryCache();

// Configure the HTTP request pipeline.
var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();


app.UseEndpoints(endpoints =>
{
    endpoints.MapAreaControllerRoute(
     name: "TrangQuanTri",
     areaName: "Admin",
     pattern: "admin",
     defaults: new { controller = "Home", action = "Index" });

    endpoints.MapAreaControllerRoute(
       name: "TaiKhoan",
       areaName: "Admin",
       pattern: "admin/quan-ly-tai-khoan",
       defaults: new { controller = "AppUsers", action = "Index" });

    endpoints.MapAreaControllerRoute(
      name: "UserDetail",
      areaName: "Admin",
      pattern: "admin/thong-tin-ca-nhan",
      defaults: new { controller = "AppUsers", action = "ViewDetail" });

    endpoints.MapAreaControllerRoute(
      name: "ChangePass",
      areaName: "Admin",
      pattern: "admin/doi-mat-khau",
      defaults: new { controller = "AppUsers", action = "ChangePassWord" });

    endpoints.MapAreaControllerRoute(
       name: "LienHe",
       areaName: "Admin",
       pattern: "admin/quan-ly-lien-he-tu-van",
       defaults: new { controller = "Contract", action = "Index" });

    endpoints.MapAreaControllerRoute(
       name: "DanhMuc",
       areaName: "Admin",
       pattern: "admin/quan-ly-danh-muc",
       defaults: new { controller = "Category", action = "Index" });

    endpoints.MapAreaControllerRoute(
       name: "ThemMoiSanPham",
       areaName: "Admin",
       pattern: "admin/tao-moi-san-pham",
       defaults: new { controller = "Product", action = "AddView" });

    endpoints.MapAreaControllerRoute(
      name: "DanhSachSanPham",
      areaName: "Admin",
      pattern: "admin/quan-ly-san-pham",
      defaults: new { controller = "Product", action = "Index" });

    endpoints.MapAreaControllerRoute(
     name: "ThemMoiBaiViet",
     areaName: "Admin",
     pattern: "admin/tao-moi-bai-viet",
     defaults: new { controller = "Post", action = "AddView" });

    endpoints.MapAreaControllerRoute(
      name: "DanhSachBaiViet",
      areaName: "Admin",
      pattern: "admin/quan-ly-bai-viet",
      defaults: new { controller = "Post", action = "Index" });

    endpoints.MapAreaControllerRoute(
      name: "DonHang",
      areaName: "Admin",
      pattern: "admin/quan-ly-don-hang",
      defaults: new { controller = "Orders", action = "Index" });

    endpoints.MapAreaControllerRoute(
      name: "DonHang",
      areaName: "Admin",
      pattern: "admin/quan-ly-danh-gia",
      defaults: new { controller = "Reviews", action = "Index" });

    endpoints.MapAreaControllerRoute(
      name: "DonHang",
      areaName: "Admin",
      pattern: "admin/quan-ly-phi-van-chuyen",
      defaults: new { controller = "Fee", action = "Index" });

    endpoints.MapAreaControllerRoute(
      name: "Advertisement",
      areaName: "Admin",
      pattern: "admin/quan-ly-quang-cao",
      defaults: new { controller = "Advertisement", action = "Index" });

    endpoints.MapAreaControllerRoute(
      name: "siders",
      areaName: "Admin",
      pattern: "admin/quan-ly-siders",
      defaults: new { controller = "Siders", action = "Index" });

    endpoints.MapAreaControllerRoute(
     name: "chitietdonhang",
     areaName: "Admin",
     pattern: "admin/chi-tiet-don-hang/{*orderId}",
     defaults: new { controller = "Orders", action = "OderDetail" });




    endpoints.MapAreaControllerRoute(
       name: "admin",
       areaName: "Admin",
       pattern: "admin/{controller=Home}/{action=Index}/{id?}");




    //// Topic List
    //routes.MapControllerRoute(
    //    name: "topic_listing",
    //    pattern: "bo-suu-tap-du-an",
    //    defaults: new { controller = "Topic", action = "List" });

    endpoints.MapControllerRoute(
       name: "SanPhamChiTiet",
       pattern: "/chi-tiet-san-pham/{*productId}",
       defaults: new { controller = "Product", action = "ProductDetail" });

    endpoints.MapControllerRoute(
      name: "GioHang",
      pattern: "/gio-hang",
      defaults: new { controller = "Carts", action = "Index" });


    endpoints.MapControllerRoute(
      name: "DanhSachSanPhamW",
      pattern: "/san-pham/{categoryId?}",
      defaults: new { controller = "Product", action = "ProductCategory" });

    endpoints.MapControllerRoute(
       name: "user",
       pattern: "/dang-ky",
       defaults: new { controller = "Users", action = "Register" });

    endpoints.MapControllerRoute(
       name: "userlogin",
       pattern: "/dang-nhap",
       defaults: new { controller = "Users", action = "Login" });
    endpoints.MapControllerRoute(
      name: "GioiThieu",
      pattern: "/gioi-thieu",
      defaults: new { controller = "Home", action = "About" });

    endpoints.MapControllerRoute(
      name: "CartPayment",
      pattern: "/thanh-toan",
      defaults: new { controller = "Carts", action = "OrderPay" });

    endpoints.MapControllerRoute(
     name: "BaiViet",
     pattern: "/bai-viet",
     defaults: new { controller = "Post", action = "Index" });

    endpoints.MapControllerRoute(
    name: "ChiTietBaiViet",
    pattern: "/chi-tiet-bai-viet/{*postId}",
    defaults: new { controller = "Post", action = "DetailPost" });

    endpoints.MapControllerRoute(
    name: "HoSoCaNhan",
    pattern: "/thong-tin-ca-nhan",
    defaults: new { controller = "Users", action = "Profile" });

    endpoints.MapControllerRoute(
    name: "LichSuMuaHang",
    pattern: "/lich-su-don-hang",
    defaults: new { controller = "Carts", action = "HistoryOrder" });

    endpoints.MapControllerRoute(
  name: "LienHeWe",
  pattern: "/lien-he-voi-chung-toi",
  defaults: new { controller = "Home", action = "Contact" });


    endpoints.MapControllerRoute(
    name: "DangXuat",
    pattern: "/dang-xuat",
    defaults: new { controller = "Users", action = "LogOut" });

    endpoints.MapControllerRoute(
  name: "TimKiemProduct",
  pattern: "/san-pham-tim-kiem/{*textSearch}",
  defaults: new { controller = "Product", action = "ProductSearch" });



    endpoints.MapControllerRoute(
 name: "TimKiemPost",
 pattern: "/bai-viet-tim-kiem/{*textSearch}",
 defaults: new { controller = "Product", action = "ProductSearch" });

    endpoints.MapControllerRoute(
   name: "DoiMatKhauWeb",
   pattern: "/doi-mat-khau",
   defaults: new { controller = "Users", action = "ChangePass" });

    endpoints.MapControllerRoute(
 name: "QuenMatKhau",
 pattern: "/quen-mat-khau",
 defaults: new { controller = "Users", action = "ForgotPassword" });

    endpoints.MapControllerRoute(
name: "XacThuc",
pattern: "/xac-thuc",
defaults: new { controller = "Users", action = "Accuracy" });

    endpoints.MapControllerRoute(
    name: "TaoMatKhauMoi",
    pattern: "/tao-mat-khau-moi",
    defaults: new { controller = "Users", action = "ChangeNewPassWord" });

//    endpoints.MapControllerRoute(
//name: "thanhtoanrepon",
//pattern: "/thanh-toan-responsive",
//defaults: new { controller = "Carts", action = "ChangeNewPassWord" });


    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");


});


//app.MapControllerRoute(

//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
