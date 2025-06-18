using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using backend.Dtos;
using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

public class AuthService
{
    private readonly CMContext _context;
    private readonly IConfiguration _config;


    public AuthService(CMContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    // Register user
    public async Task<bool> RegisterAsync(RegisterReq model)
    {
        if (await _context.AspNetUsers.AnyAsync(u => u.Email == model.Email))
            return false; // Email đã tồn tại

        // Hash password (bạn có thể dùng BCrypt hoặc ASP.NET Identity PasswordHasher)
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);

        var user = new AspNetUser
        {
            Id = Guid.NewGuid().ToString(),
            FullName = model.FullName,
            UserName = model.UserName,
            NormalizedUserName = model.UserName.ToUpper(),
            Email = model.Email,
            NormalizedEmail = model.Email.ToUpper(),
            PasswordHash = passwordHash,
            SecurityStamp = Guid.NewGuid().ToString(),
            PhoneNumber = model.PhoneNumber,
            IsTeacher = model.Role.ToLower() == "teacher",
            IsStudent = model.Role.ToLower() == "student"
            // thêm các trường khác nếu cần
        };

        await _context.AspNetUsers.AddAsync(user);
        await _context.SaveChangesAsync();

        // Gán Role
        var role = await _context.AspNetRoles.FirstOrDefaultAsync(r => r.Name == model.Role);
        if (role == null) return false;

        _context.Database.ExecuteSqlRaw(
            "INSERT INTO \"AspNetUserRoles\" (\"UserId\", \"RoleId\") VALUES ({0}, {1})",
            user.Id, role.Id);

        await _context.SaveChangesAsync();

        return true;
    }

    // Login user và trả về token
    public async Task<string> LoginAsync(LoginReq model)
    {
        var user = await _context.AspNetUsers
            .Include(u => u.Roles)
            .ThenInclude(r => r.Users) // không cần, chỉ để minh họa
            .FirstOrDefaultAsync(u => u.Email == model.Email);

        if (user == null) return null;

        // Kiểm tra mật khẩu
        if (!BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
            return null;

        // Lấy role của user
        var userWithRoles = await _context.AspNetUsers
        .Include(u => u.Roles)
        .FirstOrDefaultAsync(u => u.Id == user.Id);

        var userRoleNames = userWithRoles?.Roles.Select(r => r.Name).ToList();

        return GenerateJwtToken(user, userRoleNames);
    }

    private string GenerateJwtToken(AspNetUser user, List<string> roles)
    {
        var jwtSettings = _config.GetSection("JwtSettings");

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.Email)
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpiryMinutes"])),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
