using LabManagement.Data;
using LabManagement.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LabManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChiTietPhieuThanhLyController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ChiTietPhieuThanhLyController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{maPhieuTL}")]
        public async Task<IActionResult> GetByPhieuThanhLyId(string maPhieuTL)
        {
            var details = await _context.ChiTietPhieuThanhLy
                                        .Where(ct => ct.MaPhieuTL == maPhieuTL)
                                        .ToListAsync();
            return Ok(details);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ChiTietPhieuThanhLy chiTietPhieuThanhLy)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.ChiTietPhieuThanhLy.Add(chiTietPhieuThanhLy);
            await _context.SaveChangesAsync();
            return Ok(chiTietPhieuThanhLy);
        }

        [HttpDelete("{maPhieuTL}/{maThietBi}")]
        public async Task<IActionResult> Delete(string maPhieuTL, string maThietBi)
        {
            var detail = await _context.ChiTietPhieuThanhLy.FindAsync(maPhieuTL, maThietBi);
            if (detail == null)
            {
                return NotFound();
            }

            _context.ChiTietPhieuThanhLy.Remove(detail);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
