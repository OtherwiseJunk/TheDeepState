using DartsDiscordBots.Modules.Jackbox;
using DartsDiscordBots.Modules.Jackbox.Interfaces;
using DartsDiscordBots.Modules.Jackbox.Models;
using DeepState.Data.Context;
using Discord;
using Discord.Commands;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepState.Data.Services
{
	public class JackboxService : IJackboxService
	{
		IDbContextFactory<JackboxContext> _contextFactory { get; set; }

		public JackboxService(IDbContextFactory<JackboxContext> contextFactory)
		{
			_contextFactory = contextFactory;
		}
		public JackboxGame GetGameDetailsForGuild(ulong discordGuildId, string gameName)
		{
			JackboxGame game;

			using (JackboxContext context = _contextFactory.CreateDbContext())
			{
				game = context.JackboxGames.FirstOrDefault(jbg => jbg.Name.ToLower() == gameName.ToLower() && jbg.DiscordGuildId == discordGuildId);
				if(game == null)
				{
					game = context.JackboxGames.FirstOrDefault(jbg => jbg.Name.ToLower() == gameName.ToLower() && jbg.DiscordGuildId == JackboxConstants.DefaultDiscordGuildId);
				}
			}

			return game;
		}

		public List<JackboxGame> GetGamelistForGuild(ulong discordGuildId, int[] versionList)
		{
			List<JackboxGame> gameList;

			using (JackboxContext context = _contextFactory.CreateDbContext())
			{
				// First get the custom configurations for any games for this guild
				gameList = context.JackboxGames.AsQueryable<JackboxGame>()
					.Where(jbg => jbg.DiscordGuildId == discordGuildId && versionList.Contains(jbg.JackboxVersion)).ToList();

				// Add the default version of any game not already in the list
				gameList.AddRange(
					context.JackboxGames.AsQueryable<JackboxGame>()
					.Where(jbg => !gameList.Select(g => g.Name).Contains(jbg.Name) && jbg.DiscordGuildId == JackboxConstants.DefaultDiscordGuildId)
				);
				// Ratings are store on default versions, retrieve it for all games in the list with a custom modification
				gameList.ForEach((JackboxGame game) =>
				{
					if (game.DiscordGuildId != JackboxConstants.DefaultDiscordGuildId)
					{
						game.Ratings = context.JackboxGames.First(jbg => jbg.DiscordGuildId == JackboxConstants.DefaultDiscordGuildId && jbg.Name == game.Name).Ratings;
					}
				});
			}

			return gameList;
		}

		public GameRating GetPlayerGameRating(ulong discordUserId, string gameName)
		{
			using (JackboxContext context = _contextFactory.CreateDbContext())
			{
				return context.GameRatings.FirstOrDefault(gr => gr.DiscordUserId == discordUserId && gr.JackboxGame.Name == gameName);
			}
		}

		public List<GameRating> GetPlayerGameRatings(ulong discordUserId)
		{
			using (JackboxContext context = _contextFactory.CreateDbContext())
			{
				return context.GameRatings.AsQueryable<GameRating>().Where(gr => gr.DiscordUserId == discordUserId).ToList();
			}
		}

		public int[] ParseVersionList(string versionList, ICommandContext context)
		{
			int[] versions;
			try
			{
				versions = versionList.Split(',').ToList().ConvertAll(int.Parse).ToArray();
				return versions;
			}
			catch
			{
				context.Channel.SendMessageAsync(JackboxConstants.ParsingIntError);
				return null;
			}
		}

		public void SetGameDescriptionForGuild(ulong discordGuildId, string gameName, string description)
		{
			using (JackboxContext context = _contextFactory.CreateDbContext())
			{
				JackboxGame game = GetGameDetailsForGuild(discordGuildId, gameName);
				if(game != null)
				{					
					if (game.DiscordGuildId != discordGuildId)
					{
						game = new JackboxGame { 
							DiscordGuildId = discordGuildId, 
							HasAudience = game.HasAudience, 
							JackboxVersion = game.JackboxVersion, 
							MaxPlayers = game.MaxPlayers, 
							MinPlayers = game.MinPlayers, 
							Name = game.Name, 
							PlayerName = game.PlayerName, 
							VotingEmoji = game.VotingEmoji };
						context.JackboxGames.Add(game);
					}

					game.Description = description;
					context.SaveChanges();
				}
			}
		}

		public void SetGamePlayerNameForGuild(ulong discordGuildId, string gameName, string playerName)
		{
			using (JackboxContext context = _contextFactory.CreateDbContext())
			{
				JackboxGame game = GetGameDetailsForGuild(discordGuildId, gameName);
				if (game != null)
				{
					if (game.DiscordGuildId != discordGuildId)
					{
						game = new JackboxGame { 
							DiscordGuildId = discordGuildId, 
							HasAudience = game.HasAudience, 
							JackboxVersion = game.JackboxVersion, 
							MaxPlayers = game.MaxPlayers, 
							MinPlayers = game.MinPlayers, 
							Name = game.Name, 
							Description = game.Description, 
							VotingEmoji = game.VotingEmoji };
						context.JackboxGames.Add(game);
					}

					game.PlayerName = playerName;
					context.SaveChanges();
				}
			}
		}

		public void SetGameVotingEmojiForGuild(ulong discordGuildId, string gameName, IEmote emote)
		{
			using (JackboxContext context = _contextFactory.CreateDbContext())
			{
				JackboxGame game = GetGameDetailsForGuild(discordGuildId, gameName);
				if (game != null)
				{
					if (game.DiscordGuildId != discordGuildId)
					{
						game = new JackboxGame { 
							DiscordGuildId = discordGuildId, 
							HasAudience = game.HasAudience, 
							JackboxVersion = game.JackboxVersion, 
							MaxPlayers = game.MaxPlayers, 
							MinPlayers = game.MinPlayers, 
							Name = game.Name, 
							Description = game.Description, 
							PlayerName = game.PlayerName };
						context.JackboxGames.Add(game);
					}

					game.VotingEmoji = emote;
					context.SaveChanges();
				}
			}
		}

		public GameRating SetPlayerGameRating(ulong discordUserId, string gameName, int rating)
		{
			using (JackboxContext context = _contextFactory.CreateDbContext())
			{
				JackboxGame game = context.JackboxGames.FirstOrDefault(jbg => jbg.Name == gameName && jbg.DiscordGuildId == JackboxConstants.DefaultDiscordGuildId);

				GameRating gameRating = context.GameRatings.AsQueryable<GameRating>().FirstOrDefault(gr => gr.DiscordUserId == discordUserId && gr.JackboxGame.Name.ToLower() == gameName.ToLower());
				if (gameRating == null)
				{
					gameRating = new GameRating { JackboxGame = game, DiscordUserId = discordUserId, Rating = rating, JackboxGameId = game.ID};
					context.GameRatings.Add(gameRating);
				}

				gameRating.Rating = rating;
				context.SaveChanges();

				return gameRating;
			}
		}
	}
}
