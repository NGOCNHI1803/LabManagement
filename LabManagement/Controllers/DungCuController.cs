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
    public class DungCuController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private const string ImageDirectory = @"D:\Ky1_2024_2025\DoAnChuyenNganh\BE\LabManagement\LabManagement\Image\DungCu";
        private const string ImageBaseUrl = "http://localhost/images/DungCu";
        public DungCuController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/dungcu
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DungCu>>> GetAllDungCu()
        {
            var dungCus = await _context.DungCu
                .Include(dc => dc.LoaiDungCu)
                .Include(dc => dc.NhaCungCap)
                .ToListAsync();

            return dungCus;
        }

        // GET: api/dungcu/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<DungCu>> GetDungCuById(string id)
        {
            var dungCu = await _context.DungCu
                .Include(dc => dc.LoaiDungCu)
                .Include(dc => dc.NhaCungCap)
                .FirstOrDefaultAsync(dc => dc.MaDungCu == id);

            if (dungCu == null)
            {
                return NotFound();
            }

            return dungCu;
        }

        // POST: api/dungcu
        [HttpPost]
        public async Task<ActionResult<DungCu>> CreateDungCu(DungCu dungCu)
        {
            if (!string.IsNullOrEmpty(dungCu.HinhAnh))
            {
                string fullImagePath = Path.Combine(ImageDirectory, dungCu.HinhAnh);
                if (!System.IO.File.Exists(fullImagePath))
                {
                    return BadRequest("Hình ảnh không tồn tại.");
                }
            }

            _context.DungCu.Add(dungCu);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDungCuById), new { id = dungCu.MaDungCu }, dungCu);
        }

        // PUT: api/dungcu/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDungCu(string id, DungCu dungCu)
        {
            if (id != dungCu.MaDungCu)
            {
                return BadRequest("Mã dụng cụ không khớp.");
            }

            if (!string.IsNullOrEmpty(dungCu.HinhAnh))
            {
                string fullImagePath = Path.Combine(ImageDirectory, dungCu.HinhAnh);
                if (!System.IO.File.Exists(fullImagePath))
                {
                    return BadRequest("Hình ảnh không tồn tại.");
                }
            }

            _context.Entry(dungCu).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DungCuExists(id))
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

        // DELETE: api/dungcu/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDungCu(string id)
        {
            var dungCu = await _context.DungCu.FindAsync(id);
            if (dungCu == null)
            {
                return NotFound();
            }

            _context.DungCu.Remove(dungCu);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DungCuExists(string id)
        {
            return _context.DungCu.Any(e => e.MaDungCu == id);
        }
    }
}
