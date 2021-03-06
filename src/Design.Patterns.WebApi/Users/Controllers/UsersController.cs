﻿using Design.Patterns.Core;
using Design.Patterns.WebApi.CommonHelpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Design.Patterns.WebApi.Users
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class UsersController : ControllerBase
	{
		private readonly IUserCommandHandlers userCommandHandlers;
		private readonly IUserQueryHandlers userQueryHandlers;

		public UsersController(IUserCommandHandlers userCommandHandlers, IUserQueryHandlers userQueryHandlers)
		{
			this.userCommandHandlers = userCommandHandlers;
			this.userQueryHandlers = userQueryHandlers;
		}

		// GET api/users
		[HttpGet]
		public async Task<IEnumerable<UserView>> GetAsync()
		{
			return (await userQueryHandlers.HandleAsync(new GetUsers(), null))
				.Select(userState => userState.ToUserView());
		}

		// POST api/users/authenticate
		[HttpPost("authenticate")]
		[AllowAnonymous]
		public async Task<UserView> AuthenticateAsync([FromBody] AuthenticateUser authenticateUser)
		{
			var (userState, token) = await userQueryHandlers.HandleAsync(authenticateUser, null);

			return userState
				.ToUserView(token);
		}

		// GET api/users/5
		[HttpGet("{id}")]
		public async Task<UserView> Get(int id)
		{
			return (await userQueryHandlers.HandleAsync(new GetUser { Id = id }, null))
				.ToUserView();
		}

		// POST api/users
		[HttpPost]
		[AllowAnonymous]
		public async Task<UserView> Post([FromBody] CreateUser createUser)
		{
			return (await userCommandHandlers.HandleAsync(createUser, null))
				.ToUserView();
		}

		// PUT api/users/5
		[HttpPut("{id}")]
		public async Task<UserView> Put(int id, [FromBody] UpdateUserDetails updateUserDetails)
		{
			return (await userCommandHandlers.HandleAsync(updateUserDetails, null))
				.ToUserView();
		}

		// PUT api/users/5/updatepassword
		[HttpPut("{id}/updatepassword")]
		public async Task<UserView> Put(int id, [FromBody] UpdateUserPassword updateUserPassword)
		{
			return (await userCommandHandlers.HandleAsync(updateUserPassword, null))
				.ToUserView();
		}

		// PUT api/users/5/addrole
		[HttpPut("{id}/addrole")]
		public async Task<UserView> Put(int id, [FromBody] AddUserRole addUserRole)
		{
			return (await userCommandHandlers.HandleAsync(addUserRole, null))
				.ToUserView();
		}

		// DELETE api/users/5
		[HttpDelete("{id}")]
		[Authorize(Roles = nameof(UserRole.Admin))]
		public async Task<UserView> Delete(long id, [FromBody] DeleteUser deleteUser, [FromHeader] MessageContext messageContext)
		{
			var user = User;

			return (await userCommandHandlers.HandleAsync(deleteUser, messageContext))
				.ToUserView();
		}
	}
}
