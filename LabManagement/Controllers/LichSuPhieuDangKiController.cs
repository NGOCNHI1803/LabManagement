using LabManagement.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LabManagement.Data;

namespace LabManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LichSuPhieuDangKiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public LichSuPhieuDangKiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/LichSuPhieuThanhLy
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LichSuPhieuDangKi>>> GetLichSuPhieuDK()
        {
            return await _context.LichSuPhieuDangKi
                .Include(ls => ls.PhieuDangKi)
                .Include(ls => ls.NhanVien)
                .ToListAsync();
        }

        // GET: api/LichSuPhieuThanhLy/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<LichSuPhieuDangKi>> GetLichSuPhieuDangKiById(int id)
        {
            var lichSu = await _context.LichSuPhieuDangKi
                .Include(ls => ls.PhieuDangKi)
                .Include(ls => ls.NhanVien)
                .FirstOrDefaultAsync(ls => ls.MaLichSu == id);

            if (lichSu == null)
            {
                return NotFound();
            }

            return lichSu;
        }
        // GET: api/LichSuPhieuThanhLy/ByPhieu/{maPhieuTL}
        [HttpGet("ByPhieu/{maPhieuDK}")]
        public async Task<ActionResult<IEnumerable<LichSuPhieuDangKi>>> GetLichSuByMaPhieuDK(string maPhieuDK)
        {
            var lichSuList = await _context.LichSuPhieuDangKi
                .Where(ls => ls.MaPhieuDK == maPhieuDK)
                .Include(ls => ls.PhieuDangKi)
                .ToListAsync();

            if (lichSuList == null || lichSuList.Count == 0)
            {
                return NotFound($"Không tìm thấy lịch sử nào cho Mã Phiếu Thanh Lý: {maPhieuDK}");
            }

            return Ok(lichSuList);
        }


        // POST: api/LichSuPhieuThanhLy
        [HttpPost]
        public async Task<ActionResult<LichSuPhieuDangKi>> CreateLichSuPhieuDangKi(LichSuPhieuDangKi lichSu)
        {
            if (lichSu == null)
            {
                return BadRequest("Invalid data.");
            }

            _context.LichSuPhieuDangKi.Add(lichSu);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLichSuPhieuDangKiById), new { id = lichSu.MaLichSu }, lichSu);
        }

        // PUT: api/LichSuPhieuThanhLy/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLichSuPhieuDangKi(int id, LichSuPhieuDangKi lichSu)
        {
            if (id != lichSu.MaLichSu)
            {
                return BadRequest("ID mismatch.");
            }

            var existingLichSu = await _context.LichSuPhieuDangKi.FindAsync(id);
            if (existingLichSu == null)
            {
                return NotFound();
            }

            existingLichSu.MaPhieuDK = lichSu.MaPhieuDK;
            existingLichSu.TrangThaiTruoc = lichSu.TrangThaiTruoc;
            existingLichSu.TrangThaiSau = lichSu.TrangThaiSau;
            existingLichSu.NgayThayDoi = lichSu.NgayThayDoi;
            existingLichSu.MaNV = lichSu.MaNV;

            _context.Entry(existingLichSu).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
