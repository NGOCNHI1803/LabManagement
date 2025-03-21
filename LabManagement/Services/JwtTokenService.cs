﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LabManagement.Model;
using Microsoft.IdentityModel.Tokens;

namespace LabManagement.Services
{
    public class JwtTokenService
    {
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;

        public JwtTokenService(string secretKey, string issuer, string audience)
        {
            _secretKey = secretKey;
            _issuer = issuer;
            _audience = audience;
        }

        public string GenerateJwtToken(NhanVien nhanVien)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, nhanVien.MaNV),
                new Claim(ClaimTypes.Name, nhanVien.TenNV),
                new Claim("NhomQuyen", nhanVien.NhomQuyen?.TenNhom ?? string.Empty), // Thêm NhomQuyen vào claims
                new Claim(ClaimTypes.Email, nhanVien.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
