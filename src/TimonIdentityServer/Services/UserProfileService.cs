using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using TimonIdentityServer.Models;

namespace TimonIdentityServer.Services
{
  public class ProfileService : IProfileService
  {
    private readonly UserManager<ApplicationUser> _userManager;

    public ProfileService(UserManager<ApplicationUser> userManager)
    {
      _userManager = userManager;
    }

    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
      var user = await _userManager.GetUserAsync(context.Subject);

      var userClaims = await _userManager.GetClaimsAsync(user);

      var claims = new List<Claim>
        {
            new Claim("email", user.Email),
        };

      context.IssuedClaims.AddRange(claims);

      var timonUser = userClaims.FirstOrDefault(x => x.Type == "timonUser");
      if (timonUser != null)
      {
        context.IssuedClaims.Add(timonUser);
      }

      var displayName = userClaims.FirstOrDefault(x => x.Type == "timonUserDisplayName");
      if (displayName != null)
      {
        context.IssuedClaims.Add(displayName);
      }
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
      var user = await _userManager.GetUserAsync(context.Subject);

      context.IsActive = user != null;
    }
  }
}
