using DartsDiscordBots.Utilities;
using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepState.Handlers
{
    internal class OnEventCreatedHandlers
    {
        public static void AnnounceNewEvent(SocketGuildEvent arg)
        {
            ITextChannel announcementChnl = (ITextChannel) arg.Guild.Channels.FirstOrDefault(c => c.Name.ToLower() == "announcements");
            if(announcementChnl != null)
            {
                string creator = BotUtilities.GetDisplayNameForUser(arg.Creator);
                EmbedBuilder eb = new();
                eb.Title = $"{arg.Name} by {creator}";
                eb.AddField("When", $"{arg.StartTime.ToString("f")}");
                if (arg.Type == GuildScheduledEventType.External)
                {
                    eb.AddField("Where", $"{arg.Location.ToString()}");
                }
                else if (arg.Type == GuildScheduledEventType.Voice)
                {
                    eb.AddField("Where", $"{arg.Channel}");
                }
                eb.AddField("Why", $"{arg.Description}");                
                if(arg.CoverImageId != null) eb.ImageUrl = arg.GetCoverImageUrl();
                announcementChnl.SendMessageAsync($"{creator} has created a new event!", embed: eb.Build());
            }
        }
    }
}
