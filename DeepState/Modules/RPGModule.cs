using DeepState.Data.Constants;
using DeepState.Data.Models;
using DeepState.Data.Services;
using DeepState.Modules.Preconditions;
using DeepState.Service;
using Discord;
using Discord.Commands;
using SkiaSharp;
using Svg;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DeepState.Modules
{
	[Group("rpg"), Name("RPG Module")]
	public class RPGModule : ModuleBase
	{
		public RPGService _rpgService { get; set; }
		public UserRecordsService _userService { get; set; }
		public ImagingService _imagingService { get; set; }
		public RPGModule(RPGService rpgService, UserRecordsService userService, ImagingService imagingService)
		{
			_rpgService = rpgService;
			_userService = userService;
			_imagingService = imagingService;
		}

		[Command("newchar"), Alias("nc")]
		[RequireLibcoinBalance(RPGConstants.NewCharacterCost)]
		public async Task GenerateNewCharacter()
		{
			Character character = _rpgService.GetCharacter((IGuildUser)Context.User);
			if (character == null)
			{
				try
				{
					using (WebClient wc = new WebClient())
					{
						string guidId = Guid.NewGuid().ToString();
						string svgFile = $"{guidId}.svg";
						wc.DownloadFile($"https://avatars.dicebear.com/api/avataaars/{guidId}.svg", svgFile);
						var svgDoc = SvgDocument.Open<SvgDocument>(svgFile, null);
						Stream stream = new MemoryStream();
						svgDoc.Draw().Save(stream, System.Drawing.Imaging.ImageFormat.Png);


						string avatarUrl = _imagingService.UploadImage(RPGConstants.AvatarFolder, stream);
						character = _rpgService.CreateNewCharacter((IGuildUser)Context.User, avatarUrl);
						_userService.Deduct(Context.User.Id, Context.Guild.Id, RPGConstants.NewCharacterCost);
						await Context.Channel.SendMessageAsync($"Ok I rolled you up a new character! {RPGConstants.NewCharacterCost} Libcoin has been deducted from your account.", embed: _rpgService.BuildCharacterEmbed(character));
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine("Shit broke");
					Console.WriteLine(ex.Message);
				}
			}
			else
			{
				await Context.Channel.SendMessageAsync("Looks like you already have a character.", embed: _rpgService.BuildCharacterEmbed(character));
			}
		}

		[Command("char"), Alias("c")]
		public async Task RetrieveCharacter()
		{
			Character character = _rpgService.GetCharacter((IGuildUser)Context.User);
			if (character == null)
			{
				await Context.Channel.SendMessageAsync($"Sorry friend, you haven't rolled up a character yet. It costs {RPGConstants.NewCharacterCost} Libcoin but it's worth it!");
			}
			else
			{
				await Context.Channel.SendMessageAsync(embed: _rpgService.BuildCharacterEmbed(character));
			}
		}

		[Command("pvp")]
		public async Task TogglePvPStatus()
		{
			_rpgService.ToggleCharacterPvPFlag((IGuildUser) Context.User);
			Character character = _rpgService.GetCharacter((IGuildUser)Context.User);
			if (character.PvPFlagged)
			{
				await Context.Channel.SendMessageAsync($"Ok, I'll let everyone know {character.Name} is ready to rumble!");
			}
			else
			{
				await Context.Channel.SendMessageAsync($"Ok, I'll let everyone know {character.Name} is character of peace. For now.");
			}
		}

		[Command("pvplist"), Alias("saturdaynight","goodfites","fightclub","flist")]
		public async Task ShowPVPEnabledFighters()
		{
			List<Character> pvpCharacters = _rpgService.GetPVPCharacters();
			await Context.Channel.SendMessageAsync(embed: _rpgService.BuildPvPListEmbed(pvpCharacters));
		}

		[Command("challenge"), Alias("fight","chal", "fite")]
		public async Task ChallengeCharacter()
		{

		}
	}
}
