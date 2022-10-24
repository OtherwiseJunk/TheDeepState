using DartsDiscordBots.Utilities;
using DeepState.Utilities;
using DeepState.Constants;
using DeepState.Data.Constants;
using DeepState.Data.Models;
using DeepState.Data.Services;
using DeepState.Utilities;
using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DeepState.Handlers
{
	public class OnReactHandlers
	{
		public static async Task KlaxonCheck(IEmote reactionEmote, ISocketMessageChannel channel, IMessage msg)
		{
			//If it's an Emote, extract the ID. Otherwise we will not need it.
			ulong? emoteId = (reactionEmote as Emote) != null ? (ulong?)(reactionEmote as Emote).Id : null;
			if (SharedConstants.EmoteNameandId(reactionEmote.Name, emoteId) == SharedConstants.QIID && msg.Reactions[Emote.Parse(SharedConstants.QIID)].ReactionCount == 1)
			{
				await channel.SendMessageAsync(SharedConstants.KlaxonResponse, messageReference: new MessageReference(msg.Id));
			}
		}

		public static async Task SelfReactCheck(SocketReaction reaction, ISocketMessageChannel channel, IMessage msg)
		{
			if (msg.Reactions.Count == 1)
			{
				if (msg.Reactions.First().Value.ReactionCount == 1 && reaction.UserId == msg.Author.Id && channel.Id != SharedConstants.SelfCareChannelId)
				{
					await msg.AddReactionAsync(Emote.Parse(SharedConstants.YouAreWhiteID));
					await channel.SendMessageAsync($"{msg.Author.Mention} {SharedConstants.SelfReactResponses.GetRandom()}", messageReference: new MessageReference(msg.Id), allowedMentions: AllowedMentions.All);
					if (msg.Author.Id == SharedConstants.TheCheatingUser)
					{
						await channel.SendMessageAsync($"WE GOT HIM! {channel.GetUserAsync(SharedConstants.ThePoliceUser, CacheMode.AllowDownload).Result.Mention}");
					}
				}
            }
        }

        public static async Task ClearDeepStateReactionCheck(IEmote reactionEmote, ISocketMessageChannel channel, IMessage msg, SocketSelfUser currentUser)
        {
            //Check for the clearing reaction emote, and do no clear if the author is Deep State.
            if (SharedConstants.ClearingEmotes.Contains(reactionEmote.Name) && msg.Author != currentUser)
            {
                foreach (var reaction in msg.Reactions)
                {
                    if (reaction.Value.IsMe)
                    {
                        await msg.RemoveReactionAsync(reaction.Key, currentUser);
                    }
                }
            }
        }

		public static async Task DeleteTwitterMessage(IEmote reactionEmote, IGuildUser reactingUser, IMessage msg, SocketSelfUser currentUser)
        {
			if (SharedConstants.ClearingEmotes.Contains(reactionEmote.Name) && msg.Author == currentUser)
			{
				if(msg.Content.Contains("c.vxtwitter.com") && msg.Content.StartsWith($"**{BotUtilities.GetDisplayNameForUser(reactingUser)}**:"))
                {
					await msg.DeleteAsync();
                }
			}
		}
        public static async Task MarxEmbedPagingHandler(SocketReaction reaction, IMessage message, SocketSelfUser currentUser, string embedTitle, IServiceProvider serviceProvider)
		{
			//We only allow page changes for the first five minutes of a message.
			if ((DateTime.Now - message.Timestamp.DateTime).Minutes >= 5)
			{
				return;
			}
			//We only care about messages the bot has sent.
			if (message.Author.Id != currentUser.Id)
			{
				return;
			}
			//We only care about about messages with 1 embed.
			if (message.Embeds.Count != 1)
			{
				return;
			}
			//We don't care about reactions that the bot added 
			if (reaction.User.Value.Id == currentUser.Id)
			{
				return;
			}
			//Finally, we only care about the embed with our specific title
			IEmbed embed = message.Embeds.First();
			if (!embed.Title.Contains(" Distribution"))
			{
				return;
			}


			int currentPage = Int32.Parse(embed.Footer.Value.Text);
			//ToDo: Extract <distributionAmount:requestingUserDIscordId> from title instead of just distribution amount
			int distributionAmount = Int32.Parse(Regex.Match(embed.Title, @"\d+").Value);
			
			UserRecordsService urs = (UserRecordsService)serviceProvider.GetService(typeof(UserRecordsService));

			if (reaction.Emote.Name == DartsDiscordBots.Constants.SharedConstants.LeftArrowEmoji)
			{
				//Only bother if we're not on the first page.
				if (currentPage > 0)
				{
					// Embed newEmbed = PagingUtilities.MarxEmbedPagingCalback(message, serviceProvider, currentPage, false, distributionAmount);
					_ = ((IUserMessage)message).ModifyAsync(msg =>
					{
						// msg.Embed = newEmbed;
					});
				}
				_ = message.RemoveReactionAsync(reaction.Emote, reaction.User.Value);
			}
			if (reaction.Emote.Name == DartsDiscordBots.Constants.SharedConstants.RightArrowEmoji)
			{
				// Embed newEmbed = PagingUtilities.MarxEmbedPagingCalback(message, serviceProvider, currentPage, true, distributionAmount);
				_ = ((IUserMessage)message).ModifyAsync(msg =>
				{
					// msg.Embed = newEmbed;
				});
				_ = message.RemoveReactionAsync(reaction.Emote, reaction.User.Value);
			}
		}
	}
}
