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
            _service.CreateHighlight(Context.Message.Author.Id, triggerPhrase);
            await Context.Message.AddReactionAsync(new Emoji("✅"));
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

        [Command("deletehighlight"), Alias("hld")]
        [Summary("Deletes a highlight.")]
        public async Task DeleteHighlight([Remainder] string highlightLookup)
        {
            Highlight highlightToDelete = null;
            int highlightId;
            string errorMessage = "Shoot, something went wrong but don't ask me what.";
            if(int.TryParse(highlightLookup, out highlightId)){
                highlightToDelete = _service.GetHighlightsForUser(Context.Message.Author.Id).FirstOrDefault(h => h.HighlightId == highlightId);
                errorMessage = $"Highlight {highlightId} not found in your list of highlights.";
            }
            else{
                highlightToDelete = _service.GetHighlightsForUser(Context.Message.Author.Id).FirstOrDefault(h => h.TriggerPhrase == highlightLookup.ToLower());
                errorMessage = $"Highlight with a trigger phrase of `{highlightLookup}` not found in your list of highlights.";
            }

            if(highlightToDelete == null)
            {
                await Context.Message.ReplyAsync(errorMessage);
                return;
            }
            _service.DeleteHighlight(highlightToDelete);
            await Context.Message.AddReactionAsync(new Emoji("✅"));
        }
    }
}

