using E_commerceTechnologyWebsite.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace E_commerceTechnologyWebsite.KhoLuuTru
{
    public class DataContext : IdentityDbContext<AppUserModel>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<ThuongHieuModel> ThuongHieu { get; set; }
        public DbSet<SanPhamModel> SanPham { get; set; }
        public DbSet<DanhGiaModel> DanhGia { get; set; }
        public DbSet<TheLoaiModel> TheLoai { get; set; }
        public DbSet<OrderModel> Orders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<ContactModel> Contact { get; set; }
        public DbSet<SoLuongSanPhamModel> SoLuongSanPham { get; set; }
        public DbSet<ShippingModel> Shipping { get; set; }
        public DbSet<CouponModel> Coupon { get; set; }

        public DbSet<UserActivity> UserActivities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SanPhamModel>()
                .Property(p => p.IsActive)
                .HasDefaultValue(true);

            // Cấu hình mối quan hệ giữa SanPhamModel và DanhGiaModel
            modelBuilder.Entity<SanPhamModel>()
                .HasMany(s => s.DanhGia)
                .WithOne(d => d.Product)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // Cấu hình kiểu dữ liệu cho ProductId trong DanhGiaModel
            modelBuilder.Entity<DanhGiaModel>()
                .Property(d => d.ProductId)
                .HasColumnType("bigint");
        }
    }
}