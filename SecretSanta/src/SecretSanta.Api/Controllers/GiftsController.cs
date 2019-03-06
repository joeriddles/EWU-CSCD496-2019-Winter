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
    public class GiftsController : ControllerBase
    {
	    private IGiftService GiftService { get; }
        private IMapper Mapper { get; }
	    private ILogger Logger { get; }

        public GiftsController(IGiftService giftService, IMapper mapper, ILoggerFactory loggerFactory)
        {
            GiftService = giftService;
            Mapper = mapper;
            Logger = loggerFactory.CreateLogger("GiftsController");
        }

        [HttpGet]
        public async Task<ActionResult<GiftViewModel>> GetGift(int id)
        {
			Logger.Log(LogLevel.Information, $"GetGift({id}) called");
            var gift = await GiftService.GetGift(id);

            if (gift == null)
            {
				Logger.Log(LogLevel.Error, $"GetGift({id}) does not exist");
                return NotFound();
            }

			Logger.Log(LogLevel.Information, $"GetGift({id}) gift returned");
            return Ok(Mapper.Map<GiftViewModel>(gift));
        }

        [HttpPost]
        public async Task<ActionResult<GiftViewModel>> CreateGift(GiftInputViewModel viewModel)
        {
	        Logger.Log(LogLevel.Information, $"CreateGift() called");
			var createdGift = await GiftService.AddGift(Mapper.Map<Gift>(viewModel));

			return CreatedAtAction(nameof(GetGift), new { id = createdGift.Id }, Mapper.Map<GiftViewModel>(createdGift));
        }

        // GET api/Gift/5
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<ICollection<GiftViewModel>>> GetGiftsForUser(int userId)
        {
	        Logger.Log(LogLevel.Information, $"GetGiftsForUser({userId}) called");

			if (userId <= 0)
            {
	            Logger.Log(LogLevel.Error, $"GetGiftsForUser({userId}) does not exist");
				return NotFound();
            }
            List<Gift> databaseUsers = await GiftService.GetGiftsForUser(userId);

            Logger.Log(LogLevel.Information, $"GetGiftsForUser({userId}) gifts returned");
			return Ok(databaseUsers.Select(x => Mapper.Map<GiftViewModel>(x)).ToList());
        }
    }
}
