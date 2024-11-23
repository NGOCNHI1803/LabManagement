using Microsoft.AspNetCore.Mvc;
using LabManagement.Data;
using LabManagement.Model;
using Microsoft.EntityFrameworkCore;

namespace LabManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DangKyThietBiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DangKyThietBiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/DangKiDungCu
        [HttpGet]
        public async Task<IActionResult> GetChiTietDangKiThietBi()
        {
            var chiTietDangKiDungCus = await _context.DangKiThietBi
                .Include(c => c.PhieuDangKi) // Include PhieuDeXuat for detailed info
                .Include(c => c.ThietBi)      // Include DungCu for detailed info
                .ToListAsync();

            return Ok(chiTietDangKiDungCus);
        }

        // POST: api/DangKiDungCu
        [HttpPost]
        public async Task<IActionResult> CreateDangKiThietBi([FromBody] DangKiThietBi newChiTiet)
        {
            try
            {
                if (newChiTiet == null)
                {
                    return BadRequest("Dang Ki Thiet Bi cannot be null.");
                }

                // Validate that both MaPhieu and MaDungCu are provided
                if (string.IsNullOrEmpty(newChiTiet.MaPhieuDK) || string.IsNullOrEmpty(newChiTiet.MaThietBi))
                {
                    return BadRequest("Both MaPhieu and MaDungCu are required.");
                }

                // Ensure the PhieuDanGKi exists
                var phieuDangKi = await _context.PhieuDangKi.FindAsync(newChiTiet.MaPhieuDK);
                if (phieuDangKi == null)
                {
                    return NotFound("DangKiThietBi not found.");
                }

                // Ensure the DungCu exists
                var dungCu = await _context.ThietBi.FindAsync(newChiTiet.MaThietBi);
                if (dungCu == null)
                {
                    return NotFound("ThietBi not found.");
                }

                _context.DangKiThietBi.Add(newChiTiet);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetChiTietDangKiThietBi), new { maPhieu = newChiTiet.MaPhieuDK, maDungCu = newChiTiet.MaThietBi }, newChiTiet);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
        // GET: api/ChiTietDeXuatDungCu/{maPhieu}
        [HttpGet("{maPhieu}")]
        public async Task<IActionResult> GetDangKiThietBiByMaPhieu(string maPhieu)
        {
            // Retrieve the details of ChiTietDeXuatDungCu for the specific MaPhieu
            var chiTietDangKiDungCus = await _context.DangKiThietBi
                .Include(c => c.PhieuDangKi)  // Include PhieuDeXuat for detailed info
                .Include(c => c.ThietBi)       // Include DungCu for detailed info
                .Where(c => c.MaPhieuDK == maPhieu) // Filter by MaPhieu
                .ToListAsync();

            if (chiTietDangKiDungCus == null || chiTietDangKiDungCus.Count == 0)
            {
                return NotFound("No details found for the given MaPhieu.");
            }

            return Ok(chiTietDangKiDungCus);
        }
    }
}
