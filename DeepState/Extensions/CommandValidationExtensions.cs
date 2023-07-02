using DartsDiscordBots.Constants;
using DeepState.Models;
using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi.Core.Models;

namespace DeepState.Extensions
{
    public static class CommandValidationExtensions
    {
        public static bool HasReferencedMessage(this IUserMessage message)
        {
            return message.ReferencedMessage != null;
        }

        public static bool IsSentByMe(this IUserMessage message, ulong myId)
        {
            return message.Author.Id == myId;
        }

        public static bool HasMySpecificReaction(this IUserMessage message, string emote)
        {
            return message.Reactions.Where(r => r.Key.Name == emote && r.Value.IsMe).Count() == 0;
        }

        public static bool HasSpecificAttachmentCount(this IUserMessage message, int attachmentCount)
        {
            return message.Attachments.Count == attachmentCount;
        }

        public static bool HasSpecificEmbedCount(this IUserMessage message, int embedCount)
        {
            return message.Embeds.Count == embedCount;
        }
    }
}
