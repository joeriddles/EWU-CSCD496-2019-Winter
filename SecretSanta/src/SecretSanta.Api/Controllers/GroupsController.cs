using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SecretSanta.Api.ViewModels;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SecretSanta.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private IGroupService GroupService { get; }
        private IMapper Mapper { get; }
        private ILogger Logger { get; }

		public GroupsController(IGroupService groupService, IMapper mapper, ILoggerFactory loggerFactory)
        {
            GroupService = groupService;
            Mapper = mapper;
            Logger = loggerFactory.CreateLogger("GiftsController");
		}

		// GET api/group
		[HttpGet]
        public async Task<ActionResult<ICollection<GroupViewModel>>> GetGroups()
        {
			Logger.LogInformation("GetGroups(): called");
            var groups = await GroupService.FetchAll();
            Logger.LogInformation("GetGroups(): groups returned");
			return Ok(groups.Select(x => Mapper.Map<GroupViewModel>(x)));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GroupViewModel>> GetGroup(int id)
        {
	        Logger.LogInformation($"GetGroup({id}): called");

			var group = await GroupService.GetById(id);
            if (group == null)
            {
				Logger.LogInformation($"GetGroup({id}): group does not exist");
                return NotFound();
            }

            Logger.LogInformation($"GetGroups({id}): groups returned");
			return Ok(Mapper.Map<GroupViewModel>(group));
        }

        // POST api/group
        [HttpPost]
        public async Task<ActionResult<GroupViewModel>> CreateGroup(GroupInputViewModel viewModel)
        {
	        Logger.LogInformation("CreateGroup(): called");

			if (viewModel == null)
            {
	            Logger.LogError("CreateGroup(): viewModel is null");
				return BadRequest();
            }
            var createdGroup = await GroupService.AddGroup(Mapper.Map<Group>(viewModel));
            Logger.LogInformation($"CreateGroup({createdGroup.Id}): group created");
			return CreatedAtAction(nameof(GetGroup), new { id = createdGroup.Id}, Mapper.Map<GroupViewModel>(createdGroup));
        }

        // PUT api/group/5
        [HttpPut]
        public async Task<ActionResult> UpdateGroup(int id, GroupInputViewModel viewModel)
        {
	        Logger.LogInformation($"UpdateGroup({id}): called");

			if (viewModel == null)
            {
	            Logger.LogError($"UpdateGroup({id}): viewModel is null");
				return BadRequest();
            }
            var group = await GroupService.GetById(id);
            if (group == null)
            {
	            Logger.LogInformation($"UpdateGroup({id}): group does not exist");
				return NotFound();
            }

            Mapper.Map(viewModel, group);
            await GroupService.UpdateGroup(group);

            Logger.LogInformation($"UpdateGroup({id}): group updated");
			return NoContent();
        }

        // DELETE api/group/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteGroup(int id)
        {
			Logger.LogInformation($"DeleteGroup({id}): called");

            if (id <= 0)
            {
				Logger.LogError($"DeleteGroup({id}): invalid id");
                return BadRequest("A group id must be specified");
            }

            if (await GroupService.DeleteGroup(id))
            {
				Logger.LogInformation($"DeleteGroup({id}): group deleted");
                return Ok();
            }
			Logger.LogInformation($"DeleteGroup({id}): group does not exist");
            return NotFound();
        }
    }
}
