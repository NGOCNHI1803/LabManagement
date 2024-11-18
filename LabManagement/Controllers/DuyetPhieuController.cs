using LabManagement.Data;
using LabManagement.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace LabManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DuyetPhieuController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DuyetPhieuController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/DuyetPhieuDK
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DuyetPhieuDK>>> GetDuyetPhieuDK()
        {
            return await _context.DuyetPhieuDK.ToListAsync();
        }

        // GET: api/DuyetPhieuDK/{maPhieu}
        [HttpGet("{maPhieu}")]
        public async Task<ActionResult<DuyetPhieuDK>> GetDuyetPhieuDKById(string maPhieu)
        {
            var DuyetPhieuDK = await _context.DuyetPhieuDK.FindAsync(maPhieu);
            if (DuyetPhieuDK == null)
            {
                return NotFound();
            }
            return DuyetPhieuDK;
        }

        // PUT: api/DuyetPhieuDK/approve/{maPhieu}
        [HttpPut("approve/{maPhieu}")]
        public async Task<IActionResult> ApprovePhieu(string maPhieu, [FromBody] string maNV)
        {
            var DuyetPhieuDK = await _context.DuyetPhieuDK.FindAsync(maPhieu);
            if (DuyetPhieuDK == null)
            {
                return NotFound();
            }

            DuyetPhieuDK.TrangThai = "Approved";
            DuyetPhieuDK.NgayDuyet = DateTime.Now;
            DuyetPhieuDK.MaNV = maNV;

            _context.Entry(DuyetPhieuDK).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT: api/DuyetPhieuDK/reject/{maPhieu}
        [HttpPut("reject/{maPhieu}")]
        public async Task<IActionResult> RejectPhieu(string maPhieu, [FromBody] string maNV)
        {
            var DuyetPhieuDK = await _context.DuyetPhieuDK.FindAsync(maPhieu);
            if (DuyetPhieuDK == null)
            {
                return NotFound();
            }

            DuyetPhieuDK.TrangThai = "Rejected";
            DuyetPhieuDK.NgayDuyet = DateTime.Now;
            DuyetPhieuDK.MaNV = maNV;

            _context.Entry(DuyetPhieuDK).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/DuyetPhieuDK
        [HttpPost]
        public async Task<ActionResult<DuyetPhieuDK>> CreateDuyetPhieuDK(DuyetPhieuDK DuyetPhieuDK)
        {
            _context.DuyetPhieuDK.Add(DuyetPhieuDK);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetDuyetPhieuDKById), new { maPhieu = DuyetPhieuDK.MaPhieu }, DuyetPhieuDK);
        }

        // DELETE: api/DuyetPhieuDK/{maPhieu}
        [HttpDelete("{maPhieu}")]
        public async Task<IActionResult> DeleteDuyetPhieuDK(string maPhieu)
        {
            var DuyetPhieuDK = await _context.DuyetPhieuDK.FindAsync(maPhieu);
            if (DuyetPhieuDK == null)
            {
                return NotFound();
            }

            _context.DuyetPhieuDK.Remove(DuyetPhieuDK);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
