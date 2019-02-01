using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Api.DTO;
using SecretSanta.Domain.Services;

namespace SecretSanta.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class GroupController : ControllerBase
	{
		private readonly IGroupService _groupService;
		private readonly IGroupUserService _groupUserService;

		public GroupController(IGroupService groupService, IGroupUserService groupUserService)
		{
			_groupService = groupService ?? throw new ArgumentNullException(nameof(groupService));
			_groupUserService = groupUserService ?? throw new ArgumentNullException(nameof(groupUserService));
		}

		// GET api/Group/5
		[HttpGet("{groupId}")]
		public ActionResult<Group> GetGroup(int groupId)
		{
			if (groupId <= 0) return BadRequest("groupId invalid.");

			Domain.Models.Group modelGroup = _groupService.GetGroup(groupId);

			if (modelGroup is null)
				return NotFound("Group does not exist.");
			return new Group(modelGroup);
		}

		// POST api/Group
		[HttpPost("")]
		public ActionResult<Group> AddGroup([FromBody]Group group)
		{
			if (group is null) return BadRequest("Group is null.");
			if (group.Id <= 0) return BadRequest("Group.Id invalid.");

			return Created(
				$"api/Group/{group.Id}",
				new Group(_groupService.AddGroup(DTO.Group.ToDomain(group)))
			);
		}

		// PUT
		[HttpPut]
		public ActionResult<Group> UpdateGroup(Group group)
		{
			if (group == null) return BadRequest("Group is null.");
			if (group.Id <= 0) return BadRequest("Group.Id invalid.");

			return new Group(_groupService.UpdateGroup(DTO.Group.ToDomain(group)));
		}

		// DELETE
		[HttpDelete]
		public ActionResult RemoveGroup(Group group)
		{
			if (group is null) return BadRequest("Group is null.");
			if (group.Id <= 0) return BadRequest("Group.Id invalid.");

			_groupService.RemoveGroup(DTO.Group.ToDomain(group));
			return NoContent(); // 204 status code: success but no content
		}

		// GET ALL GROUPS
		[HttpGet]
		public ActionResult<List<Group>> GetAllGroups()
		{
			List<Group> groups =_groupService.GetAllGroups()
				.Select(g => new Group(g))
				.ToList();
			return groups;
		}

		// ADD USER FROM GROUP
		[HttpPost("{groupId}")]
		public ActionResult AddUserToGroup(int groupId, User user)
		{
			if (groupId <= 0) return BadRequest("groupId is invalid.");
			if (user is null) return BadRequest("user is null.");
			if (user.Id <= 0) return BadRequest("user.Id invalid.");

			Domain.Models.Group modelGroup = _groupService.GetGroup(groupId);
			Domain.Models.User modelUser = DTO.User.ToDomain(user);

			Domain.Models.GroupUser groupUser = (new Domain.Models.GroupUser()
			{
				Group = modelGroup,
				GroupId = modelGroup.Id,
				User = modelUser,
				UserId = modelUser.Id
			});

			_groupUserService.AddGroupUser(groupUser);
			return Ok();
		}

		// REMOVE USER FROM GROUP
		[HttpDelete("{groupId}")]
		public ActionResult RemoveUserFromGroup(int groupId, User user)
		{
			if (groupId <= 0) return BadRequest("Group.Id invalid.");
			if (user is null) return BadRequest("user is null.");
			if (user.Id <= 0) return BadRequest("user.Id invalid.");

			Domain.Models.Group modelGroup = _groupService.GetGroup(groupId);
			Domain.Models.User modelUser = DTO.User.ToDomain(user);

			Domain.Models.GroupUser groupUser = (new Domain.Models.GroupUser()
			{
				Group = modelGroup,
				GroupId = modelGroup.Id,
				User = modelUser,
				UserId = modelUser.Id
			});

			_groupUserService.RemoveGroupUser(groupUser);
			return Ok();
		}

		// GET ALL USERS IN GROUP
		[HttpGet("{groupId}/users")]
		public ActionResult<List<User>> GetAllUsersInGroup(int groupId)
		{
			if (groupId <= 0) return BadRequest("groupId is invalid.");

			List<User> users = _groupUserService.GetAllUsersInGroup(groupId)
				.Select(u => new User(u))
				.ToList();
			return users;
		}
	}
}