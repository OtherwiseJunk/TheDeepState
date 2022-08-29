﻿using DeepState.Service;
using Discord;
using Discord.Commands;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using System;

namespace DeepState.Modules
{
    [Group("Feedback"), Alias("fb", "complain")]
    public class FeedbackModule : ModuleBase
    {
        ILogger _log { get; set; }
        PanopticonService _service { get; set; }
        ulong FeedbackChannelId = 967308491041693706;
        ulong LibcraftGuildId = 698639095940907048;

        public FeedbackModule(PanopticonService service, ILogger logger)
        {
            _service = service;
            _log = logger;
        }

        [Command()]
        [Summary("Sends feedback to the Libcraft Admins")]
        public async Task CreateFeedback([Remainder] string feedback)
        {
            try
            {
                _log.Information("Received a feedback command");

                IGuild libcraftGuild = null;
                foreach(IGuild guild in Context.Client.GetGuildsAsync().Result)
                {
                    Console.WriteLine($"Does guild ID match? {guild.Id} == {LibcraftGuildId} = {guild.Id == LibcraftGuildId}");
                    if(guild.Id == LibcraftGuildId)
                    {
                        libcraftGuild = guild;
                    }
                }
                if (libcraftGuild == null)
                {
                    ITextChannel feedbackChannel = (ITextChannel)libcraftGuild.GetChannelAsync(FeedbackChannelId).Result;
                    if (feedbackChannel == null)
                    {
                        _log.Information("Attempting to create feedback.");
                        _service.CreateFeedback(feedback, Context.Message.Author.Id);
                        _log.Information("Created feedback");
                        int id = _service.GetAllFeedback().First(fb => fb.Message == feedback).Id;
                        _log.Information("Extracted ID");
                        EmbedBuilder builder = new EmbedBuilder();
                        builder.Title = $"Feedback #{id}";
                        builder.Description = feedback;
                        await feedbackChannel.SendMessageAsync(embed: builder.Build());
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
            catch(Exception ex)
            {
                _log.Error(ex.Message);
                _log.Information("Looping through any inner exceptions...");
                while (ex.InnerException != null)
                {
                    _log.Error(ex.InnerException.Message);
                    ex = ex.InnerException;
                }
            }
        }
    }
}
