using System;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Api.ViewModels;
using SecretSanta.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using SecretSanta.Domain.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SecretSanta.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiConventionType(typeof(DefaultApiConventions))]
	[ApiController]
    public class UserController : ControllerBase
    {
        private IUserService UserService { get; }
        private IMapper Mapper { get; }


		public UserController(IUserService userService, IMapper mapper)
        {
            UserService = userService ?? throw new ArgumentNullException(nameof(userService));
            Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		}

		// POST api/<controller>
		[HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
		[Produces(typeof(UserViewModel))]
        public IActionResult Post(UserInputViewModel userViewModel)
        {
            if (userViewModel == null)
                return BadRequest();
            if (ModelState.IsValid && userViewModel.FirstName != null)
            {
		        var persistedUser = UserService.AddUser(Mapper.Map<User>(userViewModel));
	            return Ok(Mapper.Map<UserViewModel>(persistedUser));
            }
            return BadRequest("First name is required");
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
		[Produces(typeof(UserViewModel))]
        public IActionResult Put(int id, UserInputViewModel userViewModel)
        {
            if (userViewModel == null)
            {
                return BadRequest();
            }
            if (ModelState.IsValid && userViewModel.FirstName != null)
            {
				var foundUser = UserService.Find(id);
	            if (foundUser == null)
	                return NotFound();

	            foundUser.FirstName = userViewModel.FirstName;
	            foundUser.LastName = userViewModel.LastName;

	            var persistedUser = UserService.UpdateUser(foundUser);
	            return Ok(Mapper.Map<UserViewModel>(persistedUser));
            }
	        return BadRequest("First name is required.");

        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public IActionResult Delete(int id)
        {
            bool userWasDeleted = UserService.DeleteUser(id);
            return userWasDeleted ? (IActionResult)Ok() : (IActionResult)NotFound();
        }
    }
}
