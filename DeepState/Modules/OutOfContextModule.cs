using DartsDiscordBots.Permissions;
using DartsDiscordBots.Utilities;
using DeepState.Data.Models;
using DeepState.Data.Context;
using DeepState.Service;
using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DeepState.Data.Services;

namespace DeepState.Modules
{
	public class OutOfContextModule : ModuleBase
	{
		private OutOfContextService _OOCService { get; set; }
		private ImagingService _imageService { get; set; }
		private string OOCCaptionFormat = "{0} Originally reported by {1}";

		public OutOfContextModule(OutOfContextService oocService, ImagingService imageService)
		{
			_OOCService = oocService;
			_imageService = imageService;
		}

		private List<string> OOCQuipFormats = new List<string>
		{
			"Another Libcraft Banger.",
			"Still can't believe they said this...",
			"SMDH, really?",
			"Ok, friend, whatever you say.",
			"Ban them tbh.",
			"A Libcraft Classic.",
			"This awful take brought to you by Libcraft.",
			"They're a genius!",
			"Libcraft actually believes this.",
			"Yikes Sweety, let's unpack this...",
			"Yikes Sweaty, let's unpack this..."
		};

		public void SendRandomOOCItem(IGuild triggeringGuild, IMessageChannel triggeringChannel)
		{
			OOCItem pulledItem = _OOCService.GetRandomRecord();
			IGuildUser reportingUser = triggeringGuild.GetUserAsync(pulledItem.ReportingUserId, CacheMode.AllowDownload).Result;
			string reportingUsername;
			if (reportingUser != null)
			{
				reportingUsername = reportingUser.Nickname != null ? reportingUser.Nickname : reportingUser.Username;
			}
			else
			{
				reportingUsername = "A mysterious stranger, who is probably hot";
			}
			//Supports messages originally logged when I was first writing this. We shouldn't attach the image/jpeg;base64, text anymore.
			string base64 = pulledItem.Base64Image.Replace("image/jpeg;base64,", "");

			_ = triggeringChannel.SendFileAsync(Converters.GetImageStreamFromBase64(base64), "OOCLibCraft.png", String.Format(OOCCaptionFormat, OOCQuipFormats.GetRandom(), reportingUsername));
		}

		public void DeleteTriggeringMessage(IMessage message)
		{
			//Wait a minute, then delete triggering message
			Thread.Sleep(60 * 1000);
			_ = message.DeleteAsync();
		}

		[Command("oocdelete"), Alias("oocdel"), RequireUserPermission(ChannelPermission.ManageMessages), RequireGuild(new ulong[] { 698639095940907048, 95887290571685888 })]
		[Summary("Allows the mods to delete the OOCRecord that the triggering message is responding to.")]
		public async Task DeleteOOCItem()
		{
			if (Context.Message.ReferencedMessage != null)
			{
				IMessage messageRepliedTo = await Context.Channel.GetMessageAsync(Context.Message.ReferencedMessage.Id);
				//We only want to log messages with a single identifiable image
				if (messageRepliedTo.Attachments.Count == 1)
				{
					try
					{
						string base64Image = await _imageService.GetBase64ImageFromURL(messageRepliedTo.Attachments.First().Url);
						if (_OOCService.ImageExists(base64Image))
						{
							_OOCService.DeleteImage(base64Image);
							await Context.Message.AddReactionAsync(new Emoji("✅"));
							new Thread(() => { DeleteTriggeringMessage(Context.Message); }).Start();
						}
						else
						{
							await Context.Channel.SendMessageAsync("Sorry, looks like that image doesn't exist, friend.");
						}
					}
					catch (Exception ex)
					{
						await Context.Channel.SendMessageAsync("Sorry, I failed to encode that image, maybe there's a problem with that file?");
						Console.WriteLine(ex.Message);
					}
				}
				else
				{
					await Context.Channel.SendMessageAsync($"There's {messageRepliedTo.Attachments.Count} attachments on this message, I need a message with exactly 1!");
				}
			}
			else
			{
				await Context.Channel.SendMessageAsync("Sorry, I don't see a message you're replying to here...");
			}
		}

		[Command("ooc"), Alias("libcraftmoment"), RequireGuild(new ulong[] { 698639095940907048, 95887290571685888 }), RequireChannel(new ulong[] { 718986327642734654, 777400598789095445, 716841087137873920, 176357319687405569, 701194133074608198, 831675528431403039 })]
		[Summary("Returns a random entry from the databse of base64 image strings.")]
		public async Task RetrieveRandomOutOfContext()
		{
			new Thread(() => { SendRandomOOCItem(Context.Guild, Context.Channel); }).Start();			
		}

		[Command("ooclog"), RequireGuild(new ulong[] { 698639095940907048, 95887290571685888 }), RequireChannel(new ulong[] { 718986327642734654, 777400598789095445, 716841087137873920, 176357319687405569, 701194133074608198, 831675528431403039 })]
		[Summary("Logs the base64 string of the image in the message this command is responding to.")]
		public async Task LogOutOfContext()
		{
			if(Context.Message.ReferencedMessage != null)
			{
				IMessage messageRepliedTo = await Context.Channel.GetMessageAsync(Context.Message.ReferencedMessage.Id);
				//We only want to log messages with a single identifiable image
				if (messageRepliedTo.Attachments.Count == 1)
				{
					try
					{
						string base64Image = await _imageService.GetBase64ImageFromURL(messageRepliedTo.Attachments.First().Url);
						if (!_OOCService.ImageExists(base64Image))
						{
							_OOCService.AddRecord(Context.Message.Author.Id, base64Image);
							await Context.Message.AddReactionAsync(new Emoji("✅"));
							new Thread(() => { DeleteTriggeringMessage(Context.Message); }).Start();
						}
						else
						{
							await Context.Channel.SendMessageAsync("Sorry, looks like someone may have already logged that one, friend.");
						}
					}
					catch(Exception ex)
					{
						await Context.Channel.SendMessageAsync("Sorry, I failed to encode that image, maybe there's a problem with that file?");
						Console.WriteLine(ex.Message);
					}					
				}
				else
				{
					await Context.Channel.SendMessageAsync($"There's {messageRepliedTo.Attachments.Count} attachments on this message, I need a message with exactly 1!");
				}
			}
			else
			{
				await Context.Channel.SendMessageAsync("Sorry, I don't see a message you're replying to here...");
			}
		}
	}
}
