using Microsoft.AspNetCore.Mvc;
using LabManagement.Data;
using LabManagement.Model;
using Microsoft.EntityFrameworkCore;

namespace LabManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DangKiDungCuController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DangKiDungCuController(ApplicationDbContext context)
        {
             _context = context;
        }

        // GET: api/DangKiDungCu
        [HttpGet]
        public async Task<IActionResult> GetChiTietDangKiDungCu()
        {
            var chiTietDangKiDungCus = await _context.DangKiDungCu
                .Include(c => c.PhieuDangKi) // Include PhieuDeXuat for detailed info
                .Include(c => c.DungCu)      // Include DungCu for detailed info
                .ToListAsync();

            return Ok(chiTietDangKiDungCus);
        }

        // POST: api/DangKiDungCu
        [HttpPost]
        public async Task<IActionResult> CreateDangKiDungCu([FromBody] DangKiDungCu newChiTiet)
        {
            try
            {
                if (newChiTiet == null)
                {
                    return BadRequest("Dang Ki Dung Cu cannot be null.");
                }

                // Validate that both MaPhieu and MaDungCu are provided
                if (string.IsNullOrEmpty(newChiTiet.MaPhieuDK) || string.IsNullOrEmpty(newChiTiet.MaDungCu))
                {
                    return BadRequest("Both MaPhieu and MaDungCu are required.");
                }

                // Ensure the PhieuDanGKi exists
                var phieuDangKi = await _context.PhieuDangKi.FindAsync(newChiTiet.MaPhieuDK);
                if (phieuDangKi == null)
                {
                    return NotFound("DangKiDungCu not found.");
                }

                // Ensure the DungCu exists
                var dungCu = await _context.DungCu.FindAsync(newChiTiet.MaDungCu);
                if (dungCu == null)
                {
                    return NotFound("DungCu not found.");
                }

                _context.DangKiDungCu.Add(newChiTiet);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetChiTietDangKiDungCu), new { maPhieu = newChiTiet.MaPhieuDK, maDungCu = newChiTiet.MaDungCu }, newChiTiet);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
        // GET: api/ChiTietDeXuatDungCu/{maPhieu}
        [HttpGet("{maPhieu}")]
        public async Task<IActionResult> GetDangKiDungCuByMaPhieu(string maPhieu)
        {
            // Retrieve the details of ChiTietDeXuatDungCu for the specific MaPhieu
            var chiTietDangKiDungCus = await _context.DangKiDungCu
                .Include(c => c.PhieuDangKi)  // Include PhieuDeXuat for detailed info
                .Include(c => c.DungCu)       // Include DungCu for detailed info
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
