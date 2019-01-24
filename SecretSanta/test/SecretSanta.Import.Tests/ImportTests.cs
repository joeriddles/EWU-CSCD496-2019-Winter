using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Domain.Models;

namespace SecretSanta.Import.Tests
{
	[TestClass]
	public class ImportTests
	{
		private static readonly string TempDirectory = Path.GetTempPath();

		[ClassInitialize]
		public static void ClassInitialize(TestContext testContext)
		{
			//should always cleanup at start and at__tempt cleanup at the end
			if (File.Exists($"{TempDirectory}/temp.txt"))
				File.Delete($"{TempDirectory}/temp.txt");
			if (File.Exists($"{TempDirectory}/temp2.txt"))
				File.Delete($"{TempDirectory}/temp2.txt");

			File.WriteAllLines($"{TempDirectory}/temp.txt", new[]{ "Name: John Smith", "Racecar", "Xbox" }, Encoding.UTF8);
			File.WriteAllLines($"{TempDirectory}/temp2.txt", new []{"Name: Smith, John", "Racecar", "Xbox" }, Encoding.UTF8);
		}

		[ClassCleanup]
		public static void ClassCleanup()
		{
			if (File.Exists($"{TempDirectory}/temp.txt"))
				File.Delete($"{TempDirectory}/temp.txt");
			if (File.Exists($"{TempDirectory}/temp2.txt"))
				File.Delete($"{TempDirectory}/temp2.txt");
		}

		[TestInitialize]
		public void TestInitialize()
		{
			if (File.Exists($"{TempDirectory}/temp3.txt"))
				File.Delete($"{TempDirectory}/temp3.txt");
		}

		[TestCleanup]
		public void TestCleanup()
		{
			if (File.Exists($"{TempDirectory}/temp3.txt"))
				File.Delete($"{TempDirectory}/temp3.txt");
		}

		[TestMethod]
		public void InitializeAndCleanupAreWorking()
		{
			Assert.IsFalse(File.Exists($"{TempDirectory}/temp3.txt"));
			Assert.IsTrue(File.Exists($"{TempDirectory}/temp.txt"));
			Assert.IsTrue(File.Exists($"{TempDirectory}/temp2.txt"));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Import_filenameIsNull_ThrowsArgumentNullException()
		{
			Import import = new Import(null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void Import_fileDoesNotExist_ThrowsArgumentException()
		{
			Import import = new Import($"{TempDirectory}/thisfiledontexist.fake");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void Import_fileIsEmpty_ThrowsArgumentException()
		{
			File.WriteAllLines($"{TempDirectory}/temp3.txt", new List<string>());
			Import import = new Import($"{TempDirectory}/temp3.txt");
		}

		[TestMethod]
		public void Import_fileExists_Success()
		{
			Import import = new Import($"{TempDirectory}/temp.txt");
			Assert.AreEqual($"{TempDirectory}/temp.txt", import.Filename);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ReadHead_FormattedIncorrectly_ThrowsArgumentException()
		{
			File.WriteAllLines($"{TempDirectory}/temp3.txt", new List<string>(){"No names here"});
			Import import = new Import($"{TempDirectory}/temp3.txt");
			import.ReadHeader();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ReadHead_NameFormattedIncorrectly_ThrowsArgumentException()
		{
			File.WriteAllLines($"{TempDirectory}/temp3.txt", new List<string>() { "NotName: John Smith" });
			Import import = new Import($"{TempDirectory}/temp3.txt");
			import.ReadHeader();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ReadHead_NameIsEmpty_ThrowsArgumentException()
		{
			File.WriteAllLines($"{TempDirectory}/temp3.txt", new List<string>() { "NotName: " });
			Import import = new Import($"{TempDirectory}/temp3.txt");
			import.ReadHeader();
		}

		[TestMethod]
		public void ReadHead_FirstNameLastName_Success()
		{
			Import import = new Import($"{TempDirectory}/temp.txt");
			User user = import.ReadHeader();
			Assert.AreEqual("John", user.FirstName);
			Assert.AreEqual("Smith", user.LastName);
		}

		[TestMethod]
		public void ReadHead_LastName_FirstName_Success()
		{
			Import import = new Import($"{TempDirectory}/temp2.txt");
			User user = import.ReadHeader();
			Assert.AreEqual("John", user.FirstName);
			Assert.AreEqual("Smith", user.LastName);
		}

		[TestMethod]
		[DataRow("Name:   John Smith")]
		[DataRow("Name: John   Smith")]
		[DataRow("Name: John Smith   ")]
		[DataRow("Name:   John   Smith   ")]
		public void ReadHead_Spaces_FirstName_LastName_Success(string fileHeader)
		{
			File.WriteAllLines($"{TempDirectory}/temp3.txt", new List<string>() { fileHeader });
			Import import = new Import($"{TempDirectory}/temp3.txt");
			User user = import.ReadHeader();
			Assert.AreEqual("John", user.FirstName);
			Assert.AreEqual("Smith", user.LastName);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ReadBody_NullUser_ThrowsException()
		{
			Import import = new Import($"{TempDirectory}/temp.txt");
			import.ReadBody(null);
		}

		[TestMethod]
		public void ReadBody_NoGifts_EmptyList()
		{
			File.WriteAllLines($"{TempDirectory}/temp3.txt", new List<string>() { "Name: John Smith" });
			Import import = new Import($"{TempDirectory}/temp3.txt");
			User user = import.ReadHeader();
			List<Gift> gifts = import.ReadBody(user);
			Assert.IsNotNull(gifts);
			Assert.AreEqual(0, gifts.Count);
		}

		[TestMethod]
		public void ReadBody_HasGifts_Success()
		{
			Import import = new Import($"{TempDirectory}/temp.txt");
			User user = import.ReadHeader();
			List<Gift> gifts = import.ReadBody(user);
			Assert.AreEqual(2, gifts.Count);
			Assert.AreEqual("Racecar", gifts[0].Title);
			Assert.AreEqual("Xbox", gifts[1].Title);
			Assert.AreEqual(user.Id, gifts[0].UserId);
		}

		[TestMethod]
		public void ReadBody_HasGiftsAndBlankLines_Success()
		{
			File.WriteAllLines($"{TempDirectory}/temp.txt", new []{"Name: John Smith", "", "Racecar", "", "Xbox", ""});
			Import import = new Import($"{TempDirectory}/temp.txt");
			User user = import.ReadHeader();
			List<Gift> gifts = import.ReadBody(user);
			Assert.AreEqual(2, gifts.Count);
			Assert.AreEqual("Racecar", gifts[0].Title);
			Assert.AreEqual("Xbox", gifts[1].Title);
			Assert.AreEqual(user.Id, gifts[0].UserId);
		}
	}
}
