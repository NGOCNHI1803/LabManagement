using Microsoft.AspNetCore.Mvc;
using LabManagement.Data;
using LabManagement.Model;
using Microsoft.EntityFrameworkCore;

namespace LabManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhieuDangKiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PhieuDangKiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetPhieuDKi()
        {
            var phieuDeXuats = await _context.PhieuDangKi
                .Include(p => p.NhanVien)
                .Include(p => p.PhongThiNghiem)
                .ToListAsync();

            return Ok(phieuDeXuats);
        }
        // POST: api/PhieuDKi
        [HttpPost]
        public async Task<IActionResult> CreatePhieuDKi([FromBody] PhieuDangKi newPhieuDKi)
        {


            try
            {
                if (newPhieuDKi == null)
                {
                    return BadRequest("Phieu Dang Ki cannot be null.");
                }

                if (string.IsNullOrEmpty(newPhieuDKi.MaNV))
                {
                    return BadRequest("Both MaThietBi and MaNV are required.");
                }

                _context.PhieuDangKi.Add(newPhieuDKi);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetPhieuDKi), new { id = newPhieuDKi.MaPhieuDK }, newPhieuDKi);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
        [HttpGet("existing")]
        public async Task<IActionResult> GetExistingPhieuDKi()
        {
            var phieuDKis = await _context.PhieuDangKi.Select(p => new { p.MaPhieuDK }).ToListAsync();
            return Ok(phieuDKis);
        }
        // GET: api/PhieuDKi/{maPhieu}
        [HttpGet("{maPhieu}")]
        public async Task<IActionResult> GetPhieuDKiByMaPhieu(string maPhieu)
        {
            // Find the proposal with the given MaPhieu
            var phieuDKi = await _context.PhieuDangKi
                .Include(p => p.NhanVien) // Include NhanVien details
                .Include(p => p.PhongThiNghiem)
                .FirstOrDefaultAsync(p => p.MaPhieuDK == maPhieu); // Search by MaPhieu

            // If the proposal is not found, return a 404 Not Found
            if (phieuDKi == null)
            {
                return NotFound($"PhieuDangKi with MaPhieu '{maPhieu}' not found.");
            }

            // Return the found proposal details
            return Ok(phieuDKi);
        }
    }
}
