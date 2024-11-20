using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LabManagement.Model
{
    public class PhieuDangKi // đề xuất thiết bị mới
    {
        [Key]
        public string? MaPhieuDK { get; set; } // Mã phiếu đăng ký

        public string? MaThietBi { get; set; } // Mã thiết bị

        public string? MaNV { get; set; } // Mã nhân viên

        public DateTime? NgaySuDung { get; set; } // Ngày sử dụng

        public string? MaDungCu { get; set; } // Mã dụng cụ

        // Foreign key cho bảng ThietBi
        [ForeignKey("MaThietBi")]
        public ThietBi? ThietBi { get; set; } // Đối tượng thiết bị

        // Foreign key cho bảng NhanVien
        [ForeignKey("MaNV")]
        public NhanVien? NhanVien { get; set; } // Đối tượng nhân viên

        // Foreign key cho bảng DungCu
        [ForeignKey("MaDungCu")]
        public DungCu? DungCu { get; set; } // Đối tượng dụng cụ
    }
}
