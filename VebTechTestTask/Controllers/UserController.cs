namespace VebTechTestTask.Controllers
{
    using System;
    using System.Net;

    using AutoMapper;

    using BLL.Services.Interfaces;

    using Controllers.Base;

    using DAL.Entities;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Requests.User;
    
    using ViewModels;

    /// <summary>
    /// Controller for managing user-related operations.
    /// </summary>
    [Authorize]
    public class UserController : ApiControllerBase
    {
        private readonly IUserService userService;

        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the UserController class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="userService">The user service.</param>
        /// <param name="mapper">The AutoMapper instance.</param>
        public UserController(ILogger<UserController> logger, IUserService userService, IMapper mapper)
            : base(logger)
        {
            this.userService = userService;
            this.mapper = mapper;
        }

        /// <summary>
        /// Get a list of users.
        /// </summary>
        /// <param name="sortBy">Optional. The field to sort by.</param>
        /// <param name="descending">Optional. Sort in descending order.</param>
        /// <param name="filter">Optional. A filter for user names.</param>
        /// <param name="page">Optional. The page number (default is 1).</param>
        /// <param name="pageSize">Optional. The page size (default is 10).</param>
        /// <returns>A list of users.</returns>
        [HttpGet]
        public async Task<IActionResult> GetUsers(
            string? sortBy,
            bool descending,
            string? filter,
            int page = 1,
            int pageSize = 10)
        {
            try
            {
                var users = await userService.GetUsersAsync(sortBy, descending, filter, page, pageSize);

                if (users == null || users.Count == 0)
                {
                    return CreateErrorResponse("Users not found.", HttpStatusCode.NotFound);
                }

                return CreateOkResponse(mapper.Map<List<UserViewModel>>(users));
            }
            catch (Exception ex)
            {
                return CreateErrorResponse(ex.Message);
            }
        }

        /// <summary>
        /// Get a user by ID.
        /// </summary>
        /// <param name="id">The user's ID.</param>
        /// <returns>The user details.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var user = await userService.GetUserByIdAsync(id);

                if (user == null)
                {
                    return CreateErrorResponse("User not found.", HttpStatusCode.NotFound);
                }

                return CreateOkResponse(mapper.Map<UserViewModel>(user));
            }
            catch (Exception ex)
            {
                return CreateErrorResponse(ex.Message);
            }
        }

        /// <summary>
        /// Assign a role to a user.
        /// </summary>
        /// <param name="userId">The user's ID.</param>
        /// <param name="roleId">The role's ID.</param>
        /// <returns>HTTP status code indicating success.</returns>
        [HttpPost("role")]
        public async Task<IActionResult> AssignRoleToUser(int userId, int roleId)
        {
            try
            {
                await userService.AssignRoleToUserAsync(userId, roleId);

                return CreateOkResponse(HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                return CreateErrorResponse(ex.Message);
            }
        }

        /// <summary>
        /// Create a new user.
        /// </summary>
        /// <param name="request">The user data.</param>
        /// <returns>HTTP status code indicating success.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateUser(AddUserRequest request)
        {
            try
            {
                await userService.CreateUserAsync(mapper.Map<User>(request));

                return CreateOkResponse(HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                return CreateErrorResponse(ex.Message);
            }
        }

        /// <summary>
        /// Update an existing user.
        /// </summary>
        /// <param name="request">The updated user data.</param>
        /// <returns>HTTP status code indicating success.</returns>
        [HttpPut]
        public async Task<IActionResult> UpdateUser(UpdateUserRequest request)
        {
            try
            {
                await userService.UpdateUserAsync(mapper.Map<User>(request));

                return CreateOkResponse(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                return CreateErrorResponse(ex.Message);
            }
        }

        /// <summary>
        /// Delete a user by ID.
        /// </summary>
        /// <param name="id">The user's ID.</param>
        /// <returns>HTTP status code indicating success.</returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteUserById(int id)
        {
            try
            {
                await userService.DeleteUserByIdAsync(id);

                return CreateOkResponse(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                return CreateErrorResponse(ex.Message);
            }
        }
    }
}