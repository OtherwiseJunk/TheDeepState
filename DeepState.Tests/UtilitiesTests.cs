using DeepState.Constants;
using DeepState.Utilities;
using NUnit.Framework;
using System.Text.RegularExpressions;
using Utils = DeepState.Utilities.Utilities;

namespace DeepState.Tests
{
    public class UtilitiesTests
    {
        #region Regex checks
        [Test]
        [TestCase("sus", true)]
        [TestCase("sssssssuuuuuuuuuuuuuuuuuuuuussssssss", true)]
        [TestCase("sus.", true)]
        [TestCase("sussy", true)]
        [TestCase("suspect", false)]
        [TestCase("suspense", false)]
        [TestCase("among us", true)]
        [TestCase("amogus", true)]
        [TestCase("consensus", false)]
        [TestCase("When the is sus", true)]
        [TestCase("There's a lobster among us, yanno?", true)]
        [TestCase("famous", false)]
        [TestCase("polyamorous", false)]
        public void IsSusCheck(string stringToCheck, bool expectSus)
        {
            Assert.AreEqual(Utils.IsSus(stringToCheck), expectSus);
        }

        [Test]
        [TestCase("https://twitter.com/TolarianCollege/status/1583098494579265536", true)]
        [TestCase("http://twitter.com/TolarianCollege/status/1583098494579265536", true)]
        [TestCase("https://www.twitter.com/TolarianCollege/status/1583098494579265536.", true)]
        [TestCase("http://www.twitter.com/TolarianCollege/status/1583098494579265536", true)]
        [TestCase("@[CSB Enthusiast] reddy https://twitter.com/taylorswift13/status/1583083964931055616?s=46&t=4QkqtgX4-lvArl3IQbJukw", true)]
        [TestCase("Some message that mentions www.twitter.com", false)]
        [TestCase("https://cdn.discordapp.com/attachments/701194133074608198/1032671502753075240/IMG_9060.jpg", false)]
        [TestCase("Some message with a link inside of it https://cdn.discordapp.com/attachments/701194133074608198/1032671502753075240/IMG_9060.jpg", false)]
        [TestCase("https://c.vxtwitter.com/TolarianCollege/status/1583098494579265536", false)]
        [TestCase("http://c.vxtwitter.com/TolarianCollege/status/1583098494579265536", false)]
        [TestCase("https://www.c.vxtwitter.com/TolarianCollege/status/1583098494579265536.", false)]
        [TestCase("http://www.c.vxtwitter.com/TolarianCollege/status/1583098494579265536", false)]
        public void ContainsTwitterLink(string stringToCheck, bool expectedResult)
        {
            Assert.AreEqual(Utils.ContainsTwitterLink(stringToCheck), expectedResult);
        }

        [Test]
        [TestCase("https://twitter.com/TolarianCollege/status/1583098494579265536", true)]
        [TestCase("http://twitter.com/TolarianCollege/status/1583098494579265536", true)]
        [TestCase("https://www.twitter.com/TolarianCollege/status/1583098494579265536", true)]
        [TestCase("http://www.twitter.com/TolarianCollege/status/1583098494579265536", true)]
        [TestCase("https://twitter.com/TolarianCollege/status/1583098494579265536?s=21", true)]
        [TestCase("http://twitter.com/TolarianCollege/status/1583098494579265536?s=21", true)]
        [TestCase("https://www.twitter.com/TolarianCollege/status/1583098494579265536?s=21", true)]
        [TestCase("http://www.twitter.com/TolarianCollege/status/1583098494579265536?s=21", true)]
        [TestCase("https://www.twitter.com/TolarianCollege/status/1583098494579265536 dsad", false)]
        [TestCase("http://www.twitter.com/TolarianCollege/status/1583098494579265536 dsad", false)]
        [TestCase("https://twitter.com/TolarianCollege/status/1583098494579265536?s=21 dsad", false)]
        [TestCase("http://twitter.com/TolarianCollege/status/1583098494579265536?s=21 dsad", false)]
        [TestCase("https://c.vxtwitter.com/TolarianCollege/status/1583098494579265536", true)]
        [TestCase("http://c.vxtwitter.com/TolarianCollege/status/1583098494579265536", true)]
        [TestCase("https://c.vxtwitter.com/TolarianCollege/status/1583098494579265536?s=21", true)]
        [TestCase("http://c.vxtwitter.com/TolarianCollege/status/1583098494579265536?s=21", true)]
        [TestCase("http://c.vxtwitter.com/TolarianCollege/status/1583098494579265536 dsad", false)]
        [TestCase("https://c.vxtwitter.com/TolarianCollege/status/1583098494579265536?s=21 dsad", false)]
        [TestCase("https://fxtwitter.com/TolarianCollege/status/1583098494579265536", true)]
        [TestCase("http://fxtwitter.com/TolarianCollege/status/1583098494579265536", true)]
        [TestCase("https://fxtwitter.com/TolarianCollege/status/1583098494579265536?s=21", true)]
        [TestCase("http://fxtwitter.com/TolarianCollege/status/1583098494579265536?s=21", true)]
        [TestCase("http://fxtwitter.com/TolarianCollege/status/1583098494579265536 dsad", false)]
        [TestCase("https://fxtwitter.com/TolarianCollege/status/1583098494579265536?s=21 dsad", false)]
        [TestCase("https://vxtwitter.com/TolarianCollege/status/1583098494579265536", true)]
        [TestCase("http://vxtwitter.com/TolarianCollege/status/1583098494579265536", true)]
        [TestCase("https://vxtwitter.com/TolarianCollege/status/1583098494579265536?s=21", true)]
        [TestCase("http://vxtwitter.com/TolarianCollege/status/1583098494579265536?s=21", true)]
        [TestCase("http://vxtwitter.com/TolarianCollege/status/1583098494579265536 dsad", false)]
        [TestCase("https://vxtwitter.com/TolarianCollege/status/1583098494579265536?s=21 dsad", false)]
        public void MessageExclusivelyContainsTweet(string input, bool expectedResult)
        {
            
        }

