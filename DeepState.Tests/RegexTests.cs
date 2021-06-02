using Discord.WebSocket;
using Moq;
using NUnit.Framework;
using Utils = DeepState.Utilities.Utilities;

namespace DeepState.Tests
{
	public class RegexTests
	{
		[Test]
		[TestCase("sus", true)]
		[TestCase("sus.", true)]
		[TestCase("sussy", true)]
		[TestCase("suspect", false)]
		[TestCase("suspense", false)]
		[TestCase("among us", true)]
		[TestCase("amogus", true)]
		[TestCase("consensus", false)]
		[TestCase("When the is sus", true)]
		[TestCase("There's a lobster among us, yanno?", true)]
		public void IsSusCheck(string stringToCheck, bool expectSus)
		{
			Assert.AreEqual(Utils.IsSus(stringToCheck), expectSus);
		}
	}
}