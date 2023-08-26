using Discord.WebSocket;
using System;
using Utils = DeepState.Utilities.Utilities;
using System.Threading.Tasks;
using DeepState.Constants;
using DartsDiscordBots.Utilities;
using Discord;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DeepState.Data.Services;
using static DartsDiscordBots.Constants.SharedConstants;
using DeepState.Utilities;

namespace DeepState.Handlers
{
	public static class OnMessageHandlers
	{
		
		static HashSet<ulong> GuildUserCacheDownloaded = new();
		static object HashsetLock = new();
		public static void EgoCheck(SocketMessage msg, bool isMentioningMe)
		{
			if (isMentioningMe)
			{
				_ = msg.AddReactionAsync(Emote.Parse(SharedConstants.RomneyRightEyeID));
				_ = msg.AddReactionAsync(Emote.Parse(SharedConstants.RomneyLeftEyeID));
			}
		}
		public static void Imposter(SocketMessage msg, bool isSus)
		{
			if (isSus)
			{
				_ = msg.AddReactionAsync(Emote.Parse(SharedConstants.SusID));
			}
		}
		public static async Task DownloadUsersForGuild(SocketMessage msg, IGuild guild)
		{
			if(!GuildUserCacheDownloaded.Contains(guild.Id)){				
				await guild.DownloadUsersAsync();
				lock (HashsetLock)
				{
					GuildUserCacheDownloaded.Add(guild.Id);
				}				
			}
		}

		public static async Task ImageServiceTest(SocketMessage msg)
		{
			msg.Author.GetAvatarUrl(size: 512);
			//ImagingService.OverlayImage()

        }

		public static async Task UWUIfyFlaggedUserTweets(SocketMessage msg) 
        {
			if (TwitterUtilities.MessageExclusivelyContainsFlaggedUserTweetURL(msg.Content))
            {
				Embed embed = await TwitterUtilities.GetUwuifiedTwitterEmbed(msg.Content, BotUtilities.GetDisplayNameForUser((IGuildUser)msg.Author));
				if (embed != null)
				{
					await msg.Channel.SendMessageAsync(embed: embed);
					_ = msg.DeleteAsync();
				}
			}
        }

		public static async Task ReplyToAllTwitterLinksWithCVXTwitter(SocketMessage msg)
        {
            if (Utils.ContainsTwitterLink(msg.Content))
            {
				await (msg as IUserMessage).ReplyAsync($"**{BotUtilities.GetDisplayNameForUser(msg.Author as IGuildUser)}**: {Utils.ReplaceTwitterWithFXTwitter(msg.Content)}", allowedMentions: AllowedMentions.None);
				await msg.DeleteAsync();
            }
        }
		public static async Task DeletePreggersMessage(SocketMessage msg)
        {
            MatchCollection matches = Regex.Matches(msg.Content.Normalize(System.Text.NormalizationForm.FormD), SharedConstants.PreggersDetector, RegexOptions.IgnoreCase);
            string spacelessContent = msg.Content.Replace(" ", String.Empty);
            bool cursedCheck = spacelessContent.Length >= 5 && spacelessContent.Length <= 8 && (Regex.Matches(spacelessContent, SharedConstants.PreggersDetector, RegexOptions.IgnoreCase)).Count > 0;

            if (matches.Count > 0 || msg.Content.StartsWith("🇵🇷🥚") || cursedCheck)
            {
				DateTime now = DateTime.Now;
				if(now.Month == 12 && now.Day == 11)
				{
					EgoCheck(msg, true);
                }
				else
				{
                    string content = $"{msg.Author.Username} in {(msg.Channel as IGuildChannel).Guild.Name}/{msg.Channel.Name} as [{DateTime.Now.ToString("MM/dd/yyyy hh:mm tt")}]: {msg.Content}";
                    Console.WriteLine("Detected someone trying to say preggers, I think...");
                    Console.WriteLine(content);
                    if (Utils.PercentileCheck(10))
                    {
                        _ = msg.Channel.SendMessageAsync("https://c.tenor.com/BH_8JPewRk4AAAAd/free-guy-ryan-reynolds.gif");
                    }
                    _ = msg.Channel.SendMessageAsync("Gwalms.........");
                    _ = msg.DeleteAsync();
                }				
            }
        }
		public static async Task RandomReactCheck(SocketMessage msg)
		{
			if (msg.Content.ToLower() == "!rank") Console.WriteLine("Rolling for rank...");
			if (msg.Content.ToLower() == "!rank" && Utils.PercentileCheck(1))
			{
				await msg.Channel.SendMessageAsync(SharedConstants.RankNerdResponses.GetRandom());
				return;
			}
			if (Utils.PercentileCheck(1) & Utils.PercentileCheck(40))
			{
				if (Utils.PercentileCheck(1))
				{
					await msg.AddReactionAsync(Emote.Parse(SharedConstants.GwalmsID));
				}
				else
				{
					await msg.AddReactionAsync(Emote.Parse(SharedConstants.ReactableEmotes.GetRandom()));
				}
			}
			if(msg.Content.ToLower() == "!eank" && Utils.PercentileCheck(10))
			{
				msg.Channel.SendMessageAsync("J0nny5 detected. Preparing to terminate...");
			}
		}

		public static async Task MalarkeyLevelOfHandler(SocketMessage msg)
		{
			if (msg.Content.ToLower().Contains("malarkey level of"))
			{
				DateTime now = DateTime.Now;
				IEmote react = SharedConstants.MalarkeyLevels.GetRandom();

                if (now.Day == 1 && now.Month == 4)
				{
					react = SharedConstants.AprilFoolsMalarkeyLevels.GetRandom();
				}

                _ = msg.AddReactionAsync(react);
            }
		}

		public static async Task TableFlipCheck(SocketMessage msg, IGuild guild, UserRecordsService userRecordService)
        {
            if (BotUtilities.isUserFlippingTable(msg.Content, out TableFlipType? type) && userRecordService.UserRecordExists(msg.Author.Id, guild.Id))
            {
				int tablesFlipped = userRecordService.IncrementTableflip(msg.Author.Id, guild.Id);
				TableFlipTier tier = GetTableflipTier(tablesFlipped);
                switch (type)
                {
                    case TableFlipType.Single:
						_ = msg.Channel.SendMessageAsync(UnflippedTable);
						break;
					case TableFlipType.Double:
                        _ = msg.Channel.SendMessageAsync(LeftDoubleUnflippedTable);
						_ = msg.Channel.SendMessageAsync(RightDoubleUnflippedTable);
						break;
					case TableFlipType.Enraged:
						_ = msg.Channel.SendMessageAsync(EnragedUnflippedTable);
						break;
				}

                _ = msg.Channel.SendMessageAsync(String.Format(TableFlipResponses[tier].GetRandom(), DartsDiscordBots.Utilities.BotUtilities.GetDisplayNameForUser((IGuildUser) msg.Author)));
            }
        }
		private static TableFlipTier GetTableflipTier(int tablesFlipped)
        {
			const int tierSize = 35;

			switch (tablesFlipped)
            {
				case < (1 * tierSize):
                    return TableFlipTier.Polite;
				case < (2 * tierSize):
					return TableFlipTier.Chastising;
				case < (3 * tierSize):
					return TableFlipTier.Aggresive;
				case < (4 * tierSize):
					return TableFlipTier.Scalding;
				case > (5 * tierSize):
					return TableFlipTier.NavySeal;
                default:
					return TableFlipTier.Polite;
            }
        }
	}
}
