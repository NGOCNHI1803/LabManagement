using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LabManagement.Data;
using LabManagement.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.Data;

namespace LabManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NhanVienController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public NhanVienController(ApplicationDbContext context)
        {
            _context = context;
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

        // GET: api/NhanVien/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NhanVien>> GetNhanVien(string id)
        {
            var nhanVien = await _context.NhanVien
                .Include(nv => nv.ChucVu)       // Include ChucVu navigation property
                .Include(nv => nv.NhomQuyen)    // Include NhomQuyen navigation property
                .FirstOrDefaultAsync(nv => nv.MaNV == id);

            if (nhanVien == null)
            {
                return NotFound();
            }

            return nhanVien;
        }

        // POST: api/NhanVien
        [HttpPost]
        public async Task<ActionResult<NhanVien>> PostNhanVien(NhanVien nhanVien)
        {
            _context.NhanVien.Add(nhanVien);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetNhanVien), new { id = nhanVien.MaNV }, nhanVien);
        }

        // PUT: api/NhanVien/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNhanVien(string id, NhanVien nhanVien)
        {
            if (id != nhanVien.MaNV)
            {
                return BadRequest();
            }

            _context.Entry(nhanVien).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NhanVienExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/NhanVien/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNhanVien(string id)
        {
            var nhanVien = await _context.NhanVien.FindAsync(id);
            if (nhanVien == null)
            {
                return NotFound();
            }

            _context.NhanVien.Remove(nhanVien);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool NhanVienExists(string id)
        {
            return _context.NhanVien.Any(e => e.MaNV == id);
        }
        [HttpPost("login")]
        public async Task<ActionResult<NhanVien>> Login([FromBody] NhanVien loginRequest)
        {
            // Tìm nhân viên theo email và mật khẩu
            var nhanVien = await _context.NhanVien
                .FirstOrDefaultAsync(nv => nv.Email == loginRequest.Email && nv.MatKhau == loginRequest.MatKhau);

            // Nếu không tìm thấy, trả về lỗi
            if (nhanVien == null)
            {
                return Unauthorized(new { message = "Email hoặc mật khẩu không đúng" });
            }

            // Nếu tìm thấy, trả về thông tin nhân viên (ẩn mật khẩu trước khi trả về)
            nhanVien.MatKhau = null;
            return Ok(nhanVien);
        }

    }

}
