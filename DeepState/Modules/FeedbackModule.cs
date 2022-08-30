using DeepState.Service;
using Discord;
using Discord.Commands;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using DartsDiscordBots.Services;
using System.Collections.Generic;
using System;

namespace DeepState.Modules
{
    [Group("Feedback"), Alias("fb", "complain")]
    public class FeedbackModule : ModuleBase
    {
        ILogger _log { get; set; }
        PanopticonService _panopticon { get; set; }
        ImagingService _imaging { get; set; }
        ulong FeedbackChannelId = 967308491041693706;
        ulong LibcraftGuildId = 698639095940907048;        

        public FeedbackModule(PanopticonService service, ImagingService imaging, ILogger logger)
        {
            _panopticon = service;
            _log = logger;
            _imaging = imaging;
        }

        [Command()]
        [Summary("Sends feedback to the Libcraft Admins")]
        public async Task CreateFeedback([Remainder] string feedback)
        {
            _log.Information("Received a feedback command");
            IGuild libcraftGuild = Context.Client.GetGuildAsync(LibcraftGuildId).Result;
            if(libcraftGuild != null)
            {
                ITextChannel feedbackChannel = (ITextChannel)libcraftGuild.GetChannelAsync(FeedbackChannelId).Result;
                if (feedbackChannel != null)
                {
                    feedback += GenerateAttachmentLinksString(Context.Message.Attachments);
                    _log.Information("Attempting to create feedback.");
                    _panopticon.CreateFeedback(feedback, Context.Message.Author.Id);
                    _log.Information("Created feedback");
                    int id = _panopticon.GetAllFeedback().First(fb => fb.Message == feedback.Replace(Environment.NewLine, String.Empty)).Id;
                    _log.Information("Extracted ID");
                    EmbedBuilder builder = new EmbedBuilder();
                    builder.Title = $"Feedback #{id}";
                    builder.Description = feedback;
                    await feedbackChannel.SendMessageAsync(embed: builder.Build());
                    await Context.Message.Author.SendMessageAsync("Successfully submitted your feedback to the Libcraft team.");
                    _log.Information("Feedback sent to reports channel.");
                }
                else
                {
                    _log.Error("Failed to find libcraft report channel when attempting to create feedback");
                }                
            }
            else
            {
                _log.Error("Failed to find libcraft Guild when attempting to create feedback");
            }
        }

        private string GenerateAttachmentLinksString(IReadOnlyCollection<IAttachment> attachments)
        {
            string links = $"{Environment.NewLine}Attachments:{Environment.NewLine}";
            if(attachments.Count == 0)
            {
                _log.Information("Found no attachments on message");
                return string.Empty;
            }
            foreach (IAttachment attachment in attachments)
            {
                links += $"{attachment.Url} {Environment.NewLine}";
            }
            _log.Information($"Found {attachments.Count} attachments, generated links.");
            return links;
        }
    }
}
