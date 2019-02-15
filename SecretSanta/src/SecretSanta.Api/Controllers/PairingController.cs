using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using SecretSanta.Api.ViewModels;

namespace SecretSanta.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PairingController : ControllerBase
	{
		private IPairingService PairingService { get; }
		private IMapper Mapper { get; }

		public PairingController(IPairingService pairingService, IMapper mapper)
		{
			PairingService = pairingService;
			Mapper = mapper;
		}

		[HttpGet("{groupId}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesDefaultResponseType]
		public IActionResult GetPairingsForGroup(int groupId)
		{
			if (groupId <= 0)
			{
				return NotFound();
			}

			return Ok(PairingService.GetPairings(groupId).Result.Select(p => Mapper.Map<PairingViewModel>(p)));
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesDefaultResponseType]
		public IActionResult GeneratePairingsForGroup(int groupId)
		{
			if (groupId <= 0)
			{
				return NotFound();
			}

			return Ok(PairingService.GeneratePairings(groupId));
		}
	}
}
