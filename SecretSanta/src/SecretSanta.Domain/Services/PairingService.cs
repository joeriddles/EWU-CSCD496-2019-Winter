using System.Collections.Generic;
using System.Linq;
using SecretSanta.Domain.Models;

namespace SecretSanta.Domain.Services
{
	public class PairingService
	{
		private ApplicationDbContext DbContext { get; }

		public PairingService(ApplicationDbContext context)
		{
			DbContext = context;
		}

		//CRUD: Create, Read, Update, Delete

		public void AddPairing(Pairing pairing)
		{
			DbContext.Pairings.Add(pairing);
			DbContext.SaveChanges();
		}

		public Pairing GetPairingById(int pairingId)
		{
			return DbContext.Pairings.Find(pairingId);
		}

		public List<Pairing> GetAllPairings()
		{
			return DbContext.Set<Pairing>().ToList();
		}

		public void UpdatePairing(Pairing pairing)
		{
			DbContext.Pairings.Update(pairing);
			DbContext.SaveChanges();
		}

		public int GetPairingCount()
		{
			return DbContext.Pairings.Count();
		}
	}
}
