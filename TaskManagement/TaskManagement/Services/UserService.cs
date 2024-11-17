using TaskManagement.Model;
using TaskManagement.Data;
using TaskManagement.Services.Interfaces;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace TaskManagement.Services
{
    public class UserService : IUserService
    {
        private readonly TaskMgmtDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IPasswordHasher<User> _passwordHasher;
        public UserService(TaskMgmtDbContext context, IConfiguration configuration, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _configuration = configuration;
            _passwordHasher = passwordHasher;
        }

        public async Task<ResponseJson<string>> RegisterUserAsync(UserRegisterDto request)
        {
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
                return new ResponseJson<string> { Success = false, Message = "Email already exists." };

            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                CreatedDate = DateTime.UtcNow
            };
            //PasswordHash = HashPassword(null ,request.Password),
            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new ResponseJson<string> { Data = "User registered successfully." };
        }

        public async Task<ResponseJson<string>> AuthenticateUserAsync(UserLoginDto request)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == request.Email);
            if (user == null || !VerifyPassword(user, request.Password, user.PasswordHash))
                return new ResponseJson<string> { Success = false, Message = "Invalid credentials." };

            var token = GenerateJwtToken(user);
            return new ResponseJson<string> { Data = token };
        }

        public async Task<ResponseJson<List<User>>> GetAllUsersAsync()
        {
            var users = await _context.Users.ToListAsync();
            return new ResponseJson<List<User>> { Data = users };
        }

        public async Task<ResponseJson<User>> GetUserByIdAsync(int id)
        {
            var user = await _context.Users.Include(u => u.Tasks).FirstOrDefaultAsync(u => u.UserId == id);
            if (user == null)
                return new ResponseJson<User> { Success = false, Message = "User not found." };

            return new ResponseJson<User> { Data = user };
        }

        public string HashPassword(User user, string password)
        {
            return _passwordHasher.HashPassword(user, password);
        }

        // Method to verify a password
        public bool VerifyPassword(User user, string password, string hashedPassword)
        {
            var result = _passwordHasher.VerifyHashedPassword(user, hashedPassword, password);
            return result == PasswordVerificationResult.Success;
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Name)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
