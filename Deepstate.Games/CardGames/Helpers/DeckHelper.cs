
using DartsDiscordBots.Utilities;

namespace Deepstate.Games.CardGames.Helpers
{
    public class DeckHelper
    {
        public int[] BuildDeck()
        {
            List<int> deck = Enumerable.Range(0, 52).ToList();
            deck.Shuffle();
            return deck.ToArray();
        }
    }
}
