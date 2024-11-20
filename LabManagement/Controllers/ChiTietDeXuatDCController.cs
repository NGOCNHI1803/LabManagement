using Microsoft.AspNetCore.Mvc;
using LabManagement.Data;
using LabManagement.Model;
using Microsoft.EntityFrameworkCore;

namespace LabManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChiTietDeXuatDungCuController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ChiTietDeXuatDungCuController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ChiTietDeXuatDungCu
        [HttpGet]
        public async Task<IActionResult> GetChiTietDeXuatDungCu()
        {
            var chiTietDeXuatDungCus = await _context.ChiTietDeXuatDungCu
                .Include(c => c.PhieuDeXuat) // Include PhieuDeXuat for detailed info
                .Include(c => c.DungCu)      // Include DungCu for detailed info
                .ToListAsync();

            return Ok(chiTietDeXuatDungCus);
        }

        // POST: api/ChiTietDeXuatDungCu
        [HttpPost]
        public async Task<IActionResult> CreateChiTietDeXuatDungCu([FromBody] ChiTietDeXuatDungCu newChiTiet)
        {
            try
            {
                if (newChiTiet == null)
                {
                    return BadRequest("ChiTietDeXuatDungCu cannot be null.");
                }

                // Validate that both MaPhieu and MaDungCu are provided
                if (string.IsNullOrEmpty(newChiTiet.MaPhieu) || string.IsNullOrEmpty(newChiTiet.MaDungCu))
                {
                    return BadRequest("Both MaPhieu and MaDungCu are required.");
                }

                // Ensure the PhieuDeXuat exists
                var phieuDeXuat = await _context.PhieuDeXuat.FindAsync(newChiTiet.MaPhieu);
                if (phieuDeXuat == null)
                {
                    return NotFound("PhieuDeXuat not found.");
                }

                // Ensure the DungCu exists
                var dungCu = await _context.DungCu.FindAsync(newChiTiet.MaDungCu);
                if (dungCu == null)
                {
                    return NotFound("DungCu not found.");
                }

                _context.ChiTietDeXuatDungCu.Add(newChiTiet);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetChiTietDeXuatDungCu), new { maPhieu = newChiTiet.MaPhieu, maDungCu = newChiTiet.MaDungCu }, newChiTiet);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        // GET: api/ChiTietDeXuatDungCu/{maPhieu}/{maDungCu}
        [HttpGet("{maPhieu}/{maDungCu}")]
        public async Task<IActionResult> GetChiTietDeXuatDungCuById(string maPhieu, string maDungCu)
        {
            var chiTiet = await _context.ChiTietDeXuatDungCu
                .Include(c => c.PhieuDeXuat)
                .Include(c => c.DungCu)
                .FirstOrDefaultAsync(c => c.MaPhieu == maPhieu && c.MaDungCu == maDungCu);

            if (chiTiet == null)
            {
                return NotFound("ChiTietDeXuatDungCu not found.");
            }

            return Ok(chiTiet);
        }

        // PUT: api/ChiTietDeXuatDungCu/{maPhieu}/{maDungCu}
        [HttpPut("{maPhieu}/{maDungCu}")]
        public async Task<IActionResult> UpdateChiTietDeXuatDungCu(string maPhieu, string maDungCu, [FromBody] ChiTietDeXuatDungCu updatedChiTiet)
        {
            if (updatedChiTiet == null)
            {
                return BadRequest("ChiTietDeXuatDungCu cannot be null.");
            }

            var chiTiet = await _context.ChiTietDeXuatDungCu
                .FirstOrDefaultAsync(c => c.MaPhieu == maPhieu && c.MaDungCu == maDungCu);

            if (chiTiet == null)
            {
                return NotFound("ChiTietDeXuatDungCu not found.");
            }

            // Update fields
            chiTiet.SoLuongDeXuat = updatedChiTiet.SoLuongDeXuat;

            try
            {
                _context.ChiTietDeXuatDungCu.Update(chiTiet);
                await _context.SaveChangesAsync();
                return NoContent(); // Return 204 No Content
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        // DELETE: api/ChiTietDeXuatDungCu/{maPhieu}/{maDungCu}
        [HttpDelete("{maPhieu}/{maDungCu}")]
        public async Task<IActionResult> DeleteChiTietDeXuatDungCu(string maPhieu, string maDungCu)
        {
            var chiTiet = await _context.ChiTietDeXuatDungCu
                .FirstOrDefaultAsync(c => c.MaPhieu == maPhieu && c.MaDungCu == maDungCu);

            if (chiTiet == null)
            {
                return NotFound("ChiTietDeXuatDungCu not found.");
            }

            try
            {
                _context.ChiTietDeXuatDungCu.Remove(chiTiet);
                await _context.SaveChangesAsync();
                return NoContent(); // Return 204 No Content
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
