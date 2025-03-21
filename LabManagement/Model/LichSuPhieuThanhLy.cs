﻿using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LabManagement.Model
{
    public class LichSuPhieuThanhLy
    {
        public int MaLichSu { get; set; }          // Mã lịch sử (ID chính)
        public string? MaPhieuTL { get; set; }     // Mã phiếu thanh lý
        public string? TrangThaiTruoc { get; set; } // Trạng thái trước khi thay đổi
        public string? TrangThaiSau { get; set; }   // Trạng thái sau khi thay đổi
        public DateTime NgayThayDoi { get; set; }  // Thời gian thay đổi
        public string? MaNV { get; set; }           // Mã nhân viên thực hiện thay đổi
        [ForeignKey("MaPhieuTL")]
        [JsonIgnore]
        public virtual PhieuThanhLy? PhieuThanhLy { get; set; }

        [ForeignKey("MaNV")]
        [JsonIgnore]
        public virtual NhanVien? NhanVien { get; set; }
    }
}
