using NUnit.Framework;
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
        [TestCase("https://fxtwitter.com/TolarianCollege/status/1583098494579265536", false)]
        [TestCase("http://fxtwitter.com/TolarianCollege/status/1583098494579265536", false)]
        [TestCase("https://www.fxtwitter.com/TolarianCollege/status/1583098494579265536.", false)]
        [TestCase("http://www.fxtwitter.com/TolarianCollege/status/1583098494579265536", false)]
        public void ContainsTwitterLink(string stringToCheck, bool expectedResult)
        {
            Assert.AreEqual(Utils.ContainsTwitterLink(stringToCheck), expectedResult);
        }
        #endregion

        #region String replacement checks
        [Test]
        [TestCase("https://twitter.com/TolarianCollege/status/1583098494579265536", "https://fxtwitter.com/TolarianCollege/status/1583098494579265536")]
        [TestCase("http://twitter.com/TolarianCollege/status/1583098494579265536", "http://fxtwitter.com/TolarianCollege/status/1583098494579265536")]
        [TestCase("https://www.twitter.com/TolarianCollege/status/1583098494579265536", "https://www.fxtwitter.com/TolarianCollege/status/1583098494579265536")]
        [TestCase("http://www.twitter.com/TolarianCollege/status/1583098494579265536", "http://www.fxtwitter.com/TolarianCollege/status/1583098494579265536")]
        [TestCase("@[CSB Enthusiast] reddy https://twitter.com/taylorswift13/status/1583083964931055616?s=46&t=4QkqtgX4-lvArl3IQbJukw", "@[CSB Enthusiast] reddy https://fxtwitter.com/taylorswift13/status/1583083964931055616?s=46&t=4QkqtgX4-lvArl3IQbJukw")]
        [TestCase("Some message that mentions www.twitter.com", "Some message that mentions www.fxtwitter.com")]
        [TestCase("Some message with a link inside of it https://cdn.discordapp.com/attachments/701194133074608198/1032671502753075240/IMG_9060.jpg", "Some message with a link inside of it https://cdn.discordapp.com/attachments/701194133074608198/1032671502753075240/IMG_9060.jpg")]
        public void TwitterReplacedWithFXTwitter(string inputString, string expectedString)
        {
            Assert.AreEqual(Utils.ReplaceTwitterWithFXTwitter(inputString), expectedString);
        }
        #endregion
    }
}