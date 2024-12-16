﻿using Microsoft.AspNetCore.Mvc;
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

            if (string.IsNullOrEmpty(newPhieuDeXuat.MaNV))
            {
                return BadRequest("MaNV are required.");
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
    // GET: api/PhieuDeXuat/{maPhieu}
    [HttpGet("{maPhieu}")]
    public async Task<IActionResult> GetPhieuDeXuatByMaPhieu(string maPhieu)
    {
        // Find the proposal with the given MaPhieu
        var phieuDeXuat = await _context.PhieuDeXuat
            .Include(p => p.NhanVien) // Include NhanVien details
            .FirstOrDefaultAsync(p => p.MaPhieu == maPhieu); // Search by MaPhieu

        // If the proposal is not found, return a 404 Not Found
        if (phieuDeXuat == null)
        {
            return NotFound($"PhieuDeXuat with MaPhieu '{maPhieu}' not found.");
        }

        // Return the found proposal details
        return Ok(phieuDeXuat);
    }


}
