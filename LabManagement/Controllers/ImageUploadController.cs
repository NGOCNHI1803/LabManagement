using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class ImageUploadController : ControllerBase
{
    private readonly string _dungCuPath = @"D:\Ky1_2024_2025\DoAnChuyenNganh\BE\LabManagement\LabManagement\Image\DungCu";
    private readonly string _thietBiPath = @"D:\Ky1_2024_2025\DoAnChuyenNganh\BE\LabManagement\LabManagement\Image\ThietBi";

    [HttpPost("uploadDungCu")]
    public async Task<IActionResult> UploadDungCu(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("Không có tệp hình ảnh nào được chọn.");
        }

        var filePath = Path.Combine(_dungCuPath, file.FileName);

        // Lưu file vào thư mục "DungCu"
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return Ok(new { FilePath = $"/images/DungCu/{file.FileName}" });
    }

    [HttpPost("uploadThietBi")]
    public async Task<IActionResult> UploadThietBi(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("Không có tệp hình ảnh nào được chọn.");
        }

        var filePath = Path.Combine(_thietBiPath, file.FileName);

        // Lưu file vào thư mục "ThietBi"
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return Ok(new { FilePath = $"/images/ThietBi/{file.FileName}" });
    }
}
