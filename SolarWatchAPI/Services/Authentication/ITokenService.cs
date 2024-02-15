using Microsoft.AspNetCore.Identity;

namespace SolarWatchAPI.Services.Authentication;

public interface ITokenService
{
    string CreateToken(IdentityUser user, string role);
}