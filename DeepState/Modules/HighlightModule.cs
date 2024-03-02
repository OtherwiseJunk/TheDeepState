using DeepState.Data.Models;
using DeepState.Data.Services;
using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepState.Modules
{
    public class HighlightModule : ModuleBase
    {
        private HighlightService _service {get;}
        public HighlightModule(HighlightService service)
        {
            _service = service;
        }

        [Command("highlight")]
        [Summary("Registers the string as a highlight for you. When a message is sent to a channel you're in with the EXACT string (ignores case) the bot will send you a message with a link to the triggering message.")]
        public async Task CreateHighlight([Remainder] string triggerPhrase)
        {
            Console.WriteLine($"Saving highlight: {Context.Message.Author.Username} - {triggerPhrase}");
            _service.CreateHighlight(Context.Message.Author.Id, triggerPhrase);
            await Context.Message.AddReactionAsync(new Discord.Emoji("✅"));
            Console.WriteLine("Saved");
        }

        [Command("highlights")]
        [Summary("Lists all of your highlights.")]
        public async Task ListHighlights()
        {
            var highlights = _service.GetHighlightsForUser(Context.Message.Author.Id);
            var builder = new EmbedBuilder();
            builder.WithTitle("Highlight Trigger Phrases");
            builder.WithColor(Color.DarkBlue);
            foreach(Highlight highlight in highlights){
                builder.AddField($"{highlight.HighlightId}.", highlight.TriggerPhrase);
            }

            await Context.Message.ReplyAsync(embed: builder.Build());
        }
    }
}
