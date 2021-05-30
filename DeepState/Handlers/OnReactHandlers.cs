using DartsDiscordBots.Utilities;
using DeepState.Constants;
using Discord;
using Discord.WebSocket;
using System;
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
	}
}
