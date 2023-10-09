namespace VebTechTestTask.Controllers
{
    using System.IdentityModel.Tokens.Jwt;

    using BLL.Services.Interfaces;

    using Configurations;
    
    using Controllers.Base;

    using Helpers;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;

    using Requests.Identity;

    /// <summary>
    /// Controller for identity-related operations like user authentication.
    /// </summary>
    [AllowAnonymous]
    public class IdentityController : ApiControllerBase
    {
        private readonly JwtTokenConfiguration jwtTokenConfiguration;

        private readonly IUserService userService;

        /// <summary>
        /// Initializes a new instance of the IdentityController class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="jwtTokenConfiguration">The JWT token configuration.</param>
        /// <param name="userService">The user service.</param>
        public IdentityController(
            ILogger<IdentityController> logger, 
            IOptions<JwtTokenConfiguration> jwtTokenConfiguration,
            IUserService userService) 
            : base(logger)
        {
            this.jwtTokenConfiguration = jwtTokenConfiguration.Value;
            this.userService = userService;
        }

        /// <summary>
        /// Authenticate a user and generate a JWT token.
        /// </summary>
        /// <param name="input">The login request containing email and password.</param>
        /// <returns>The JWT token upon successful authentication.</returns>
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest input)
        {
            var userToVerify = await IdentityHelper.CreateClaimsIdentityAsync(userService, input.Email, input.Password);
            if (userToVerify == null)
            {
                return CreateErrorResponse("Invalid data");
            }

            var token = new JwtSecurityToken
            (
                issuer: jwtTokenConfiguration.Issuer,
                audience: jwtTokenConfiguration.Audience,
                claims: userToVerify.Claims,
                expires: jwtTokenConfiguration.EndDate,
                notBefore: jwtTokenConfiguration.StartDate,
                signingCredentials: jwtTokenConfiguration.SigningCredentials
            );

            return CreateOkResponse(new JwtSecurityTokenHandler().WriteToken(token));
        }
    }
}