        [Test]
        [TestCase("https://twitter.com/realDonaldTrump/status/1583098494579265536", true)]
        [TestCase("http://twitter.com/realDonaldTrump/status/1583098494579265536", true)]
        [TestCase("https://www.twitter.com/realDonaldTrump/status/1583098494579265536", true)]
        [TestCase("http://www.twitter.com/realDonaldTrump/status/1583098494579265536", true)]
        [TestCase("https://twitter.com/realDonaldTrump/status/1583098494579265536?s=21", true)]
        [TestCase("http://twitter.com/realDonaldTrump/status/1583098494579265536?s=21", true)]
        [TestCase("https://www.twitter.com/realDonaldTrump/status/1583098494579265536?s=21", true)]
        [TestCase("http://www.twitter.com/realDonaldTrump/status/1583098494579265536?s=21", true)]
        [TestCase("https://www.twitter.com/realDonaldTrump/status/1583098494579265536 dsad", false)]
        [TestCase("http://www.twitter.com/realDonaldTrump/status/1583098494579265536 dsad", false)]
        [TestCase("https://twitter.com/realDonaldTrump/status/1583098494579265536?s=21 dsad", false)]
        [TestCase("http://twitter.com/realDonaldTrump/status/1583098494579265536?s=21 dsad", false)]
        [TestCase("https://c.vxtwitter.com/realDonaldTrump/status/1583098494579265536", true)]
        [TestCase("http://c.vxtwitter.com/realDonaldTrump/status/1583098494579265536", true)]
        [TestCase("https://c.vxtwitter.com/realDonaldTrump/status/1583098494579265536?s=21", true)]
        [TestCase("http://c.vxtwitter.com/realDonaldTrump/status/1583098494579265536?s=21", true)]
        [TestCase("http://c.vxtwitter.com/ReaLDonaldTrump/status/1583098494579265536 dsad", false)]
        [TestCase("https://c.vxtwitter.com/realDonaldTrump/status/1583098494579265536?s=21 dsad", false)]
        [TestCase("https://fxtwitter.com/realDonaldTrump/status/1583098494579265536", true)]
        [TestCase("http://fxtwitter.com/realDonaldTrump/status/1583098494579265536", true)]
        [TestCase("https://fxtwitter.com/realDonaldTrump/status/1583098494579265536?s=21", true)]
        [TestCase("http://fxtwitter.com/realDonaldTrump/status/1583098494579265536?s=21", true)]
        [TestCase("http://fxtwitter.com/realDonaldTrump/status/1583098494579265536 dsad", false)]
        [TestCase("https://fxtwitter.com/realDonaldTrump/status/1583098494579265536?s=21 dsad", false)]
        [TestCase("https://vxtwitter.com/realDonaldTrump/status/1583098494579265536", true)]
        [TestCase("http://vxtwitter.com/realDonaldTrump/status/1583098494579265536", true)]
        [TestCase("https://vxtwitter.com/realDonaldTrump/status/1583098494579265536?s=21", true)]
        [TestCase("http://vxtwitter.com/realDonaldTrump/status/1583098494579265536?s=21", true)]
        [TestCase("http://vxtwitter.com/realDonaldTrump/status/1583098494579265536 dsad", false)]
        [TestCase("https://vxtwitter.com/realDonaldTrump/status/1583098494579265536?s=21 dsad", false)]
        [TestCase("https://twitter.com/elonmusk/status/1583098494579265536", true)]
        [TestCase("http://twitter.com/elonmusk/status/1583098494579265536", true)]
        [TestCase("https://www.twitter.com/elonmusk/status/1583098494579265536", true)]
        [TestCase("http://www.twitter.com/elonmusk/status/1583098494579265536", true)]
        [TestCase("https://twitter.com/elonmusk/status/1583098494579265536?s=21", true)]
        [TestCase("http://twitter.com/elonmusk/status/1583098494579265536?s=21", true)]
        [TestCase("https://www.twitter.com/elonmusk/status/1583098494579265536?s=21", true)]
        [TestCase("http://www.twitter.com/elonmusk/status/1583098494579265536?s=21", true)]
        [TestCase("https://www.twitter.com/elonmusk/status/1583098494579265536 dsad", false)]
        [TestCase("http://www.twitter.com/elonmusk/status/1583098494579265536 dsad", false)]
        [TestCase("https://twitter.com/elonmusk/status/1583098494579265536?s=21 dsad", false)]
        [TestCase("http://twitter.com/elonmusk/status/1583098494579265536?s=21 dsad", false)]
        [TestCase("https://c.vxtwitter.com/elonmusk/status/1583098494579265536", true)]
        [TestCase("http://c.vxtwitter.com/elonmusk/status/1583098494579265536", true)]
        [TestCase("https://c.vxtwitter.com/EloNmuSk/status/1583098494579265536?s=21", true)]
        [TestCase("http://c.vxtwitter.com/elonmusk/status/1583098494579265536?s=21", true)]
        [TestCase("http://c.vxtwitter.com/elonmusk/status/1583098494579265536 dsad", false)]
        [TestCase("https://c.vxtwitter.com/elonmusk/status/1583098494579265536?s=21 dsad", false)]
        [TestCase("https://fxtwitter.com/elonmusk/status/1583098494579265536", true)]
        [TestCase("http://fxtwitter.com/elonmusk/status/1583098494579265536", true)]
        [TestCase("https://fxtwitter.com/elonmusk/status/1583098494579265536?s=21", true)]
        [TestCase("http://fxtwitter.com/elonmusk/status/1583098494579265536?s=21", true)]
        [TestCase("http://fxtwitter.com/elonmusk/status/1583098494579265536 dsad", false)]
        [TestCase("https://fxtwitter.com/elonmusk/status/1583098494579265536?s=21 dsad", false)]
        [TestCase("https://vxtwitter.com/elonmusk/status/1583098494579265536", true)]
        [TestCase("http://vxtwitter.com/elonmusk/status/1583098494579265536", true)]
        [TestCase("https://vxtwitter.com/elonmusk/status/1583098494579265536?s=21", true)]
        [TestCase("http://vxtwitter.com/elonmusk/status/1583098494579265536?s=21", true)]
        [TestCase("http://vxtwitter.com/elonmusk/status/1583098494579265536 dsad", false)]
        [TestCase("https://vxtwitter.com/elonmusk/status/1583098494579265536?s=21 dsad", false)]

