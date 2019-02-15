using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services.Interfaces;

namespace SecretSanta.Domain.Services
{
	public class PairingService : IPairingService
	{
		private ApplicationDbContext DbContext { get; }
		private SemaphoreSlim mutex = new SemaphoreSlim(0, 1);
		public PairingService(ApplicationDbContext dbContext)
		{
			DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
		}

		public async Task<List<Pairing>> GetPairings(int groupId)
		{
			if (groupId <= 0)
				throw new ArgumentOutOfRangeException(nameof(groupId));

			await mutex.WaitAsync();
			try
			{
				Group group = await DbContext.FindAsync<Group>(groupId);
				List<int> userIds = group.GroupUsers.Select(gu => gu.UserId).ToList();
				return await DbContext.Pairings.Where(p => userIds.Contains(p.RecipientId) || userIds.Contains(p.SantaId))
					.ToListAsync();
			}
			finally
			{
				mutex.Release();
			}
		}

		public async Task GeneratePairings(int groupId)
		{
			if (groupId <= 0)
				throw new ArgumentOutOfRangeException(nameof(groupId));

			await mutex.WaitAsync();
			try
			{
				await Task.Run(async () =>
				{
					Group group = await DbContext.FindAsync<Group>(groupId);
					List<int> userIds = group.GroupUsers.Select(gu => gu.UserId).ToList();
					List<User> users = await DbContext.Users.Where(u => userIds.Contains(u.Id))
						.ToListAsync();

					List<int> duplicateUserIds = new List<int>(userIds);
					FisherYatesShuffle(duplicateUserIds);
					while (!CheckForValidPairings(userIds, duplicateUserIds))
						FisherYatesShuffle(duplicateUserIds);

					List<Pairing> pairings = new List<Pairing>();
					for (int i = 0; i < duplicateUserIds.Count; i++)
					{
						pairings.Add(new Pairing
						{
							Recipient = users[duplicateUserIds[i]],
							RecipientId = users[duplicateUserIds[i]].Id,
							Santa = users[i],
							SantaId = users[i].Id
						});
					}

					DbContext.Pairings.AddRange(pairings);
					await DbContext.SaveChangesAsync();
				});
			}
			finally
			{
				mutex.Release();
			}
		}

		// Implementation of Fisher-Yates shuffle is not my original idea.
		public void FisherYatesShuffle(List<int> list)
		{
			int n = list.Count;
			Random rng = new Random();
			while (n > 1)
			{
				n--;
				int k = rng.Next(n + 1);
				int value = list[k];
				list[k] = list[n];
				list[n] = value;
			}
		}

		public bool CheckForValidPairings(List<int> A, List<int> B)
		{
			if (A is null) throw new ArgumentNullException(nameof(A));
			if (B == null) throw new ArgumentNullException(nameof(B));
			if (A.Count != B.Count) return false;

			for (int i = 0; i < A.Count; i++)
				if (A[i] == B[i]) return false;

			return true;
		}
	}
}
