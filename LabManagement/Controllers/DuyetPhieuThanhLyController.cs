using LabManagement.Data;
using LabManagement.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LabManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DuyetPhieuThanhLyController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DuyetPhieuThanhLyController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{maPhieuTL}")]
        public async Task<IActionResult> GetByPhieuThanhLyId(string maPhieuTL)
        {
            var approvals = await _context.DuyetPhieuThanhLy
                                          .Where(dp => dp.MaPhieuTL == maPhieuTL)
                                          .ToListAsync();
            return Ok(approvals);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DuyetPhieuThanhLy duyetPhieuThanhLy)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.DuyetPhieuThanhLy.Add(duyetPhieuThanhLy);
            await _context.SaveChangesAsync();
            return Ok(duyetPhieuThanhLy);
        }

        [HttpPut("{maPhieuTL}/{maNV}")]
        public async Task<IActionResult> Update(string maPhieuTL, string maNV, [FromBody] DuyetPhieuThanhLy updatedDuyetPhieuThanhLy)
        {
            if (maPhieuTL != updatedDuyetPhieuThanhLy.MaPhieuTL || maNV != updatedDuyetPhieuThanhLy.MaNV)
            {
                return BadRequest();
            }

            _context.Entry(updatedDuyetPhieuThanhLy).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.DuyetPhieuThanhLy.Any(e => e.MaPhieuTL == maPhieuTL && e.MaNV == maNV))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }
    }
}
