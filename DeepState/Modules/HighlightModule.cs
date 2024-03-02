using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepState.Modules
{
    internal class HighlightModule : ModuleBase
    {
        [Command("highlight")]
        [Summary("Registers the string as a highlight for you. When a message is sent to a channel you're in with the EXACT string (ignores case) the bot will send you a message with a link to the triggering message.")]
        public void CreateHighlight([Remainder] string triggerPhrase)
        {
            
        }
    }
}
