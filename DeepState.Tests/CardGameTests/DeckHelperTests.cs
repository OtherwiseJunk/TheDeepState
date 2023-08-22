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

        [Test]
        public void BuildDeckShouldReturnARandomizedDeck()
        {
            int[] deckOne = helper.BuildDeck();
            int[] deckTwo = helper.BuildDeck();
            bool firstThreeCardsNotEqual = (deckOne[0] != deckTwo[0]) && (deckOne[1] != deckTwo[1]) && (deckOne[2] != deckTwo[2]);
            Assert.IsTrue(firstThreeCardsNotEqual);        }
    }
}
