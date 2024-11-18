using System.ComponentModel.DataAnnotations;

namespace LabManagement.Model
{
    public class DuyetPhieuDK
    {
        [Key]
        public string MaPhieu { get; set; }
        public string MaNV { get; set; }
        public DateTime? NgayDuyet { get; set; }
        public string TrangThai { get; set; }
    }
}
