using Microsoft.AspNetCore.Mvc;
using LabManagement.Data;
using LabManagement.Model;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class PhieuDeXuatController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public PhieuDeXuatController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetPhieuDeXuat()
    {
        var phieuDeXuats = await _context.PhieuDeXuat
            .Include(p => p.ThietBi)
            .Include(p => p.NhanVien)
            .ToListAsync();

        return Ok(phieuDeXuats);
    }
    // POST: api/PhieuDeXuat
    [HttpPost]
    public async Task<IActionResult> CreatePhieuDeXuat([FromBody] PhieuDeXuat newPhieuDeXuat)
    {
       

        try
        {
            if (newPhieuDeXuat == null)
            {
                return BadRequest("PhieuDeXuat cannot be null.");
            }

            if (string.IsNullOrEmpty(newPhieuDeXuat.MaThietBi) || string.IsNullOrEmpty(newPhieuDeXuat.MaNV))
            {
                return BadRequest("Both MaThietBi and MaNV are required.");
            }

            _context.PhieuDeXuat.Add(newPhieuDeXuat);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPhieuDeXuat), new { id = newPhieuDeXuat.MaPhieu }, newPhieuDeXuat);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error: " + ex.Message);
        }
    }
    [HttpGet("existing")]
    public async Task<IActionResult> GetExistingPhieuDeXuat()
    {
        var phieuDeXuats = await _context.PhieuDeXuat.Select(p => new { p.MaPhieu }).ToListAsync();
        return Ok(phieuDeXuats);
    }

}
