using DartsDiscordBots.Utilities;
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
						await channel.SendMessageAsync($"WE GOT HIM! {channel.GetUserAsync(SharedConstants.ThePoliceUser).Result.Mention}");
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

		public static async Task CheckForTributePages(SocketReaction reaction, ISocketMessageChannel channel, IMessage msg, SocketSelfUser currentUser, HungerGamesService service)
		{
			//We only allow page changes for the first five minutes of a message.
			if((DateTime.Now - msg.Timestamp.DateTime).Minutes >= 5)
			{
				return;
			}
			//We only care about messages the bot has sent.
			if(msg.Author.Id != currentUser.Id)
			{
				return;
			}
			//We only care about about messages with 1 embed.
			if(msg.Embeds.Count != 1)
			{
				return;
			}
			//We don't care about reactions that the bot added 
			if(reaction.User.Value.Id == currentUser.Id)
			{
				return;
			}
			//Finally, we only care about the embed with our specific title
			IEmbed embed = msg.Embeds.First();
			if(embed.Title != HungerGameConstants.HungerGameTributesEmbedTitle)
			{
				return;
			}

			int currentPage = Int32.Parse(embed.Footer.Value.Text);

			if (reaction.Emote.Name == SharedConstants.LeftArrowEmoji)
			{
				//Only bother if we're not on the first page.
				if (currentPage > 0)
				{
					_ = ((IUserMessage)msg).ModifyAsync(msg =>
					  {
						  IGuild guild = ((IGuildChannel)channel).Guild;
						  List<HungerGamesTribute> tributes = service.GetPagedTributeList(guild.Id, out currentPage, --currentPage);
						  msg.Embed = HungerGameUtilities.BuildTributeEmbed(tributes, currentPage, guild);
					  });
				}
				_ = msg.RemoveReactionAsync(reaction.Emote, reaction.User.Value);
			}
			if (reaction.Emote.Name == SharedConstants.RightArrowEmoji)
			{
				_ = ((IUserMessage)msg).ModifyAsync(msg =>
				{
					IGuild guild = ((IGuildChannel)channel).Guild;
					List<HungerGamesTribute> tributes = service.GetPagedTributeList(guild.Id, out currentPage, ++currentPage);
					msg.Embed = HungerGameUtilities.BuildTributeEmbed(tributes, currentPage, guild);
				});
				_ = msg.RemoveReactionAsync(reaction.Emote, reaction.User.Value);
			}
		}
	}
}