        [TestCase("https://twitter.com/kanyewest/status/1583098494579265536", true)]
        [TestCase("http://twitter.com/kanyewest/status/1583098494579265536", true)]
        [TestCase("https://www.twitter.com/kanyewest/status/1583098494579265536", true)]
        [TestCase("http://www.twitter.com/kanyewest/status/1583098494579265536", true)]
        [TestCase("https://twitter.com/kanyewest/status/1583098494579265536?s=21", true)]
        [TestCase("http://twitter.com/kanyewest/status/1583098494579265536?s=21", true)]
        [TestCase("https://www.twitter.com/kanyewest/status/1583098494579265536?s=21", true)]
        [TestCase("http://www.twitter.com/kAnYewEst/status/1583098494579265536?s=21", true)]
        [TestCase("https://www.twitter.com/kanyewest/status/1583098494579265536 dsad", false)]
        [TestCase("http://www.twitter.com/kanyewest/status/1583098494579265536 dsad", false)]
        [TestCase("https://twitter.com/kanyewest/status/1583098494579265536?s=21 dsad", false)]
        [TestCase("http://twitter.com/kanyewest/status/1583098494579265536?s=21 dsad", false)]
        [TestCase("https://c.vxtwitter.com/kanyewest/status/1583098494579265536", true)]
        [TestCase("http://c.vxtwitter.com/kanyewest/status/1583098494579265536", true)]
        [TestCase("https://c.vxtwitter.com/kanyewest/status/1583098494579265536?s=21", true)]
        [TestCase("http://c.vxtwitter.com/kanyewest/status/1583098494579265536?s=21", true)]
        [TestCase("http://c.vxtwitter.com/kanyewest/status/1583098494579265536 dsad", false)]
        [TestCase("https://c.vxtwitter.com/kanyewest/status/1583098494579265536?s=21 dsad", false)]
        [TestCase("https://fxtwitter.com/kanyewest/status/1583098494579265536", true)]
        [TestCase("http://fxtwitter.com/kanyewest/status/1583098494579265536", true)]
        [TestCase("https://fxtwitter.com/kanyewest/status/1583098494579265536?s=21", true)]
        [TestCase("http://fxtwitter.com/kanyewest/status/1583098494579265536?s=21", true)]
        [TestCase("http://fxtwitter.com/kanyewest/status/1583098494579265536 dsad", false)]
        [TestCase("https://fxtwitter.com/kanyewest/status/1583098494579265536?s=21 dsad", false)]
        [TestCase("https://vxtwitter.com/kanyewest/status/1583098494579265536", true)]
        [TestCase("http://vxtwitter.com/kanyewest/status/1583098494579265536", true)]
        [TestCase("https://vxtwitter.com/kanyewest/status/1583098494579265536?s=21", true)]
        [TestCase("http://vxtwitter.com/kanyewest/status/1583098494579265536?s=21", true)]
        [TestCase("http://vxtwitter.com/kanyewest/status/1583098494579265536 dsad", false)]
        [TestCase("https://vxtwitter.com/kanyewest/status/1583098494579265536?s=21 dsad", false)]

