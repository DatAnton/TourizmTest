using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using TourizmTest.Models;
using TourizmTest.Serializers;

namespace TourizmTest.Controllers.Api
{

    [Route("api/[controller]")]
    // [Route("[controller]/[action]")]
    [ApiController]
    // [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, 
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            if(!_roleManager.Roles.Any())
            {
                _roleManager.CreateAsync(new IdentityRole("Tourist"));
                _roleManager.CreateAsync(new IdentityRole("Owner"));
            }
        }

        [HttpGet("getAccounts")]
        [Authorize]
        public List<string> GetAccounts()
        {
            return new List<string>{ "Customer1", "Customer2" };
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromForm]UserSerializer model)
        {
            if(ModelState.IsValid)
            {
                User user = new User{ Email = model.Email, Age = model.Age, UserName = model.UserName };
                if(!_roleManager.RoleExistsAsync(model.RoleName).Result)
                    return BadRequest("Error role!");
                var result = await _userManager.CreateAsync(user, model.Password);
                if(result.Succeeded)
                {
                    if(_roleManager.RoleExistsAsync(model.RoleName).Result)
                        await _userManager.AddToRoleAsync(user, model.RoleName);
                    
                    return Created("Header", "You are successfully registered!");
                }
                return BadRequest(result.Errors);
                
            }
            // Dictionary<string, List<string>> errors = new Dictionary<string, List<string>>();
            // foreach(var key in ModelState.Keys){
            //     if(ModelState[key].Errors.Count != 0)
            //     {
            //         errors[key] = new List<string>();
            //         foreach(var error in ModelState[key].Errors){
            //             errors[key].Add(error.ErrorMessage);
            //         }
            //     }
            // }
            return BadRequest();
        }

        [HttpPost("CreateToken")]
        public async Task<IActionResult> CreateToken([FromForm] JwtTokenViewModel model)
        {
            if(ModelState.IsValid){
                User user = await _userManager.FindByEmailAsync(model.Email);
                var signInResult = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                if(signInResult.Succeeded)
                {
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(MVSJwtConstants.Key));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var claims = new []
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, model.Email),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Email, model.Email)
                    };

                    var token = new JwtSecurityToken(
                        MVSJwtConstants.Issuer,
                        MVSJwtConstants.Audience,
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(30),
                        signingCredentials: creds
                    );
                    var results = new 
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo
                    };
                    return Created("", results);
                }
                return BadRequest();
            }
            return BadRequest();
        }

        [Authorize]
        [HttpGet("GetIdentity")]
        public async Task<IActionResult> GetIdentity()
        {
            User user = await _userManager.FindByEmailAsync(_userManager.GetUserId(HttpContext.User));
            return Ok(user);
        }

    }
}