namespace SecretSanta.Domain.Models
{
	public abstract class Counter
	{
		protected static int IdCounter { get; set; }
		public int Id { get; set; }

		// This implementation does not have separate counters for different derived classes.
		// TODO: Fix above.
		protected Counter()
		{
			IdCounter++;
			Id = IdCounter;
		}

		public static void ResetCounter()
		{
			IdCounter = 0;
		}
	}
}
