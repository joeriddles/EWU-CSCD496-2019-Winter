using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Api.ViewModels;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SecretSanta.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiConventionType(typeof(DefaultApiConventions))]
	[ApiController]
    public class GiftController : ControllerBase
    {
        private IGiftService GiftService { get; }
		private IMapper Mapper { get; }

        public GiftController(IGiftService giftService, IMapper mapper)
        {
            GiftService = giftService ?? throw new ArgumentNullException(nameof(giftService));
            Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

		// GET api/Gift/5
		[HttpGet("{userId}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesDefaultResponseType]
		[Produces(typeof(List<GroupViewModel>))]
		public IActionResult GetGiftForUser(int userId)
        {
            if (userId <= 0)
            {
                return NotFound();
            }
            List<Gift> databaseUsers = GiftService.GetGiftsForUser(userId);

            return Ok(Mapper.Map<List<GiftViewModel>>(databaseUsers));
        }
    }
}
