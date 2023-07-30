using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deepstate.Games.CardGames.Helpers
{
    public class DeckHelper
    {
        public int[] BuildDeck()
        {
            return Enumerable.Range(0, 51).ToArray();
        }
    }
}