        [TestCase("https://twitter.com/TolarianCollege/status/1583098494579265536", false)]
        [TestCase("http://twitter.com/TolarianCollege/status/1583098494579265536", false)]
        [TestCase("https://www.twitter.com/TolarianCollege/status/1583098494579265536", false)]
        [TestCase("http://www.twitter.com/TolarianCollege/status/1583098494579265536", false)]
        [TestCase("https://twitter.com/TolarianCollege/status/1583098494579265536?s=21", false)]
        [TestCase("http://twitter.com/TolarianCollege/status/1583098494579265536?s=21", false)]
        [TestCase("https://www.twitter.com/TolarianCollege/status/1583098494579265536?s=21", false)]
        [TestCase("http://www.twitter.com/TolarianCollege/status/1583098494579265536?s=21", false)]
        [TestCase("https://www.twitter.com/TolarianCollege/status/1583098494579265536 dsad", false)]
        [TestCase("http://www.twitter.com/TolarianCollege/status/1583098494579265536 dsad", false)]
        [TestCase("https://twitter.com/TolarianCollege/status/1583098494579265536?s=21 dsad", false)]
        [TestCase("http://twitter.com/TolarianCollege/status/1583098494579265536?s=21 dsad", false)]
        [TestCase("https://c.vxtwitter.com/TolarianCollege/status/1583098494579265536", false)]
        [TestCase("http://c.vxtwitter.com/TolarianCollege/status/1583098494579265536", false)]
        [TestCase("https://c.vxtwitter.com/TolarianCollege/status/1583098494579265536?s=21", false)]
        [TestCase("http://c.vxtwitter.com/TolarianCollege/status/1583098494579265536?s=21", false)]
        [TestCase("http://c.vxtwitter.com/TolarianCollege/status/1583098494579265536 dsad", false)]
        [TestCase("https://c.vxtwitter.com/TolarianCollege/status/1583098494579265536?s=21 dsad", false)]
        [TestCase("https://fxtwitter.com/TolarianCollege/status/1583098494579265536", false)]
        [TestCase("http://fxtwitter.com/TolarianCollege/status/1583098494579265536", false)]
        [TestCase("https://fxtwitter.com/TolarianCollege/status/1583098494579265536?s=21", false)]
        [TestCase("http://fxtwitter.com/TolarianCollege/status/1583098494579265536?s=21", false)]
        [TestCase("http://fxtwitter.com/TolarianCollege/status/1583098494579265536 dsad", false)]
        [TestCase("https://fxtwitter.com/TolarianCollege/status/1583098494579265536?s=21 dsad", false)]
        [TestCase("https://vxtwitter.com/TolarianCollege/status/1583098494579265536", false)]
        [TestCase("http://vxtwitter.com/TolarianCollege/status/1583098494579265536", false)]
        [TestCase("https://vxtwitter.com/TolarianCollege/status/1583098494579265536?s=21", false)]
        [TestCase("http://vxtwitter.com/TolarianCollege/status/1583098494579265536?s=21", false)]
        [TestCase("http://vxtwitter.com/TolarianCollege/status/1583098494579265536 dsad", false)]
        [TestCase("https://vxtwitter.com/TolarianCollege/status/1583098494579265536?s=21 dsad", false)]
        public void MessageExclusivelyContainsFlaggedUserTweet(string input, bool expectedResult)
        {
            Match match = Regex.Match(input, SharedConstants.FlaggedTwitterUserDetector, RegexOptions.IgnoreCase);
            bool result = match.Success && match.Length == input.Length;
            Assert.AreEqual(result, expectedResult);
        }
        #endregion

