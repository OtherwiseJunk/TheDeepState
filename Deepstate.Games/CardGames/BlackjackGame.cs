using Deepstate.Games.CardGames.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deepstate.Games.CardGames
{
    public class BlackjackGame : ICardGame
    {
        public int[] Deck { get; set; }

        public BlackjackGame() { }
    }
}
