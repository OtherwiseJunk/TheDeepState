using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Deepstate.Games.CardGames;

namespace DeepState.Tests.CardGameTests
{
    class BlackjackTests
    {
        public BlackjackGame game;

        [SetUp]
        public void setup()
        {
            game = new();
        }

        [Test]
        public void NewGameShouldStartWith52Cards()
        {
            Assert.AreEqual(game.Deck.Distinct().Count(), 52);
        }
    }
}
