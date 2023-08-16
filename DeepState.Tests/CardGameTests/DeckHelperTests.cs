using Deepstate.Games.CardGames.Helpers;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System.Collections.Generic;
using System.Linq;

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
            Assert.AreEqual(helper.BuildDeck().Distinct().Count(), 52);
        }
    }
}
