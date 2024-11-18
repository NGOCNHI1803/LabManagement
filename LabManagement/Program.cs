using LabManagement;
using LabManagement.Data;
using LabManagement.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.IO;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// JWT Configuration
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
string secretKey = jwtSettings["SecretKey"];
string issuer = jwtSettings["Issuer"];
string audience = jwtSettings["Audience"];

builder.Services.AddSingleton(new JwtTokenService(secretKey, issuer, audience));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});

// Add policies for role-based authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("GiamDocTrungTam", policy => policy.RequireClaim("NhomQuyen", "GiamDocTrungTam"));
    options.AddPolicy("ChuyenVien", policy => policy.RequireClaim("NhomQuyen", "ChuyenVien"));
    options.AddPolicy("NguoiDung", policy => policy.RequireClaim("NhomQuyen", "NguoiDung"));
});

// Configure your database context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost3000", builder =>
    {
        builder.WithOrigins("http://localhost:3000") // Adjust to match your frontend URL
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable static files
app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(@"D:\Ky1_2024_2025\DoAnChuyenNganh\BE\LabManagement\LabManagement\Image\DungCu"),
    RequestPath = "/images/DungCu"
});

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(@"D:\Ky1_2024_2025\DoAnChuyenNganh\BE\LabManagement\LabManagement\Image\ThietBi"),
    RequestPath = "/images/ThietBi"
});

app.UseHttpsRedirection();

// Apply CORS policy
app.UseCors("AllowLocalhost3000");

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
