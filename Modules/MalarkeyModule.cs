using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheDeepState.Constants;
using TraceLd.MineStatSharp;

namespace TheDeepState.Modules
{
	public class MalarkeyModule : ModuleBase
	{
		[Command("clap")]
		[Summary("Places a 👏 emoji in place of any spaces. Will delete the original message, but will include the triggering user's username.")]
		public async Task Clap([Summary("The message to Clapify."), Remainder] string msg)
		{
			string user = (Context.Message.Author as IGuildUser).Nickname ?? Context.Message.Author.Username;

			if (Context.Message.Reference != null)
			{
				MessageReference messageRepliedTo = Context.Message.Reference;
				AllowedMentions allowedMentions = AllowedMentions.None;

				//Only Ping with the reply if the original reply pinged.
				if (Context.Message.MentionedUserIds.Count > 0)
				{
					allowedMentions = AllowedMentions.All;
				}

				await Context.Message.DeleteAsync();
				await Context.Channel.SendMessageAsync(SharedConstants.ReplacedMessageFormat(user, Clapify(msg)), messageReference: messageRepliedTo, allowedMentions: allowedMentions);
			}
			else
			{
				await Context.Message.DeleteAsync();
				await Context.Channel.SendMessageAsync(SharedConstants.ReplacedMessageFormat(user, Clapify(msg)));
			}						
		}
		public string Clapify(string messageToClapify)
		{
			return $"👏 { string.Join(" 👏 ", messageToClapify.Split(' '))} 👏";
		}

		[Command("jackbox")]
		[Summary("Makes a jackbox poll, and will announce a winner after 5 mintues. User must provide a comma separated list of the jack.")]
		public async Task Jackbox([Summary("A comma seperated list of the versions of jackbox to make the list for")] string versions)
		{
			List<string> versionList = versions.Split(',').ToList();
			List<string> pollGameList = new List<string>();

			for (int i = 1; i < 8; i++)
			{
				if (versionList.Contains(i.ToString()))
				{
					Dictionary<int, List<string>> jackbox = JackboxConstants.JackboxGameListByNumber;
					var gameList = jackbox[i];
					pollGameList.AddRange(gameList);
				}
			}

			await Context.Channel.SendMessageAsync(string.Join(Environment.NewLine, pollGameList));
		}

		[Command("mock")]
		[Summary("Turns the text following the command into MoCKinG TExT lIkE THiS.")]
		public async Task Mock([Summary("The message to Mockify"), Remainder] string msg)
		{									
			string user = (Context.Message.Author as IGuildUser).Nickname ?? Context.Message.Author.Username;			

			if (Context.Message.Reference != null)
			{
				MessageReference messageRepliedTo = Context.Message.Reference;
				AllowedMentions allowedMentions = AllowedMentions.None;

				//Only Ping with the reply if the original reply pinged.
				if (Context.Message.MentionedUserIds.Count > 0)
				{
					allowedMentions = AllowedMentions.All;
				}

				await Context.Message.DeleteAsync();
				await Context.Channel.SendMessageAsync(SharedConstants.ReplacedMessageFormat(user, Mockify(msg)),messageReference: messageRepliedTo, allowedMentions: allowedMentions);
			}
			else
			{
				await Context.Message.DeleteAsync();
				await Context.Channel.SendMessageAsync(SharedConstants.ReplacedMessageFormat(user, Mockify(msg)));
			}
			
		}

		public string Mockify(string messageToMockify)
		{
			//Seed it based on the Discord Message Content length, author username length, guild name length, and channelname length
			int seed = Context.Message.Content.Length + Context.Message.Author.Username.Length + Context.Channel.Name.Length + Context.Guild.Name.Length;
			Random rand = new Random(seed);
			List<string> words = messageToMockify.ToLower().Split(' ').ToList();
			string mockifiedMessage = "";
			
			double oneThird = 1 / 3.0;
			double chance = oneThird;
			bool capitalize = rand.Next() % 2 == 0;

			foreach (string word in words)
			{
				foreach (char letter in word)
				{
					if (Char.IsLetter(letter))
					{
						// The new algorithm guarantees that there is never a run of greater than three characters with the same capitalization status.
						// Essentially, the chance of switching capitalization increases each time it doesn't switch.
						// Note that it doesn't increment if the current character isn't a letter. This shouldn't matter much.
						if (rand.NextDouble() < chance) {
							capitalize = !capitalize;							
							chance = oneThird;
						} else {							
							chance += oneThird;
						}
						if (capitalize) {
							mockifiedMessage += Char.ToUpper(letter);
						} else {
							mockifiedMessage += letter;
						}
					}
					else
					{
						mockifiedMessage += letter;
					}
				}
				mockifiedMessage += " ";
			}
			return mockifiedMessage;
		}

		[Command("mstatus"), Alias("minecraft", "minecraftstatus")]
		[Summary("Returns a message with a status of Sporf's Minecraft server")]
		public async Task MinecraftStatus()
		{
			MineStat ms = new MineStat(SporfbaseConstants.ServerAddress, SporfbaseConstants.ServerPort);

			if (ms.ServerUp)
			{
				EmbedBuilder eb = new EmbedBuilder();

				eb.WithTitle($"{SporfbaseConstants.ServerAddress} Status");
				eb.AddField("Player Count:", $"{ms.CurrentPlayers}/{ms.MaximumPlayers}");
				eb.AddField("MotD:", $"{ms.Motd}");				
				await Context.Channel.SendMessageAsync("", false, eb.Build());
			}
			else
			{
				await Context.Channel.SendMessageAsync("Server is offline!");
			}
		}
	}
}