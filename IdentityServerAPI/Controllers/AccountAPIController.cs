using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using IdentityServerAPI.Models;
using Domain.Models;

namespace IdentityServerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountAPIController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountAPIController(SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
        [HttpPost("login")]
        public async Task<RequestResult> Login([FromBody] LoginModel loginViewModel)
        {
            try
            {
                var res = await _signInManager.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password, true, false);
                RequestResult result = new()
                {
                    Success = res.Succeeded,
                };
                if (!res.Succeeded)
                {
                    result.ErrorMessage = "Wrong login or password";
                }
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new RequestResult()
                {
                    Success = false,
                    ErrorMessage = e.Message
                };
            }
        }
        [HttpPost("register")]
        public async Task<RequestResult> Register([FromBody] RegisterModel registerViewModel)
        {
            var user = new User()
            {
                UserName = registerViewModel.Email,
                Email = registerViewModel.Email,
                Password = registerViewModel.Password,
            };
            IdentityResult res = await _userManager.CreateAsync(user, registerViewModel.Password);
            RequestResult result = new()
            {
                Success = res.Succeeded,
            };
            if (!res.Succeeded)
            {
                result.ErrorMessage += res.Errors.First().Description ?? "" + "<br/>";
            }
            return result;
        }
        [HttpPost("passwordupdate")]
        public async Task<RequestResult> PasswordUpdate([FromBody] RegisterModel registerViewModel)
        {
            var user = await _userManager.FindByEmailAsync(registerViewModel.Email);
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            IdentityResult res = await _userManager.ResetPasswordAsync(user, token, registerViewModel.Password);
            RequestResult result = new()
            {
                Success = res.Succeeded,
            };
            if (!res.Succeeded)
            {
                result.ErrorMessage += res.Errors.First().Description ?? "" + "<br/>";
            }
            return result;
        }
        public bool isLoggedIn()
        {
            var res = _signInManager.IsSignedIn(User);
            return res;
        }
    }
}