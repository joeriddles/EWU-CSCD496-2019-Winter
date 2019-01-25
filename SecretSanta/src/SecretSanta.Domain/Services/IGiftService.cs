using System.Collections.Generic;
using SecretSanta.Domain.Models;

namespace SecretSanta.Domain.Services
{
	// See: IRepository pattern
	public interface IGiftService
	{
		List<Gift> GetGiftsForUser(int userId);
	}
}