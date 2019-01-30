using System;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Api.DTO;
using SecretSanta.Domain.Services;

namespace SecretSanta.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly IUserService _userService;

		public UserController(IUserService userService)
		{
			_userService = userService ?? throw new ArgumentNullException(nameof(userService));
		}

		// GET api/User/5
		[HttpGet("{userId}")]
		public ActionResult<User> GetUser(int userId)
		{
			if (userId <= 0) return BadRequest("userId invalid.");

			Domain.Models.User modelUser = _userService.GetUser(userId);

			if (modelUser is null)
				return NotFound("User does not exist.");
			return new User(modelUser);
		}

		// POST api/User
		[HttpPost]
		public ActionResult<User> AddUser(User user)
		{
			if (user is null) return BadRequest("user is null.");
			if (user.Id <= 0) return BadRequest("user.Id invalid.");

			return Created(
				$"api/User/{user.Id}",
				new User(_userService.AddUser(DTO.User.ToDomain(user)))
			);
		}

		// PUT
		[HttpPut]
		public ActionResult<User> UpdateUser(User user)
		{
			if (user == null) return BadRequest("user is null.");
			if (user.Id <= 0) return BadRequest("user.Id invalid.");

			return new User(_userService.UpdateUser(DTO.User.ToDomain(user)));
		}

		// DELETE
		[HttpDelete]
		public ActionResult RemoveUser(User user)
		{
			if (user is null) return BadRequest("user is null.");
			if (user.Id <= 0) return BadRequest("user.Id invalid.");

			_userService.RemoveUser(DTO.User.ToDomain(user));
			return NoContent(); // 204 status code: success but no content
		}
	}
}