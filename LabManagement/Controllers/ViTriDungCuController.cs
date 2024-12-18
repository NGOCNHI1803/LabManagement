using LabManagement.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LabManagement.Data;

namespace LabManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ViTriDungCuController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ViTriDungCuController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ViTriDungCu
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ViTriDungCu>>> GetAll()
        {
            return await _context.ViTriDungCu
                .Include(v => v.DungCu)
                .Include(v => v.PhongThiNghiem)
                .ToListAsync();
        }

        // GET: api/ViTriDungCu/ByMaPhong/{maPhong}
        [HttpGet("ByMaPhong/{maPhong}")]
        public async Task<ActionResult<IEnumerable<ViTriDungCu>>> GetByMaPhong(string maPhong)
        {
            var viTriDungCu = await _context.ViTriDungCu
                .Where(v => v.MaPhong == maPhong)
                .Include(v => v.DungCu)
                .Include(v => v.PhongThiNghiem)
                .ToListAsync();

            if (!viTriDungCu.Any())
                return NotFound("No items found for the given MaPhong.");

            return viTriDungCu;
        }

        // GET: api/ViTriDungCu/ByMaDungCu/{maDungCu}
        [HttpGet("ByMaDungCu/{maDungCu}")]
        public async Task<ActionResult<IEnumerable<ViTriDungCu>>> GetByMaDungCu(string maDungCu)
        {
            var viTriDungCu = await _context.ViTriDungCu
                .Where(v => v.MaDungCu == maDungCu)
                .Include(v => v.DungCu)
                .Include(v => v.PhongThiNghiem)
                .ToListAsync();

            if (!viTriDungCu.Any())
                return NotFound("No items found for the given MaDungCu.");

            return viTriDungCu;
        }

        // POST: api/ViTriDungCu
        [HttpPost]
        public async Task<ActionResult<ViTriDungCu>> Create(ViTriDungCu viTriDungCu)
        {
            if (viTriDungCu == null)
                return BadRequest("Invalid data.");

            _context.ViTriDungCu.Add(viTriDungCu);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAll), new { id = viTriDungCu.MaDungCu }, viTriDungCu);
        }

        // PUT: api/ViTriDungCu/{maDungCu}/{maPhong}
        [HttpPut("{maDungCu}/{maPhong}")]
        public async Task<IActionResult> Update(string maDungCu, string maPhong, ViTriDungCu viTriDungCu)
        {
            if (maDungCu != viTriDungCu.MaDungCu || maPhong != viTriDungCu.MaPhong)
                return BadRequest("Mismatch in DungCu or Phong ID.");

            var existingItem = await _context.ViTriDungCu.FirstOrDefaultAsync(v => v.MaDungCu == maDungCu && v.MaPhong == maPhong);

            if (existingItem == null)
                return NotFound("Item not found.");

            existingItem.SoLuong = viTriDungCu.SoLuong;
            existingItem.NgayCapNhat = viTriDungCu.NgayCapNhat;

            _context.Entry(existingItem).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/ViTriDungCu/{maDungCu}/{maPhong}
        [HttpDelete("{maDungCu}/{maPhong}")]
        public async Task<IActionResult> Delete(string maDungCu, string maPhong)
        {
            var viTriDungCu = await _context.ViTriDungCu.FirstOrDefaultAsync(v => v.MaDungCu == maDungCu && v.MaPhong == maPhong);

            if (viTriDungCu == null)
                return NotFound("Item not found.");

            _context.ViTriDungCu.Remove(viTriDungCu);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        // PUT: api/ViTriDungCu/UpdateMaPhong/{maDungCu}
        [HttpPut("UpdateMaPhong/{maDungCu}")]
        public async Task<IActionResult> UpdateMaPhong(string maDungCu, [FromBody] string newMaPhong)
        {
            if (string.IsNullOrEmpty(newMaPhong))
                return BadRequest("New MaPhong cannot be empty.");

            var viTriDungCu = await _context.ViTriDungCu.FirstOrDefaultAsync(v => v.MaDungCu == maDungCu);

            if (viTriDungCu == null)
                return NotFound("Item not found.");

            viTriDungCu.MaPhong = newMaPhong;
            viTriDungCu.NgayCapNhat = DateTime.Now;

            _context.Entry(viTriDungCu).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(viTriDungCu);
        }

    }
}
