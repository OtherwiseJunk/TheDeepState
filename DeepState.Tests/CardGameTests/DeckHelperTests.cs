using Deepstate.Games.CardGames.Helpers;
using NUnit.Framework;

namespace DeepState.Tests.CardGameTests
{
    public class DeckHelperTests
    {
        public DeckHelper helper;
        [SetUp]
        public void Setup()
        {
            helper = new();
        }

        [Test]
        public void BuildDeckShouldReturnADeckOf52DistinctCards()
        {
            Assert.AreEqual(helper.BuildDeck().ToList().Distinct().Count(), 52);
        }
    }
}
