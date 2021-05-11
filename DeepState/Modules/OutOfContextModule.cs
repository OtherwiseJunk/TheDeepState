using DartsDiscordBots.Permissions;
using DartsDiscordBots.Utilities;
using DeepState.Data;
using DeepState.Data.Context;
using DeepState.Service;
using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DeepState.Modules
{
	public class OutOfContextModule : ModuleBase
	{
		private OOCDBContext _DBContext { get; set; }
		private ImagingService _imageService {get;set;}
		private string OOCCaptionFormat = "{0} Originally reported by {1}";

		public OutOfContextModule(OOCDBContext context, ImagingService imageService)
		{
			_DBContext = context;
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

		public void SendRandomOOCItem()
		{
			OOCItem pulledItem = _DBContext.GetRandomRecord();
			IGuildUser reportingUser = Context.Guild.GetUserAsync(pulledItem.ReportingUserId).Result;
			string reportingUsername = reportingUser.Nickname != null ? reportingUser.Nickname : reportingUser.Username;
			//Supports messages originally logged when I was first writing this. We shouldn't attach the image/jpeg;base64, text anymore.
			string base64 = pulledItem.Base64Image.Replace("image/jpeg;base64,", "");

			_ = Context.Channel.SendFileAsync(Converters.GetImageStreamFromBase64(base64), "OOCLibCraft.png", String.Format(OOCCaptionFormat, OOCQuipFormats.GetRandom(), reportingUsername));
		}

		[Command("ooc"), Alias("libcraftmoment"), RequireGuild("698639095940907048")]
		[Summary("Returns a random entry from the databse of base64 image strings.")]
		public async Task RetrieveRandomOutOfContext()
		{
			new Thread(SendRandomOOCItem).Start();			
		}

		[Command("ooclog"), RequireGuild("698639095940907048")]
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
						if (!_DBContext.ImageExists(base64Image))
						{
							_DBContext.AddRecord(Context.Message.Author.Id, base64Image);
							await Context.Channel.SendMessageAsync("Ok, I've logged this as a PATENTED LIBCRAFT MOMENT");
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
