using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DeepState.Constants;
using Discord;
using Discord.WebSocket;

namespace DeepState.Utilities
{
	public static class Utilities
	{
		public static bool PercentileCheck(int successCheck)
		{
			return CreateSeededRandom().Next(1, 100) <= successCheck;
		}
		public static bool IsMentioningMe(SocketMessage discordMessage, SocketSelfUser currentUser)
		{
			IMessage replyingToMessage = discordMessage.Reference != null ? discordMessage.Channel.GetMessageAsync(discordMessage.Reference.MessageId.Value).Result : null;

			if (discordMessage.MentionedUsers.Contains(currentUser))
			{
				return true;
			}
			if (replyingToMessage != null && replyingToMessage.Author.Id == currentUser.Id)
			{
				return true;
			}
			if (Regex.IsMatch(discordMessage.Content, BotProperties.SelfIdentifyingRegex, RegexOptions.IgnoreCase))
			{
				return true;
			}
			return false;
		}

		public static Random CreateSeededRandom()
		{
			return new Random(Guid.NewGuid().GetHashCode());
		}
		
		public static bool IsSus(SocketMessage message)
		{
			if (Regex.IsMatch(message.Content, SharedConstants.SusRegex, RegexOptions.IgnoreCase))
			{
				return true;
			}
			return false;
		}
	}
}
