﻿using System.ComponentModel.DataAnnotations.Schema;

namespace LabManagement.Model
{
    public class NhanVien
    {
        public string? MaNV { get; set; }
        public string? TenNV { get; set; }
        public string? GioiTinh { get; set; }
        public DateTime? NgaySinh { get; set; }
        public string? DiaChi { get; set; }
        public string? SoDT { get; set; }

        // Foreign key for ChucVu
        public string? MaCV { get; set; }
        [ForeignKey("MaCV")]
        public ChucVu? ChucVu { get; set; }

        // Foreign key for NhomQuyen
        public string? MaNhom { get; set; }
        [ForeignKey("MaNhom")]
        public NhomQuyen? NhomQuyen { get; set; }
    }

}
