using System;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityModel.Client;
using IdentityServer4;
using IdentityServer4.Events;
using IdentityServer4.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TimonIdentityServer.Config;
using TimonIdentityServer.Models;
using TimonIdentityServer.Tos;
using TimonIdentityServer.ViewModels;
using TimonIdentityServer.Services;

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
        private readonly IMediator _mediator;

        public TimonAccountController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager,
            IHttpClientFactory httpClientFactory, IEventService events,
            IOptions<IdentityServerOptions> identityServerOptions,
            IMediator mediator)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _httpClientFactory = httpClientFactory;
            _events = events;
            _identityServerOptions = identityServerOptions;
            _mediator = mediator;
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
                UserName = model.UserName,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
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

            var token = await GetTokenAsync(user, model.Password);

            // TODO: publish create user
            var payload = new PublishRegisteredUser(id: Guid.Parse(user.Id), email: user.Email, token.AccessToken);
            var response = await _mediator.Send(payload);

            await _userManager.AddClaimAsync(user, new Claim("timonUser", response.UserId));
            await _userManager.AddClaimAsync(user, new Claim("timonUserDisplayName", response.DisplayName));

            token = await GetTokenAsync(user, model.Password);

            return Json(token);
        }

        [HttpPost("/login")]
        public async Task<IActionResult> Login([FromBody] LoginViewTo to)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(to.Email).ConfigureAwait(false);

            if (user == null)
            {
                return NotFound();
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, to.Password, false, false)
                .ConfigureAwait(false);

            if (!result.Succeeded) return BadRequest();

            await _events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id, user.UserName));

            var token = await GetTokenAsync(user, to.Password);

            return Json(token);
        }

        [HttpPost("/refresh-token")]
        public async Task<IActionResult> RenewToken([FromBody] RefreshTokenTo to)
        {
            var serverClient = _httpClientFactory.CreateClient();
            var discoveryDocument = await serverClient.GetDiscoveryDocumentAsync(_identityServerOptions.Value.EndPoint);

            var tokenResponse = await serverClient.RequestRefreshTokenAsync(
                new RefreshTokenRequest()
                {
                    Address = discoveryDocument.TokenEndpoint,
                    ClientId = _identityServerOptions.Value.ClientId,
                    ClientSecret = _identityServerOptions.Value.ClientSecret,
                    GrantType = "refresh_token",
                    RefreshToken = to.RefreshToken
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

        private async Task<TokenViewModel> GetTokenAsync(ApplicationUser user, string password)
        {
            var serverClient = _httpClientFactory.CreateClient();
            var discoveryDocument = await serverClient.GetDiscoveryDocumentAsync(_identityServerOptions.Value.EndPoint);

            var tokenResponse = await serverClient.RequestPasswordTokenAsync(
                new PasswordTokenRequest
                {
                    Address = discoveryDocument.TokenEndpoint,
                    ClientId = _identityServerOptions.Value.ClientId,
                    ClientSecret = _identityServerOptions.Value.ClientSecret,
                    UserName = user.UserName,
                    Password = password,
                    Scope =
                        $"{_identityServerOptions.Value.ClientApp} {IdentityServerConstants.StandardScopes.OpenId} {IdentityServerConstants.StandardScopes.Profile} {IdentityServerConstants.StandardScopes.OfflineAccess}"
                });

            return new TokenViewModel
            {
                Scope = tokenResponse.Scope,
                AccessToken = tokenResponse.AccessToken,
                ExpiresIn = tokenResponse.ExpiresIn,
                RefreshToken = tokenResponse.RefreshToken,
                TokenType = tokenResponse.TokenType
            };
        }
    }
}
