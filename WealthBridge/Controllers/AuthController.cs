using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using WealthBridge.Core.Database;
using WealthBridge.Core.Models;
using WealthBridge.Core.Services.Interface;

namespace BorrowerProject.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IAuthService _authService;

        public AuthController(IConfiguration configuration, IAuthService authService)
        {
            _configuration = configuration;
            _authService = authService;
        }

        [HttpPost("signin-google")]
        public async Task<IActionResult> SignInGoogleAsync(AuthenticateRequest model)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"https://www.googleapis.com/oauth2/v3/userinfo?access_token={model.Token}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    string email = JsonConvert.DeserializeObject<dynamic>(apiResponse).email;
                    var user = _authService.FindByEmail(email);
                    if (user != null)
                    {
                        var authResponse = _authService.CreateUserAuthTokenAsync(user);
                        return Ok(authResponse);
                    }
                }
            }
            return Unauthorized();
        }

        [HttpPost("signup-google")]
        public async Task<IActionResult> SignUpAsync(AuthenticateRequest model)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync($"https://www.googleapis.com/oauth2/v3/userinfo?access_token={model.Token}"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        string email = JsonConvert.DeserializeObject<dynamic>(apiResponse).email;
                        string lastName = JsonConvert.DeserializeObject<dynamic>(apiResponse).family_name;
                        string firstName = JsonConvert.DeserializeObject<dynamic>(apiResponse).given_name;
                        var userExists = _authService.FindByEmail(email);
                        if (userExists != null)
                        {
                            return Ok(_authService.CreateUserAuthTokenAsync(userExists));
                        }

                        Borrower user = new()
                        {
                            NameLast = lastName,
                            NameFirst = firstName,
                            Email = email
                        };
                        _authService.Create(user);
                        userExists = _authService.FindByEmail(email);
                        return Ok(_authService.CreateUserAuthTokenAsync(userExists));
                    }
                }
            }
            catch (Exception ex)
            {
                return Unauthorized();
            }
        }
    }
}
