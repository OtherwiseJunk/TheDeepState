using DartsDiscordBots.Permissions;
using DeepState.Service;
using Discord;
using Discord.Commands;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using DeepState.Constants;
using DartsDiscordBots.Services;
using Panopticon.Shared.Models;

namespace DeepState.Modules
{
	public class OutOfContextModule : ModuleBase
	{
		private OOCService _panopticon { get; set; }
		private ImagingService _imageService { get; set; }
		private string OutOfCOntextFolder = "OutOfContext";

		public OutOfContextModule(OOCService panopticon, ImagingService imageService)
		{
			_panopticon = panopticon;
			_imageService = imageService;
		}

		

		public void SendRandomOOCItem(IGuild triggeringGuild, IMessageChannel triggeringChannel)
		{
			OOCItem pulledItem = _panopticon.GetRandomRecord();
			if (pulledItem != null)
            {
				EmbedBuilder embed = _panopticon.BuildOOCEmbed(triggeringGuild, triggeringChannel, pulledItem);

				_ = triggeringChannel.SendMessageAsync(embed: embed.Build());
			}			
		}

		public void DeleteTriggeringMessage(IMessage message)
		{
			//Wait a minute, then delete triggering message
			Thread.Sleep(60 * 1000);
			_ = message.DeleteAsync();
		}

		[Command("ooc"), Alias("libcraftmoment", "libcraftfacts","libfacts","hippofacts"), RequireGuild(new ulong[] { SharedConstants.LibcraftGuildId, 95887290571685888 }), RequireChannel(new ulong[] { 718986327642734654, SharedConstants.LibcraftOutOfContext, 716841087137873920, 176357319687405569, 701194133074608198, 831675528431403039 })]
		[Summary("Returns a random entry from the databse of base64 image strings.")]
		public async Task RetrieveRandomOutOfContext()
		{
			new Thread(() => { SendRandomOOCItem(Context.Guild, Context.Channel); }).Start();			
		}

		[Command("ooclog"), RequireGuild(new ulong[] { SharedConstants.LibcraftGuildId, 95887290571685888 }), RequireChannel(new ulong[] { 718986327642734654, SharedConstants.LibcraftOutOfContext, 716841087137873920, 176357319687405569, 701194133074608198, 831675528431403039 })]
		[Summary("Stores the attached image in the message this command is replying to.")]
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
						WebClient client = new WebClient();
						Stream image = new MemoryStream(client.DownloadData(messageRepliedTo.Attachments.First().Url));
						if (messageRepliedTo.Reactions.Where(r => r.Key.Name == "📷" && r.Value.IsMe).Count() == 0)
						{
							string url = _imageService.UploadImage(OutOfCOntextFolder, image);							
							if(_panopticon.AddRecord(Context.Message.Author.Id, Context.Guild.Id, url))
                            {
								await Context.Message.AddReactionAsync(new Emoji("✅"));
								_ = messageRepliedTo.AddReactionAsync(new Emoji("📷"));
							}
                            else
                            {
								await Context.Message.ReplyAsync("Sorry, something went wrong when attempting to log that message.");
                            }
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
