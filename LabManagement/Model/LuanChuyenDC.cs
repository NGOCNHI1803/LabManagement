using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LabManagement.Model
{
    public class LuanChuyenDC
    {
        [Key, Column(Order = 0)]
        [MaxLength(20)]
        public string? MaPhieu { get; set; }

        [Key, Column(Order = 1)]
        [MaxLength(20)]
        public string? MaPhong { get; set; }

        [Key, Column(Order = 2)]
        [MaxLength(20)]
        public string? MaDungCu { get; set; }

        public int SoLuong { get; set; }

        public DateTime? NgayLuanChuyen { get; set; }

        public DateTime? NgayHoanTat { get; set; }

        [MaxLength(50)]
        public string? TrangThai { get; set; }

        [ForeignKey("MaPhieu")]
        [JsonIgnore]
        public PhieuPhanBoTB? PhieuPhanBoTB { get; set; } // Đối tượng nhân viên

        [ForeignKey("MaPhong")]
        [JsonIgnore]
        public PhongThiNghiem? PhongThiNghiem { get; set; } // Đối tượng nhân viên

        [ForeignKey("MaDungCu")]
        [JsonIgnore]
        public DungCu? DungCu{ get; set; } // Đối tượng nhân viên
    }
}
