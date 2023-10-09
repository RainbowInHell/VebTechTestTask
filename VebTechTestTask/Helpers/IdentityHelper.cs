namespace VebTechTestTask.Helpers
{
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Security.Principal;

    using BLL.Services.Interfaces;

    using DAL.Entities;

    public static class IdentityHelper
    {
        public static async Task<ClaimsIdentity> CreateClaimsIdentityAsync(IUserService userService, string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return null;
            }

            var userToVerify = await userService.GetUserByEmailAsync(email);

            if (userToVerify == null || userToVerify.Password != password)
            {
                return null;
            }

            var claims = CreateUserClaims(userToVerify);
            claims = await CreateRoleClaims(userService, userToVerify.Id, claims);

            return new ClaimsIdentity(new ClaimsIdentity(new GenericIdentity(email, "Token"), claims));
        }

        private static async Task<List<Claim>> CreateRoleClaims(IUserService userService, int id, List<Claim> claims)
        {
            var roles = await userService.GetRolesByUserIdAsync(id);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }

            return claims;
        }

        private static List<Claim> CreateUserClaims(User userToVerify)
        {
            var claims = new List<Claim>(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userToVerify.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("Id", userToVerify.Id.ToString())
            });

            return claims;
        }
    }
}