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
		[ClassInitialize]
		public static void ClassInitialize(TestContext testContext)
		{
			//should always cleanup at start and at__tempt cleanup at the end
			if (File.Exists("__temp.txt"))
				File.Delete("__temp.txt");
			if (File.Exists("__temp2.txt"))
				File.Delete("__temp2.txt");

			File.WriteAllLines("__temp.txt", new[]{ "Name: John Smith", "Racecar", "Xbox" }, Encoding.UTF8);
			File.WriteAllLines("__temp2.txt", new []{"Name: Smith, John", "Racecar", "Xbox" }, Encoding.UTF8);
		}

		[ClassCleanup]
		public static void ClassCleanup()
		{
			if (File.Exists("__temp.txt"))
				File.Delete("__temp.txt");
			if (File.Exists("__temp2.txt"))
				File.Delete("__temp2.txt");
		}

		[TestInitialize]
		public void TestInitialize()
		{
			if (File.Exists("__empty.txt"))
				File.Delete("__empty.txt");
		}

		[TestCleanup]
		public void TestCleanup()
		{
			if (File.Exists("__empty.txt"))
				File.Delete("__empty.txt");
		}

		[TestMethod]
		public void InitializeAndCleanupAreWorking()
		{
			Assert.IsFalse(File.Exists("__empty.txt"));
			Assert.IsTrue(File.Exists("__temp.txt"));
			Assert.IsTrue(File.Exists("__temp2.txt"));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Import_filenameIsNull_ThrowsException()
		{
			Import import = new Import(null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void Import_fileDoesNotExist_ThrowsException()
		{
			Import import = new Import("__thisfiledontexist.fake");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void Import_fileIsEmpty_ThrowsException()
		{
			File.WriteAllLines("__empty.txt", new List<string>());
			Import import = new Import("__empty.txt");
		}

		[TestMethod]
		public void Import_fileExists_Success()
		{
			Import import = new Import("__temp.txt");
			Assert.AreEqual("__temp.txt", import.Filename);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ReadHead_FormattedIncorrectly_ThrowsException()
		{
			File.WriteAllLines("__empty.txt", new List<string>(){"No names here"});
			Import import = new Import("__empty.txt");
			import.ReadHeader();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ReadHead_NameFormattedIncorrectly_ThrowsException()
		{
			File.WriteAllLines("__empty.txt", new List<string>() { "NotName: John Smith" });
			Import import = new Import("__empty.txt");
			import.ReadHeader();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ReadHead_NameIsEmpty_ThrowsExcption()
		{
			File.WriteAllLines("__empty.txt", new List<string>() { "NotName: " });
			Import import = new Import("__empty.txt");
			import.ReadHeader();
		}

		[TestMethod]
		public void ReadHead_FirstNameLastName_Success()
		{
			Import import = new Import("__temp.txt");
			User user = import.ReadHeader();
			Assert.AreEqual("John", user.FirstName);
			Assert.AreEqual("Smith", user.LastName);
		}

		[TestMethod]
		public void ReadHead_LastName_FirstName_Success()
		{
			Import import = new Import("__temp2.txt");
			User user = import.ReadHeader();
			Assert.AreEqual("John", user.FirstName);
			Assert.AreEqual("Smith", user.LastName);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ReadBody_NullUser_ThrowsException()
		{
			Import import = new Import("__temp.txt");
			import.ReadBody(null);
		}

		[TestMethod]
		public void ReadBody_NoGifts_EmptyList()
		{
			File.WriteAllLines("__empty.txt", new List<string>() { "Name: John Smith" });
			Import import = new Import("__empty.txt");
			User user = import.ReadHeader();
			List<Gift> gifts = import.ReadBody(user);
			Assert.IsNotNull(gifts);
			Assert.AreEqual(0, gifts.Count);
		}

		[TestMethod]
		public void ReadBody_HasGifts_Success()
		{
			Import import = new Import("__temp.txt");
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
			File.WriteAllLines("__temp.txt", new []{"Name: John Smith", "", "Racecar", "", "Xbox", ""});
			Import import = new Import("__temp.txt");
			User user = import.ReadHeader();
			List<Gift> gifts = import.ReadBody(user);
			Assert.AreEqual(2, gifts.Count);
			Assert.AreEqual("Racecar", gifts[0].Title);
			Assert.AreEqual("Xbox", gifts[1].Title);
			Assert.AreEqual(user.Id, gifts[0].UserId);
		}
	}
}
