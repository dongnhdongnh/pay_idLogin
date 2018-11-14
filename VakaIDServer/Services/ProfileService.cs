using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using VakaxaIDServer.Models;

namespace VakaxaIDServer.Services
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            //>Processing
            var user = _userManager.GetUserAsync(context.Subject).Result;
            var userModel = new UserModelClaim
            {
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Birthday = user.Birthday,
                CountryCode = user.CountryCode,
                Country = user.Country,
                PostalCode = user.PostalCode,
                StreetAddress = user.StreetAddress,
                Avatar = user.Avatar
            };

            var json = JsonConvert.SerializeObject(userModel);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtClaimTypes.Id, user.Id),
                new Claim("userInfo", json)
            };

            context.IssuedClaims.AddRange(claims);

            //>Return
            return Task.FromResult(0);
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            //>Processing
            var user = _userManager.GetUserAsync(context.Subject).Result;

            context.IsActive = user != null;

            //>Return
            return Task.FromResult(0);
        }
    }
}