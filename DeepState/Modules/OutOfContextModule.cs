using DeepState.Data.Context;
using DeepState.Service;
using Discord;
using Discord.Commands;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DeepState.Modules
{
	public class OutOfContextModule : ModuleBase
	{
		private OOCDBContext _DBContext { get; set; }
		private ImagingService _imageService {get;set;}

		public OutOfContextModule(OOCDBContext context, ImagingService imageService)
		{
			_DBContext = context;
			_imageService = imageService;
		}

		[Command("ooclog")]
		[Summary("Makes a jackbox poll, and will announce a winner after 5 mintues. User must provide a comma separated list of the jack.")]
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
