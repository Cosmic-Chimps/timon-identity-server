using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel.Client;
using IdentityServer4;
using IdentityServer4.Events;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TimonIdentityServer.Config;
using TimonIdentityServer.Models;
using TimonIdentityServer.Tos;
using TimonIdentityServer.ViewModels;

namespace TimonIdentityServer.Controllers
{
    [ApiController]
    public class TimonAccountController : Controller
    {
        private readonly IEventService _events;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IOptions<IdentityServerOptions> _identityServerOptions;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public TimonAccountController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager,
            IHttpClientFactory httpClientFactory, IEventService events,
            IOptions<IdentityServerOptions> identityServerOptions)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _httpClientFactory = httpClientFactory;
            _events = events;
            _identityServerOptions = identityServerOptions;
        }

        [HttpGet("/")]
        public IActionResult Index()
        {
            return Ok();
        }

        [HttpPost("/register")]
        public async Task<IActionResult> Register([FromBody] RegisterTo model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = new ApplicationUser
            {
                UserName = model.UserName, FirstName = model.FirstName, LastName = model.LastName, Email = model.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, model.Password).ConfigureAwait(false);

            var role = _identityServerOptions.Value.UserDefaultRole;

            if (!result.Succeeded) return BadRequest(result.Errors);

            if (await _roleManager.FindByNameAsync(role) == null)
                await _roleManager.CreateAsync(new IdentityRole(role));

            await _userManager.AddToRoleAsync(user, role);
            await _userManager.AddClaimAsync(user, new Claim("userName", user.UserName));
            await _userManager.AddClaimAsync(user, new Claim("firstName", user.FirstName));
            await _userManager.AddClaimAsync(user, new Claim("lastName", user.LastName));
            await _userManager.AddClaimAsync(user, new Claim("email", user.Email));
            await _userManager.AddClaimAsync(user, new Claim("role", role));

            return await GetTokenAsync(user, model.Password);
        }

        [HttpPost("/login")]
        public async Task<IActionResult> Login([FromBody] LoginViewTo to)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(to.Email).ConfigureAwait(false);

            var result = await _signInManager.PasswordSignInAsync(user.UserName, to.Password, false, false)
                .ConfigureAwait(false);

            if (!result.Succeeded) return BadRequest();

            await _events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id, user.UserName));

            return await GetTokenAsync(user, to.Password);
        }

        private async Task<IActionResult> GetTokenAsync(ApplicationUser user, string password)
        {
            var serverClient = _httpClientFactory.CreateClient();
            var discoveryDocument = await serverClient.GetDiscoveryDocumentAsync(_identityServerOptions.Value.EndPoint);

            var tokenResponse = await serverClient.RequestPasswordTokenAsync(
                new PasswordTokenRequest
                {
                    Address = discoveryDocument.TokenEndpoint,
                    ClientId = _identityServerOptions.Value.ClientId,
                    ClientSecret = _identityServerOptions.Value.ClientSecret,
                    UserName = user.NormalizedUserName,
                    Password = password,
                    Scope =
                        $"{_identityServerOptions.Value.ClientApp} {IdentityServerConstants.StandardScopes.OpenId} {IdentityServerConstants.StandardScopes.Profile} {IdentityServerConstants.StandardScopes.OfflineAccess}"
                });


            return Json(new TokenViewModel
            {
                Scope = tokenResponse.Scope,
                AccessToken = tokenResponse.AccessToken,
                ExpiresIn = tokenResponse.ExpiresIn,
                RefreshToken = tokenResponse.RefreshToken,
                TokenType = tokenResponse.TokenType
            });
        }
    }
}