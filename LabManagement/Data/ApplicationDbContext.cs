﻿using LabManagement.Model;
using Microsoft.EntityFrameworkCore;

namespace LabManagement.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Các DbSet hiện tại
        public DbSet<ChucVu> ChucVu { get; set; }
        public DbSet<NhomQuyen> NhomQuyen { get; set; }
        public DbSet<NhanVien> NhanVien { get; set; }
        public DbSet<LoaiDungCu> LoaiDungCu { get; set; }
        public DbSet<NhaCungCap> NhaCungCap { get; set; }
        public DbSet<DungCu> DungCu { get; set; }
        public DbSet<LoaiThietBi> LoaiThietBi { get; set; }
        public DbSet<ThietBi> ThietBi { get; set; }

        // Thêm các DbSet mới
        public DbSet<PhieuDeXuat> PhieuDeXuat { get; set; }
        public DbSet<DuyetPhieu> DuyetPhieu { get; set; }
        public DbSet<ChiTietDeXuatDungCu> ChiTietDeXuatDungCu { get; set; }

        public DbSet<PhieuDangKi> PhieuDangKi { get; set; }
        public DbSet<DuyetPhieuDK> DuyetPhieuDK { get; set; }
        public DbSet<DangKiDungCu> DangKiDungCu { get; set; }


        public DbSet<DangKiThietBi> DangKiThietBi { get; set; }

        public DbSet<ThoiGianSuDung> ThoiGianSuDung { get; set; }

        public DbSet<PhongThiNghiem> PhongThiNghiem { get; set; }
        public DbSet<PhieuPhanBoTB> PhieuPhanBoTB { get; set; }
        public DbSet<PhanBoDC> PhanBoDC { get; set; }
        public DbSet<LuanChuyenDC> LuanChuyenDC { get; set; }
        public DbSet<LuanChuyenTB> LuanChuyenTB { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cấu hình cho các bảng hiện tại
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

                entity.HasOne(e => e.ChucVu)
                      .WithMany()
                      .HasForeignKey(e => e.MaCV)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.NhomQuyen)
                      .WithMany()
                      .HasForeignKey(e => e.MaNhom)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.TenNV).IsRequired().HasMaxLength(100);
                entity.Property(e => e.GioiTinh).HasMaxLength(10);
                entity.Property(e => e.DiaChi).HasMaxLength(200);
                entity.Property(e => e.SoDT).HasMaxLength(15);
                entity.Property(e => e.Email).HasMaxLength(100).IsRequired();
                entity.Property(e => e.MatKhau).HasMaxLength(100).IsRequired();
            });

            modelBuilder.Entity<LoaiDungCu>(entity =>
            {
                entity.HasKey(e => e.MaLoaiDC);
                entity.Property(e => e.TenLoaiDC)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.Property(e => e.MoTa)
                      .HasMaxLength(255);
            });

            modelBuilder.Entity<NhaCungCap>(entity =>
            {
                entity.HasKey(e => e.MaNCC);
                entity.Property(e => e.TenNCC)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.Property(e => e.DiaChi)
                      .HasMaxLength(255);
            });

            modelBuilder.Entity<DungCu>(entity =>
            {
                entity.HasKey(e => e.MaDungCu);

                entity.HasOne(e => e.LoaiDungCu)
                      .WithMany()
                      .HasForeignKey(e => e.MaLoaiDC)
                      .OnDelete(DeleteBehavior.Restrict);

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

                entity.HasOne(e => e.LoaiThietBi)
                      .WithMany()
                      .HasForeignKey(e => e.MaLoaiThietBi)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.NhaCungCap)
                      .WithMany()
                      .HasForeignKey(e => e.MaNCC)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<PhieuDeXuat>(entity =>
            {
                entity.HasKey(e => e.MaPhieu);
                entity.Property(e => e.MaPhieu).HasMaxLength(20).IsRequired();
                entity.Property(e => e.MaThietBi).HasMaxLength(20).IsRequired();
                entity.Property(e => e.LyDoDeXuat).HasMaxLength(100).IsRequired();
                entity.Property(e => e.MaNV).HasMaxLength(20).IsRequired();
                entity.Property(e => e.GhiChu).HasMaxLength(255);
                entity.Property(e => e.TrangThai).HasMaxLength(50);
                entity.Property(e => e.NgayTao).IsRequired(false);
                entity.Property(e => e.NgayHoanTat).IsRequired(false);

                entity.HasOne(e => e.ThietBi)
                    .WithMany()
                    .HasForeignKey(e => e.MaThietBi)
                    .OnDelete(DeleteBehavior.Restrict);


                entity.HasOne(e => e.NhanVien)
                      .WithMany() 
                      .HasForeignKey(e => e.MaNV)
                      .OnDelete(DeleteBehavior.Restrict);
            });


            // Cấu hình cho DuyetPhieuDeXuat
            modelBuilder.Entity<DuyetPhieu>(entity =>
            {
                entity.HasKey(e => e.MaPhieu);
                entity.Property(e => e.MaPhieu).HasMaxLength(20).IsRequired();
                entity.Property(e => e.MaNV).HasMaxLength(20).IsRequired();
                entity.Property(e => e.NgayDuyet).IsRequired(false);
                entity.Property(e => e.TrangThai).HasMaxLength(100).IsRequired();
                entity.Property(e => e.LyDoTuChoi).HasMaxLength(255).IsRequired();

                entity.HasOne(e => e.PhieuDeXuat)
                      .WithMany()
                      .HasForeignKey(e => e.MaPhieu)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.NhanVien)
                      .WithMany()
                      .HasForeignKey(e => e.MaNV)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Cấu hình cho ChiTietDeXuatDungCu
            modelBuilder.Entity<ChiTietDeXuatDungCu>(entity =>
            {
                entity.HasKey(e => new { e.MaPhieu, e.MaDungCu });
                entity.Property(e => e.SoLuongDeXuat).IsRequired();

                entity.HasOne(e => e.PhieuDeXuat)
                      .WithMany()
                      .HasForeignKey(e => e.MaPhieu)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.DungCu)
                      .WithMany()
                      .HasForeignKey(e => e.MaDungCu)
                      .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<PhieuDangKi>(entity => { 
                entity.HasKey(e => e.MaPhieuDK);
                entity.Property(e => e.MaPhieuDK).HasMaxLength(20).IsRequired();
                entity.Property(e => e.LyDoDK).HasMaxLength(100).IsRequired();
                entity.Property(e => e.MaNV).HasMaxLength(20).IsRequired();
                entity.Property(e => e.GhiChu).HasMaxLength(255);
                entity.Property(e => e.TrangThai).HasMaxLength(50);
                entity.Property(e => e.NgayLap).IsRequired(false);
                entity.Property(e => e.NgayHoanTat).IsRequired(false);

                entity.HasOne(e => e.NhanVien)
                      .WithMany()
                      .HasForeignKey(e => e.MaNV)
                      .OnDelete(DeleteBehavior.Restrict);

            });
            // Cấu hình cho DuyetPhieuDK
            modelBuilder.Entity<DuyetPhieuDK>(entity =>
            {
                entity.HasKey(e => e.MaPhieuDK);
                entity.Property(e => e.MaPhieuDK).HasMaxLength(20).IsRequired();
                entity.Property(e => e.MaNV).HasMaxLength(20).IsRequired();
                entity.Property(e => e.NgayDuyet).IsRequired(false);
                entity.Property(e => e.TrangThai).HasMaxLength(100).IsRequired();
                entity.Property(e => e.LyDoTuChoi).HasMaxLength(255).IsRequired();

                entity.HasOne(e => e.PhieuDangKi)
                      .WithMany()
                      .HasForeignKey(e => e.MaPhieuDK)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.NhanVien)
                      .WithMany()
                      .HasForeignKey(e => e.MaNV)
                      .OnDelete(DeleteBehavior.Restrict);
            });
            // Cấu hình cho DKDungCu
            modelBuilder.Entity<DangKiDungCu>(entity =>
            {
                entity.HasKey(e => new{ e.MaPhieuDK, e.MaDungCu });
                entity.Property(e => e.SoLuong).IsRequired();
                entity.Property(e => e.NgayDangKi).IsRequired();
                entity.Property(e => e.NgayKetThuc).IsRequired();

                entity.HasOne(e => e.PhieuDangKi)
                      .WithMany()
                      .HasForeignKey(e => e.MaPhieuDK)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.DungCu)
                      .WithMany()
                      .HasForeignKey(e => e.MaDungCu)
                      .OnDelete(DeleteBehavior.Restrict);
            });
            // Cấu hình cho DKDungCu
            modelBuilder.Entity<DangKiThietBi>(entity =>
            {
                entity.HasKey(e => new { e.MaPhieuDK, e.MaThietBi });
                entity.Property(e => e.NgayDangKi).IsRequired();
                entity.Property(e => e.NgayKetThuc).IsRequired();

                entity.HasOne(e => e.PhieuDangKi)
                      .WithMany()
                      .HasForeignKey(e => e.MaPhieuDK)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.ThietBi)
                      .WithMany()
                      .HasForeignKey(e => e.MaThietBi)
                      .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<ThoiGianSuDung>(entity =>
            {
                entity.HasKey(e => new { e.MaPhieuDK, e.MaThietBi });
                entity.Property(e => e.MaNV).HasMaxLength(20).IsRequired();
                entity.Property(e => e.NgayBatDau).IsRequired();
                entity.Property(e => e.NgayKetThuc).IsRequired();
                entity.Property(e => e.SoGio).IsRequired();

                entity.HasOne(e => e.PhieuDangKi)
                      .WithMany()
                      .HasForeignKey(e => e.MaPhieuDK)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.ThietBi)
                      .WithMany()
                      .HasForeignKey(e => e.MaThietBi)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.NhanVien)
                      .WithMany()
                      .HasForeignKey(e => e.MaNV)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<PhongThiNghiem>(entity =>
            {
                entity.HasKey(e => e.MaPhong);
                entity.Property(e => e.MaPhong).HasMaxLength(20).IsRequired();
                entity.Property(e => e.LoaiPhong).HasMaxLength(50);
                entity.Property(e => e.ChucNang).HasMaxLength(50);
            });

            modelBuilder.Entity<PhieuPhanBoTB>(entity =>
            {
                entity.HasKey(e => e.MaPhieu);
                entity.Property(e => e.MaPhieu).HasMaxLength(20).IsRequired();
                entity.Property(e => e.MaThietBi).HasMaxLength(20).IsRequired();
                entity.Property(e => e.MaNV).HasMaxLength(20).IsRequired();
                entity.Property(e => e.NoiDung).HasMaxLength(255);
                entity.Property(e => e.NgayLap);
                entity.Property(e => e.TrangThai).HasMaxLength(50);

                entity.HasOne(e => e.NhanVien)
                      .WithMany()
                      .HasForeignKey(e => e.MaNV)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.ThietBi)
                      .WithMany()
                      .HasForeignKey(e => e.MaThietBi)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<PhanBoDC>(entity =>
            {
                entity.HasKey(e => new { e.MaPhieu, e.MaDungCu });
                entity.Property(e => e.SoLuong).IsRequired();
                entity.Property(e => e.NgayLap);
                entity.Property(e => e.TrangThai).HasMaxLength(50);

                entity.HasOne(e => e.PhieuPhanBoTB)
                      .WithMany()
                      .HasForeignKey(e => e.MaPhieu)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.DungCu)
                      .WithMany()
                      .HasForeignKey(e => e.MaDungCu)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Cấu hình cho bảng LuanChuyenDC
            modelBuilder.Entity<LuanChuyenDC>(entity =>
            {
                entity.HasKey(e => new { e.MaPhieu, e.MaPhong, e.MaDungCu });

                entity.Property(e => e.SoLuong)
                      .IsRequired();

                entity.Property(e => e.NgayLuanChuyen)
                      .IsRequired(false);

                entity.Property(e => e.NgayHoanTat)
                      .IsRequired(false);

                entity.Property(e => e.TrangThai)
                      .HasMaxLength(50)
                      .IsRequired(false);

                // Cấu hình quan hệ với PhieuPhanBoTB
                entity.HasOne(e => e.PhieuPhanBoTB)
                      .WithMany()
                      .HasForeignKey(e => e.MaPhieu)
                      .OnDelete(DeleteBehavior.Restrict);

                // Cấu hình quan hệ với PhongThiNghiem
                entity.HasOne(e => e.PhongThiNghiem)
                      .WithMany()
                      .HasForeignKey(e => e.MaPhong)
                      .OnDelete(DeleteBehavior.Restrict);

                // Cấu hình quan hệ với DungCu
                entity.HasOne(e => e.DungCu)
                      .WithMany()
                      .HasForeignKey(e => e.MaDungCu)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Cấu hình cho bảng LuanChuyenTB
            modelBuilder.Entity<LuanChuyenTB>(entity =>
            {
                entity.HasKey(e => new { e.MaPhieu, e.MaPhong, e.MaThietBi });

                entity.Property(e => e.NgayLuanChuyen)
                      .IsRequired(false);

                entity.Property(e => e.NgayHoanTat)
                      .IsRequired(false);

                entity.Property(e => e.TrangThai)
                      .HasMaxLength(50)
                      .IsRequired(false);

                // Cấu hình quan hệ với PhieuPhanBoTB
                entity.HasOne(e => e.PhieuPhanBoTB)
                      .WithMany()
                      .HasForeignKey(e => e.MaPhieu)
                      .OnDelete(DeleteBehavior.Restrict);

                // Cấu hình quan hệ với PhongThiNghiem
                entity.HasOne(e => e.PhongThiNghiem)
                      .WithMany()
                      .HasForeignKey(e => e.MaPhong)
                      .OnDelete(DeleteBehavior.Restrict);

                // Cấu hình quan hệ với ThietBi
                entity.HasOne(e => e.ThietBi)
                      .WithMany()
                      .HasForeignKey(e => e.MaThietBi)
                      .OnDelete(DeleteBehavior.Restrict);
            });

        }
    }
}
