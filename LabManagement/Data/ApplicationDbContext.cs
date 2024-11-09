using LabManagement.Model;
using Microsoft.EntityFrameworkCore;

namespace LabManagement.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ChucVu> ChucVu { get; set; }
        public DbSet<NhomQuyen> NhomQuyen { get; set; }
        public DbSet<NhanVien> NhanVien { get; set; }
        public DbSet<LoaiDungCu> LoaiDungCu { get; set; }
        public DbSet<NhaCungCap> NhaCungCap { get; set; }

        public DbSet<DungCu> DungCu { get; set; }
        public DbSet<LoaiThietBi> LoaiThietBi { get; set; }
        public DbSet<ThietBi> ThietBi { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ChucVu>(entity =>
            {
                entity.HasKey(e => e.MaCV);
                entity.Property(e => e.TenCV).IsRequired().HasMaxLength(100);
            });

            modelBuilder.Entity<NhomQuyen>(entity =>
            {
                entity.HasKey(e => e.MaNhom);
                entity.Property(e => e.TenNhom).IsRequired().HasMaxLength(50);
            });

            modelBuilder.Entity<NhanVien>(entity =>
            {
                entity.HasKey(e => e.MaNV);

                // Configure foreign key for ChucVu
                entity.HasOne(e => e.ChucVu)
                      .WithMany()
                      .HasForeignKey(e => e.MaCV)
                      .OnDelete(DeleteBehavior.Restrict);

                // Configure foreign key for NhomQuyen
                entity.HasOne(e => e.NhomQuyen)
                      .WithMany()
                      .HasForeignKey(e => e.MaNhom)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.TenNV).IsRequired().HasMaxLength(100);
                entity.Property(e => e.GioiTinh).HasMaxLength(10);
                entity.Property(e => e.DiaChi).HasMaxLength(200);
                entity.Property(e => e.SoDT).HasMaxLength(15);

                // Cấu hình cho cột Email và Mật khẩu
                entity.Property(e => e.Email)
                      .HasMaxLength(100)
                      .IsRequired();

                entity.Property(e => e.MatKhau)
                      .HasMaxLength(100)
                      .IsRequired();
            });

            modelBuilder.Entity<LoaiDungCu>(entity =>
            {
                entity.HasKey(e => e.MaLoaiDC); // Thiết lập khóa chính
                entity.Property(e => e.TenLoaiDC)
                      .IsRequired()
                      .HasMaxLength(100); // Thiết lập yêu cầu và chiều dài tối đa cho TênLoaiDC
                entity.Property(e => e.MoTa)
                      .HasMaxLength(255); // Chiều dài tối đa cho MoTa
            });

            modelBuilder.Entity<NhaCungCap>(entity =>
            {
                entity.HasKey(e => e.MaNCC); // Thiết lập khóa chính
                entity.Property(e => e.TenNCC)
                      .IsRequired()
                      .HasMaxLength(100); 
                entity.Property(e => e.DiaChi)
                      .HasMaxLength(255); // Chiều dài tối đa cho MoTa
            });

            modelBuilder.Entity<DungCu>(entity =>
            {
                entity.HasKey(e => e.MaDungCu);

                // Configure foreign key for ChucVu
                entity.HasOne(e => e.LoaiDungCu)
                      .WithMany()
                      .HasForeignKey(e => e.MaLoaiDC)
                      .OnDelete(DeleteBehavior.Restrict);

                // Configure foreign key for NhomQuyen
                entity.HasOne(e => e.NhaCungCap)
                      .WithMany()
                      .HasForeignKey(e => e.MaNCC)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.TenDungCu)
                     .IsRequired()
                     .HasMaxLength(100);

                entity.Property(e => e.SoLuong)
                      .IsRequired();

                entity.Property(e => e.TinhTrang)
                      .HasMaxLength(50);

                entity.Property(e => e.ViTri)
                      .HasMaxLength(255);

                entity.Property(e => e.NgayCapNhat);
                entity.Property(e => e.NgaySX);
                entity.Property(e => e.NhaSX)
                      .HasMaxLength(100);
                entity.Property(e => e.NgayBaoHanh);
                entity.Property(e => e.XuatXu)
                      .HasMaxLength(100);
                entity.Property(e => e.HinhAnh)
                      .HasMaxLength(255);
            });

            modelBuilder.Entity<LoaiThietBi>(entity =>
            {
                entity.HasKey(e => e.MaLoaiThietBi);
                entity.Property(e => e.TenLoaiThietBi)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.Property(e => e.MoTa)
                      .HasMaxLength(255);
            });

            modelBuilder.Entity<ThietBi>(entity =>
            {
                entity.HasKey(e => e.MaThietBi);
                entity.Property(e => e.TenThietBi)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.Property(e => e.TinhTrang)
                      .HasMaxLength(50);
                entity.Property(e => e.NhaSX)
                      .HasMaxLength(100);
                entity.Property(e => e.XuatXu)
                      .HasMaxLength(100);
                entity.Property(e => e.HinhAnh)
                      .HasMaxLength(255);

                // Thiết lập khóa ngoại cho bảng LoaiThietBi
                entity.HasOne(e => e.LoaiThietBi)
                      .WithMany()
                      .HasForeignKey(e => e.MaLoaiThietBi)
                      .OnDelete(DeleteBehavior.Restrict);

                // Thiết lập khóa ngoại cho bảng NhaCungCap
                entity.HasOne(e => e.NhaCungCap)
                      .WithMany()
                      .HasForeignKey(e => e.MaNCC)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
