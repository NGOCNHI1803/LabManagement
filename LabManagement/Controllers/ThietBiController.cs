using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LabManagement.Data;
using LabManagement.Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LabManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThietBiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private const string ImageDirectory = @"D:\Project\LabManagement\LabManagement\LabManagement\Image\ThietBi";
        private const string ImageBaseUrl = "http://localhost:5123/images/ThietBi";

        public ThietBiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/thietbi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ThietBi>>> GetAllThietBi()
        {
            var thietBis = await _context.ThietBi
                .Include(tb => tb.LoaiThietBi)
                .Include(tb => tb.NhaCungCap)
                .Include(tb => tb.PhongThiNghiem)
                .ToListAsync();

            return thietBis;
        }

        // GET: api/thietbi/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ThietBi>> GetThietBiById(string id)
        {
            var thietBi = await _context.ThietBi
                .Include(tb => tb.LoaiThietBi)
                .Include(tb => tb.NhaCungCap)
                .Include(tb => tb.PhongThiNghiem)
                .FirstOrDefaultAsync(tb => tb.MaThietBi == id);

            if (thietBi == null)
            {
                return NotFound();
            }

            return thietBi;
        }

        // POST: api/thietbi
        [HttpPost]
        public async Task<ActionResult<ThietBi>> CreateThietBi(ThietBi thietBi)
        {
            if (!string.IsNullOrEmpty(thietBi.HinhAnh))
            {
                string fullImagePath = Path.Combine(ImageDirectory, thietBi.HinhAnh);
                if (!System.IO.File.Exists(fullImagePath))
                {
                    return BadRequest("Hình ảnh không tồn tại.");
                }
            }

            _context.ThietBi.Add(thietBi);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetThietBiById), new { id = thietBi.MaThietBi }, thietBi);
        }

        // PUT: api/thietbi/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateThietBi(string id, ThietBi thietBi)
        {
            if (id != thietBi.MaThietBi)
            {
                return BadRequest("Mã thiết bị không khớp.");
            }

            if (!string.IsNullOrEmpty(thietBi.HinhAnh))
            {
                string fullImagePath = Path.Combine(ImageDirectory, thietBi.HinhAnh);
                if (!System.IO.File.Exists(fullImagePath))
                {
                    return BadRequest("Hình ảnh không tồn tại.");
                }
            }

            _context.Entry(thietBi).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ThietBiExists(id))
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

        // DELETE: api/thietbi/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteThietBi(string id)
        {
            var thietBi = await _context.ThietBi.FindAsync(id);
            if (thietBi == null)
            {
                return NotFound();
            }

            _context.ThietBi.Remove(thietBi);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ThietBiExists(string id)
        {
            return _context.ThietBi.Any(e => e.MaThietBi == id);
        }
        // GET: api/thietbi/loaithethicbi/{maLoaiThietBi}
        [HttpGet("LoaiThietBi/{maLoaiThietBi}")]
        public async Task<ActionResult<IEnumerable<ThietBi>>> GetThietBisByLoaiThietBi(string maLoaiThietBi)
        {
            try
            {
                var thietBis = await _context.ThietBi
                    .Where(tb => tb.MaLoaiThietBi == maLoaiThietBi)
                    .Include(tb => tb.LoaiThietBi)
                    .Include(tb => tb.NhaCungCap)
                    .Include(tb => tb.PhongThiNghiem)
                    .ToListAsync();

                if (!thietBis.Any())
                {
                    return NotFound("Không tìm thấy thiết bị nào thuộc loại này.");
                }

                return Ok(thietBis); // Trả về danh sách thiết bị
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi: {ex.Message}");
            }
        }


    }
}
