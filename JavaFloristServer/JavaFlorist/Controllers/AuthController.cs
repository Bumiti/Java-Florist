﻿using JavaFlorist.Data;
using JavaFlorist.Models;
using JavaFlorist.Models.Accounts;
using JavaFlorist.Models.Emails;
using JavaFlorist.Repositories.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Web;

namespace JavaFlorist.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IAccountService _accountService;
        private readonly IMailService _mail;

        private readonly JavaFloristDbContext _context;
        public static User userDetial = new User();

        public AuthController(IConfiguration configuration, IAccountService accountService, JavaFloristDbContext context, IMailService mail)
        {
            _configuration = configuration;
            _accountService = accountService;
            _context = context;
            _mail = mail;
        }

        [HttpGet, Authorize]
        public ActionResult<string> GetMe()
        {

            var userName = _accountService.GetMyName();
            return Ok(userName);
            /*var userName = User?.Identity.Name;
            var userName2 = User.FindFirstValue(ClaimTypes.Name);
            var role = User.FindFirstValue(ClaimTypes.Role);
            return Ok(new {userName, userName2, role});*/
        }

        // Add a route in your backend to handle the verification action
        [HttpPut("verify-blog")]
        public async Task<IActionResult> VerifyBlog(Blog id)
        {
            if (id == null)
            {
                // Handle invalid or missing identifier
                return BadRequest("Invalid user identifier");
            }
            else
            {
                var blog = await _context.Blogs.FirstOrDefaultAsync(u => u.Id == id.Id);
                // Update the user's StatusId to 2
                blog.StatusId = 2;
                _context.Blogs.Update(blog);
                await _context.SaveChangesAsync();
                return Ok("Blog verification successful!");
            }
        }



        [HttpGet("verify-email")]
        public async Task<IActionResult> VerifyEmail(string userMail)
        {
            if (string.IsNullOrEmpty(userMail))
            {
                // Handle invalid or missing identifier
                return BadRequest("Invalid user identifier");
            }

            // Find the user based on the identifier (e.g., Email or UserId) extracted from the link
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userMail);

            if (user == null)
            {
                // Handle user not found
                return NotFound();
            }
            else
            {
                // Update the user's StatusId to 2
                user.StatusId = 2;
                await _context.SaveChangesAsync();

                var voucher = new Voucher
                {
                    Code = "First Order",
                    UserId = user.Id,
                    DiscountPercent = 10,
                    Type = "Avaiable"
                };
                _context.Vouchers.Add(voucher);
                await _context.SaveChangesAsync();

                var mailData = new VoucherMail
                {
                    To = user.Email,

                    From = "javafloristshop@gmail.com",
                    DisplayName = "Java Florist",

                    Subject = " Voucher for first order",
                    Body = "First Order"
                };
                _mail.SendMailAsync(mailData, default);
                // Return a success response or redirect the user to a success page
                return Ok("Email verification successful!");
            }

        }

        [HttpPost("signUp")]
        public async Task<ActionResult<User>> SignUp(UserLogin login)
        {
            if (_context.Users.Any(u => u.Email == login.Email))
            {
                return BadRequest("User already exists.");
            }
            CreatePasswordHash(login.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var user = new User
            {
                Email = login.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Role = "User",
                StatusId = 1
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();



            // Gui token ve mail
            // Bam vo link de chay tuong tu resetpass
            // neu dung thi lam tiep cai duoi
            string verificationLink = $"http://localhost:5026/api/Auth/verify-email?userMail={HttpUtility.UrlEncode(user.Email)}";

            var mailVerify = new VoucherMail
            {
                To = user.Email,

                From = "javafloristshop@gmail.com",
                DisplayName = "Java Florist",

                Subject = " Verify Mail",
                Body = $"Click the link below to verify your email: {verificationLink}"
            };
            _mail.SendMailAsync(mailVerify, default);

            return Ok("User successfully created!");
        }

        [HttpPost("signUpFlorist"), Authorize]
        public async Task<ActionResult<Florist>> SignUpFlorist(Florist model)
        {
            try
            {
                var userName = await _accountService.GetMyName();
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.Equals(userName));
                if (user.StatusId == 2)
                {
                    var florist = new Florist
                    {
                        Name = model.Name,
                        Logo = model.Logo,
                        Email = user.Email,
                        Phone = user.Phone,
                        Address = user.Address,
                        UserId = user.Id,
                        StatusId = 2
                    };
                    await _context.Florists.AddAsync(florist);
                    user.Role = "Florist";
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();
                    var mailData = new VoucherMail
                    {
                        To = florist.Email,

                        From = "javafloristshop@gmail.com",
                        DisplayName = "Java Florist",

                        Subject = "Success Register",
                        Body = "Start your bussiness"
                    };
                    _mail.SendMailAsync(mailData, default);
                    return Ok(florist);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return null; // An error occured
            }
        }
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserLogin login)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == login.Email);

            if (user == null)
            {
                return BadRequest("User not found.");
            }
            if (user.Email != login.Email)
            {
                return BadRequest("User not found.");
            }

            if (!VerifyPasswordHash(login.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Wrong password.");
            }

            if (user.StatusId == 1)
            {
                return BadRequest("Unverify.");
            }

            string token = CreateToken(user);

            var refreshToken = GenerateRefreshToken();
            SetRefreshToken(refreshToken);

            return Ok(token);
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            string token = CreateToken(user);
            user.PasswordResetToken = token;
            user.TokenExpires = DateTime.Now.AddDays(1);
            await _context.SaveChangesAsync();

            return Ok("You may now reset your password.");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResettPassword(ResetPasswordRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.PasswordResetToken == request.Token);
            if (user == null || user.TokenExpires < DateTime.Now)
            {
                return BadRequest("Invalid Token.");
            }

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.PasswordResetToken = null;
            user.TokenExpires = null;

            await _context.SaveChangesAsync();

            return Ok("Password successfully reset.");
        }
        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Role)

            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private string CreateVerifyToken(UserLogin user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<string>> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (!userDetial.RefreshToken.Equals(refreshToken))
            {
                return Unauthorized("Invalid Refresh Token.");
            }
            else if (userDetial.TokenExpires < DateTime.Now)
            {
                return Unauthorized("Token expired.");
            }

            string token = CreateToken(userDetial);
            var newRefreshToken = GenerateRefreshToken();
            SetRefreshToken(newRefreshToken);

            return Ok(token);
        }

        private void SetRefreshToken(RefreshToken newRefreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires
            };
            Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);

            userDetial.RefreshToken = newRefreshToken.Token;
            userDetial.TokenCreated = newRefreshToken.Created;
            userDetial.TokenExpires = newRefreshToken.Expires;
        }

        private RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddDays(7),
                Created = DateTime.Now
            };

            return refreshToken;
        }
    }
}
