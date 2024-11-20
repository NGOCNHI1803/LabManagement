using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LabManagement.Model
{
    public class ChiTietDeXuatDungCu
    {
        [Key, Column(Order = 0)]
        public string MaPhieu { get; set; } // Mã phiếu

        [Key, Column(Order = 1)]
        public string MaDungCu { get; set; } // Mã dụng cụ

        public int SoLuongDeXuat { get; set; } // Số lượng đề xuất

        // Quan hệ với bảng PhieuDeXuat
        [ForeignKey("MaPhieu")]
        [JsonIgnore]
        public PhieuDeXuat? PhieuDeXuat { get; set; }

        // Quan hệ với bảng DungCu
        [ForeignKey("MaDungCu")]
        [JsonIgnore]
        public DungCu? DungCu { get; set; }
    }
}
