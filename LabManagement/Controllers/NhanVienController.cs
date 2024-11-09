using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LabManagement.Data;
using LabManagement.Model;
using LabManagement.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using BCrypt.Net;

namespace LabManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NhanVienController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly JwtTokenService _jwtTokenService;

        public NhanVienController(ApplicationDbContext context, JwtTokenService jwtTokenService)
        {
            _context = context;
            _jwtTokenService = jwtTokenService;
        }

        // GET: api/NhanVien
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NhanVien>>> GetNhanViens()
        {
            return await _context.NhanVien
                .Include(nv => nv.ChucVu)       // Include ChucVu navigation property
                .Include(nv => nv.NhomQuyen)    // Include NhomQuyen navigation property
                .ToListAsync();
        }

        //[HttpPost]
        //public async Task<ActionResult<NhanVien>> PostNhanVien(NhanVien nhanVien)
        //{
        //    // Băm mật khẩu trước khi lưu
        //    nhanVien.MatKhau = BCrypt.Net.BCrypt.HashPassword(nhanVien.MatKhau);

        //    _context.NhanVien.Add(nhanVien);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction(nameof(GetNhanViens), new { id = nhanVien.MaNV }, nhanVien);
        //}

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var nhanVien = await _context.NhanVien
                .Include(nv => nv.NhomQuyen)
                .FirstOrDefaultAsync(nv => nv.Email == loginRequest.Email);

            if (nhanVien == null)
            {
                return Unauthorized(new { message = "Email hoặc mật khẩu không đúng" });
            }

            try
            {
                if (BCrypt.Net.BCrypt.Verify(loginRequest.MatKhau, nhanVien.MatKhau))
                {
                    var token = _jwtTokenService.GenerateJwtToken(nhanVien);
                    nhanVien.MatKhau = null;

                    return Ok(new
                    {
                        Token = token,
                        Role = nhanVien.NhomQuyen?.TenNhom
                    });
                }
                else
                {
                    return Unauthorized(new { message = "Email hoặc mật khẩu không đúng" });
                }
            }
            catch (BCrypt.Net.SaltParseException)
            {
                nhanVien.MatKhau = BCrypt.Net.BCrypt.HashPassword(loginRequest.MatKhau);
                _context.NhanVien.Update(nhanVien);
                await _context.SaveChangesAsync();

                return Unauthorized(new { message = "Salt không tương thích, đã cập nhật lại mật khẩu" });
            }
        }


        //[HttpPost("check-salt-version")]
        //public IActionResult CheckSaltVersion([FromBody] string password)
        //{
        //    // Giả sử lấy mật khẩu đã băm từ cơ sở dữ liệu hoặc đối tượng
        //    string hashedPassword = "$2a$12$DdY0gDgZyUrxwFhRYtPtjOI1F4nH1BsAGXxb98gOkV7tXXqMuGyOG"; // Ví dụ salt cũ

        //    try
        //    {
        //        // Kiểm tra mật khẩu với salt đã băm
        //        if (BCrypt.Net.BCrypt.Verify(password, hashedPassword))
        //        {
        //            return Ok("Mật khẩu hợp lệ và salt tương thích");
        //        }
        //        else
        //        {
        //            return Unauthorized(new { message = "Mật khẩu không hợp lệ hoặc salt không tương thích." });
        //        }
        //    }
        //    catch (System.ArgumentException ex)
        //    {
        //        return Unauthorized(new { message = $"Lỗi salt: {ex.Message}" });
        //    }
        //}



        // Secure endpoints based on roles
        [Authorize(Policy = "GiamDocTrungTam")] // Giám đốc trung tâm
        [HttpGet("giam-doc-trung-tam")]
        public IActionResult ForQuanLyPTN()
        {
            return Ok("Giám đốc trung tâm");
        }

        [Authorize(Policy = "ChuyenVien")] // Chuyên viên phòng thí nghiệm
        [HttpGet("chuyen-vien")]
        public IActionResult ForChuyenVien()
        {
            return Ok("Chuyên viên phòng thí nghiệm");
        }

        [Authorize(Policy = "NguoiDung")] // Người dùng (Học sinh/Giáo viên)
        [HttpGet("nguoi-dung")]
        public IActionResult ForNguoiDung()
        {
            return Ok("Người dùng - Học sinh/Giáo viên");
        }

       


    }
}
