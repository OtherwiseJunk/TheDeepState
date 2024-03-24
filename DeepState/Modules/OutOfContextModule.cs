using DartsDiscordBots.Permissions;
using DeepState.Services;
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
using DeepState.Extensions;

namespace DeepState.Modules
{
    public class OutOfContextModule : ModuleBase
    {
        private OOCService _panopticon { get; set; }
        private DartsDiscordBots.Services.ImagingService _imageService { get; set; }
        private string OutOfCOntextFolder = "OutOfContext";
        public static string OutOfContextTitle = "Libcraft Out Of Context: ";

        public OutOfContextModule(OOCService panopticon, DartsDiscordBots.Services.ImagingService imageService)
        {
            _panopticon = panopticon;
            _imageService = imageService;
        }

        public void SendRandomOOCItem(IGuild triggeringGuild, IMessageChannel triggeringChannel)
        {
            OOCItem pulledItem = _panopticon.GetRandomRecord();
            if (pulledItem != null)
            {
                EmbedBuilder embed = _panopticon.BuildOOCEmbed(OutOfContextTitle, triggeringGuild, triggeringChannel, pulledItem);

                _ = triggeringChannel.SendMessageAsync(embed: embed.Build());
            }
        }

        public void SendSpecificOOCItem(IGuild triggeringGuild, IMessageChannel triggeringChannel, int oocId)
        {
            OOCItem pulledItem = _panopticon.GetSpecificRecord(oocId);
            if (pulledItem != null)
            {
                EmbedBuilder embed = _panopticon.BuildOOCEmbed(OutOfContextTitle, triggeringGuild, triggeringChannel, pulledItem);

                _ = triggeringChannel.SendMessageAsync(embed: embed.Build());
            }
            else
            {
                Context.Message.ReplyAsync("My apologies Citizen, I didn't find that record.");
            }
        }

        public void DeleteTriggeringMessage(IMessage message)
        {
            //Wait a minute, then delete triggering message
            Thread.Sleep(60 * 1000);
            _ = message.DeleteAsync();
        }

        [Command("oocremove"),Alias("oocdel","oocdelete"), RequireUserPermission(ChannelPermission.ManageMessages, Group ="privilegedUser"), RequireOwner(Group = "privilegedUser"), RequireGuild(
                new ulong[] {
                    95887290571685888,
                    SharedConstants.LibcraftGuildId
                }
            ),
            RequireChannel(
                new ulong[]  {
                    718986327642734654,
                    SharedConstants.LibcraftOutOfContext,
                    716841087137873920,
                    176357319687405569,
                    701194133074608198,
                    831675528431403039
                }
            )]
        public async Task DeleteOOCItem()
        {
            if (!Context.Message.HasReferencedMessage())
            {
                await Context.Channel.SendMessageAsync(CommandErrorConstants.RequiredMessageReferenceMissingError);
                return;
            }
            IUserMessage messageRepliedTo = await Context.Channel.GetMessageAsync(Context.Message.ReferencedMessage.Id) as IUserMessage;
            if (!messageRepliedTo.IsSentByMe(Context.Client.CurrentUser.Id))
            {
                await Context.Channel.SendMessageAsync(CommandErrorConstants.NotReplyingToMeError);
                return;
            }
            if (!messageRepliedTo.HasSpecificEmbedCount(1))
            {
                await Context.Channel.SendMessageAsync(CommandErrorConstants.EmbedCountError(1));
                return;
            }
            IEmbed embed = messageRepliedTo.Embeds.First();
            if (!embed.Title.Contains(OutOfContextTitle))
            {
                await Context.Channel.SendMessageAsync("That's the wrong type of embed, Citizen. That or you're referencing an older version of the out of context embed.");
                return;
            }

            if (!embed.Footer.HasValue)
            {
                await Context.Channel.SendMessageAsync("That's an outdated version of the out of context embed, Citizen. It doesn't work with this command, my apologizes.");
                return;
            }
            if (_panopticon.DeleteRecord(Int32.Parse(embed.Footer.Value.Text)))
            {
                await Context.Channel.SendMessageAsync("Ok, I've deleted that OOC, Citizen");
            }
            else
            {
                await Context.Channel.SendMessageAsync("Sorry Citizen, I wasn't able to delete that message for some reason. Please try again.");
            }
        }