        #region String replacement checks
        [Test]
        [TestCase("https://twitter.com/TolarianCollege/status/1583098494579265536", "https://c.vxtwitter.com/TolarianCollege/status/1583098494579265536")]
        [TestCase("http://twitter.com/TolarianCollege/status/1583098494579265536", "http://c.vxtwitter.com/TolarianCollege/status/1583098494579265536")]
        [TestCase("https://www.twitter.com/TolarianCollege/status/1583098494579265536", "https://www.c.vxtwitter.com/TolarianCollege/status/1583098494579265536")]
        [TestCase("http://www.twitter.com/TolarianCollege/status/1583098494579265536", "http://www.c.vxtwitter.com/TolarianCollege/status/1583098494579265536")]
        [TestCase("@[CSB Enthusiast] reddy https://twitter.com/taylorswift13/status/1583083964931055616?s=46&t=4QkqtgX4-lvArl3IQbJukw", "@[CSB Enthusiast] reddy https://c.vxtwitter.com/taylorswift13/status/1583083964931055616?s=46&t=4QkqtgX4-lvArl3IQbJukw")]
        [TestCase("Some message that mentions www.twitter.com", "Some message that mentions www.c.vxtwitter.com")]
        [TestCase("Some message with a link inside of it https://cdn.discordapp.com/attachments/701194133074608198/1032671502753075240/IMG_9060.jpg", "Some message with a link inside of it https://cdn.discordapp.com/attachments/701194133074608198/1032671502753075240/IMG_9060.jpg")]
        public void TwitterReplacedWithFXTwitter(string inputString, string expectedString)
        {
            Assert.AreEqual(Utils.ReplaceTwitterWithFXTwitter(inputString), expectedString);
        }
        #endregion

        #region Twitter Utilities Checks
        [Test]
        [TestCase("https://c.vxtwitter.com/realDonaldTrump/status/332308211321425920?s=21", 332308211321425920)]
        [TestCase("https://c.vxtwitter.com/realDonaldTrump/status/332308211321425920", 332308211321425920)]
        [TestCase("https://vxtwitter.com/realDonaldTrump/status/332308211321425920?s=21", 332308211321425920)]
        [TestCase("https://vxtwitter.com/realDonaldTrump/status/332308211321425920", 332308211321425920)]
        [TestCase("https://fxtwitter.com/realDonaldTrump/status/332308211321425920?s=21", 332308211321425920)]
        [TestCase("https://fxtwitter.com/realDonaldTrump/status/332308211321425920", 332308211321425920)]
        [TestCase("https://twitter.com/realDonaldTrump/status/332308211321425920?s=21", 332308211321425920)]
        [TestCase("https://twitter.com/realDonaldTrump/status/332308211321425920", 332308211321425920)]
        public void ValidateCorrectTweetIDIsExtracted(string inputString, long expectedId)
        {
            Assert.AreEqual(TwitterUtilities.GetTweetId(inputString), expectedId);
        }
        #endregion
    }
}