using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepState.Extensions
{
    public static class DiscordDotNetExtensions
    {
        public static async Task ModifyTextChannel(this ITextChannel channel, string name = "", string topic = "")
        {
            await channel.ModifyAsync((channel) =>
            {
                if (!String.IsNullOrEmpty(name))
                {
                    channel.Name = name;
                }
                if (!String.IsNullOrEmpty(topic))
                {
                    channel.Topic = topic;
                }
            });
        }
    }
}