        [Command("ooc"), Alias("libcraftmoment", "libcraftfacts", "libfacts", "hippofacts"),
            RequireGuild(
                new ulong[] {
                    95887290571685888,
                    SharedConstants.LibcraftGuildId
                }
            ),
            RequireChannel(
                new ulong[]  {
                    718986327642734654,
                    SharedConstants.LibcraftOutOfContext,
                    716841087137873920,
                    176357319687405569,
                    701194133074608198,
                    831675528431403039
                }
            )
        ]
        [Summary("Returns a specific entry from the out of context archives")]
        public async Task RetrieveSpecificOutOfContext(int oocId)
        {
            new Thread(() => { SendSpecificOOCItem(Context.Guild, Context.Channel, oocId); }).Start();
        }

        [Command("ooc"), Alias("libcraftmoment", "libcraftfacts", "libfacts", "hippofacts"),
            RequireGuild(
                new ulong[] {
                    95887290571685888,
                    SharedConstants.LibcraftGuildId
                }
            ),
            RequireChannel(
                new ulong[]  {
                    718986327642734654,
                    SharedConstants.LibcraftOutOfContext,
                    716841087137873920,
                    176357319687405569,
                    701194133074608198,
                    831675528431403039
                }
            )
        ]
        [Summary("Returns a random entry from the databse of base64 image strings.")]
        public async Task RetrieveRandomOutOfContext()
        {
            new Thread(() => { SendRandomOOCItem(Context.Guild, Context.Channel); }).Start();
        }

        [Command("ooclog"),
            RequireGuild(
                new ulong[] {
                    95887290571685888,
                    SharedConstants.LibcraftGuildId
                }
            ),
            RequireChannel(
                new ulong[]  {
                    718986327642734654,
                    SharedConstants.LibcraftOutOfContext,
                    716841087137873920,
                    176357319687405569,
                    701194133074608198,
                    831675528431403039
                }
            )
        ]
        [Summary("Stores the attached image in the message this command is replying to.")]
        public async Task LogOutOfContext()
        {
            if (!Context.Message.HasReferencedMessage())
            {
                await Context.Channel.SendMessageAsync(CommandErrorConstants.RequiredMessageReferenceMissingError);
            }
            IUserMessage messageRepliedTo = await Context.Channel.GetMessageAsync(Context.Message.ReferencedMessage.Id) as IUserMessage;
            if (!messageRepliedTo.HasSpecificAttachmentCount(1))
            {
                await Context.Channel.SendMessageAsync(CommandErrorConstants.AttachmentCountError(1));
            }
            if (!messageRepliedTo.HasMySpecificReaction("📷"))
            {
                await Context.Channel.SendMessageAsync("Sorry, another Citizen has already reported that.");
            }
            using (WebClient client = new WebClient())
            {
                Stream image = new MemoryStream(client.DownloadData(messageRepliedTo.Attachments.First().Url));
                try
                {
                    string url = _imageService.UploadImage(OutOfCOntextFolder, image);
                    if (_panopticon.AddRecord(Context.Message.Author.Id, Context.Guild.Id, url))
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
                catch (Exception ex)
                {
                    await Context.Channel.SendMessageAsync("Sorry Citizen, I failed to encode that image, maybe there's a problem with that file?");
                    Console.WriteLine(ex.Message);
                }
            }
        }

        [Command("occfuckywucky"), RequireOwner()]
        public async Task FixTheFuckyWucky()
        {
            string[] actualUrls = _panopticon.GetAllURLs();
            string[] expectedUrls = _imageService.GetAllFolderUrls("OutOfContext");

            string[] difference = expectedUrls.Where(url => !actualUrls.Contains(url)).ToArray();

            await Context.Message.ReplyAsync($"Identified {difference.Length} missing items.");
            foreach (string url in difference)
            {
                _panopticon.AddRecord(69, SharedConstants.LibcraftGuildId, url);
            }

            await Context.Message.ReplyAsync($"Successfully resubmitted items under userID 69");
        }

        [Command("testEmbed"), RequireOwner()]
        public async Task TestOOCEmbed()
        {
            EmbedBuilder embedBuilder = _panopticon.BuildOOCEmbed(OutOfContextTitle, Context.Guild, Context.Channel, new OOCItem { ItemID = 16809, ReportingUserId = Context.Client.CurrentUser.Id, ImageUrl = "https://pbs.twimg.com/profile_images/1143218135/wtfmini_400x400.png", DateStored = DateTime.Now});
            await Context.Message.ReplyAsync("", embed: embedBuilder.Build());
        }
    }
}
