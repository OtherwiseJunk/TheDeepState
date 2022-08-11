using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;
using DeepState.Constants;
using TraceLd.MineStatSharp;
using DartsDiscordBots.Permissions;
using DeepState.Utilities;
using DartsDiscordBots.Utilities;
using DeepState.Service;
using System.IO;
using SkiaSharp;
using System.Threading;
using System.Net.Http;
using DeepState.Models;
using Newtonsoft.Json;
using System.Globalization;
using Utils = DeepState.Utilities.Utilities;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FFMpegCore.Extend;
using System.Text.RegularExpressions;

namespace DeepState.Modules
{
    public class MalarkeyModule : ModuleBase
    {
        public string ImageURLRegex = @"(http(s?):)([/|.|\w|\s|-])*\.(?:jpg|gif|png|webp)";
        List<string> DeepstateSaysSnark { get; set; } = new()
        {
            "You'd know how many are on, if you'd play",
            "Turn on, tune in, drop out, drop off, switch off, switch on, and explode",
            "If you logged on there would be {ms.currentPlayers +1} friends breaking blocks!",
            "More blocks than your body has room for.",
            "I think I saw a creeper skulking around the base. You might want to log on and take care of that.",
            "You will eat the bugs. You will Mine the Craft.",
            "I bet you 50 libcoin you can't find my base. Here's a hint: There's a lot of restone.",
            "The Phantom Power Party will rise again.",
            "'The Rise and Fall of the Arcadian Empire' is my favorite book",
            "I saw some shady figure by your stuff. Better log on to make sure it's all good.",
            "Citizen, we have always been at war with the River Basin Confederation",
            "I'm selling elytra for 1 iron bar to everyone who logs in in the next 15 minutes and finds my base.",
            "I betcha wanna know how many people are playing. Good news, I know one surefire way to find out.",
            "Touch Blocks",
            "If you don't log on I'm going to call in a raid by your trading hall. I'm sure it'll be fine though right?",
            "Log on or the Gwalms gets it!",
            "Turmoil has engulfed Fantasia. The taxation of trade routes to outlying regions is in disupute. Hoping to resolve the matter with their thousands of diamonds, the greedy Trade Federation has stopped all shipping to Arcadia."
        };
        List<string> ServerIsFullSnark { get; set; } = new()
        {
            "More bodies than your server has room for! Wait shoot."
        };
        List<string> ServerIsntFullSnark { get; set; } = new()
        {
            "come on in the servers fine"
        };
        List<string> ServerIsOfflineSnark { get; set; } = new()
        {
            "Everyone pray to God (Sporf), the server is down."
        };
        [Command("mstatus"), Alias("minecraft", "minecraftstatus", "mcstatus"), RequireGuild(new ulong[] { SharedConstants.LibcraftGuildId, 95887290571685888 })]
        [Summary("Returns a message with a status of Sporf's Minecraft server")]
        public async Task MinecraftStatus(string serverAddress = SporfbaseConstants.ServerAddress, ushort serverPort = SporfbaseConstants.ServerPort)
        {
            MineStat ms = new MineStat(serverAddress, serverPort);
            EmbedBuilder eb = new();
            string serverStatus;
            if (!ms.ServerUp)
            {
                serverStatus = ServerIsOfflineSnark.GetRandom();
            }
            else
            {
                 serverStatus = ms.CurrentPlayers == ms.MaximumPlayers ? ServerIsFullSnark.GetRandom() : ServerIsntFullSnark.GetRandom();
            }

            eb.WithTitle($"{serverAddress} Status");
            eb.AddField("Server Status:", $"{serverStatus}");
            eb.AddField("MotD:", $"{ms.Motd}");
            eb.AddField("Deepstate Says", $"{DeepstateSaysSnark.GetRandom()}");
            await Context.Channel.SendMessageAsync("", false, eb.Build());
        }

        [Command("racisme")]
        public async Task LeRacisme()
        {
            AutoResponse("https://cdn.discordapp.com/attachments/959323818860621885/959325464093138964/gamer_country.mp4");
        }

        [Command("wokemoralists"), Alias("lobsters", "cleanyourroom", "peterson")]
        public async Task WokeMoralists()
        {
            AutoResponse("https://vxtwitter.com/bradenisbased/status/1544448370500161543");
        }

        [Command("darkbrandon"), Alias("letsgobrandon","theonetruebrandon", "darkbrandom")]
        public async Task LetsGoDarkBrandon()
        {
            AutoResponse("https://cdn.discordapp.com/attachments/999910404316733500/1001980262994956309/dark_brandon.mp4");
        }

        [Command("petersonsex"), Alias("lobstersex")]
        public async Task PetersonSex()
        {
            AutoResponse("https://vxtwitter.com/alaning/status/1544546265828139010?s=20&t=jI8Ujl52cbASY2SN6QFyqw");
        }

        [Command("notthistime")]
        [Summary("Not This Time. It's Fiction.")]
        public async Task NotThisTime()
        {
            AutoResponse($"{SharedConstants.JonathanFrakesThatsNotTrue.GetRandom()}");
        }

        [Command("clara"), Alias("sillywoman")]
        [Summary("CLARA!")]
        public async Task Clara()
        {
            AutoResponse("https://cdn.discordapp.com/attachments/931723945416228915/952094761907535872/shut_up_silly_woman.mp4");
        }
        [Command("rodrigo"), Alias("patbomb")]
        [Summary("get pats. Maybe get lots of pats.")]
        public async Task PatBomb()
        {
            new Thread(() =>
            {
                if (Utils.PercentileCheck(25))
                {
                    List<string> headPats = SharedConstants.HeadPats;
                    headPats.Shuffle();
                    foreach (string headPatEmote in headPats)
                    {
                        _ = Context.Message.AddReactionAsync(Emote.Parse(headPatEmote));
                        Thread.Sleep(100);
                    };
                }
                else
                {
                    Context.Message.AddReactionAsync(Emote.Parse(SharedConstants.HeadPats.GetRandom()));
                }
            }).Start();
        }

        [Command("eml"),Alias("notaferret")]
        [Summary("Live EML Reaction")]
        public async Task EMLReaction()
        {
            AutoResponse("https://cacheblasters.nyc3.digitaloceanspaces.com/eml.png");
        }

        [Command("weekend")]
        [Summary("It's The Weekend, Ladies and Gentleman!")]
        public async Task TheWeekend()
        {
            AutoResponse("https://cdn.discordapp.com/attachments/745024703365644320/840383340790939658/theweekend.mp4");
        }

        [Command("crackers")]
        [Summary("That's just how they feel, yanno?")]
        public async Task Crackers()
        {
            AutoResponse("https://vocaroo.com/1bINV12mPOSF");
        }

        [Command("stupidsonofabitch"), Alias("ssoab", "sob", "ssob")]
        [Summary("You're a stupid son of a bitch.")]
        public async Task StupidSonOfABitch()
        {
            AutoResponse("https://cdn.discordapp.com/attachments/855227586212134922/937769372406132756/RPReplay_Final1643652007.mov");
        }

        [Command("2spicy"), Alias("tummytroubles")]
        [Summary("Anton has a weak stomache.")]
        public async Task TummyTroubles()
        {
            AutoResponse("https://media.discordapp.net/attachments/701194133074608198/923110430732324914/album_2021-12-22_00-08-55.gif");
        }

        [Command("walkingdad"), RequireGuild(new ulong[] { SharedConstants.LibcraftGuildId, 95887290571685888 })]
        [Summary("Check in on the server's favorite dad!")]
        public async Task WalkingDad()
        {
            IGuildUser theDad = Context.Guild.GetUserAsync(SharedConstants.TheDad, CacheMode.AllowDownload).Result;
            string name = theDad.Nickname ?? theDad.Username;
            EmbedBuilder embed = new EmbedBuilder();
            embed.WithTitle($"Time to check in on {name}!");
            embed.WithImageUrl(theDad.GetAvatarUrl());
            embed.AddField($"{name}", "Status: Dad");

            _ = Context.Channel.SendMessageAsync(embed: embed.Build());
        }

        [Command("wilhelm")]
        [Summary("Adds the wilhelm scream sound to an image. As r00t IDK.")]
        public async Task WilhelmScream()
        {
            string attachmentUrl = null;
            if(Context.Message.Attachments.Count == 1)
            {
                attachmentUrl = Context.Message.Attachments.First().Url;                
            }
            else if(Regex.IsMatch(Context.Message.Content, ImageURLRegex)){
                attachmentUrl = Regex.Match(Context.Message.Content, ImageURLRegex).Groups.Values.First().Value;
            }
            else if (Context.Message.ReferencedMessage != null)
            {
                IMessage messageRepliedTo = await Context.Channel.GetMessageAsync(Context.Message.ReferencedMessage.Id);
                if (messageRepliedTo.Attachments.Count == 1)
                {
                    attachmentUrl = messageRepliedTo.Attachments.First().Url;
                }
                Match match = Regex.Match(messageRepliedTo.Content, ImageURLRegex);
                if (match.Success)
                {
                    attachmentUrl = match.Value;
                }
            }
            else
            {
                _ = Context.Channel.SendMessageAsync("No image attachment found.");
            }

            if(attachmentUrl != null)
            {
                new Thread(async () =>
                {
                    using (HttpClient httpClient = new HttpClient())
                    {
                        using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, attachmentUrl))
                        {
                            using (HttpResponseMessage response = await httpClient.SendAsync(request))
                            {
                                var image = System.Drawing.Image.FromStream(response.Content.ReadAsStream());
                                int height = image.Height % 2 == 0 ? image.Height : image.Height + 1;
                                int width = image.Width % 2 == 0 ? image.Width : image.Width + 1;
                                if (image.Height % 2 != 0 || image.Width % 2 != 0)
                                {
                                    image = (System.Drawing.Image)new Bitmap(image, new Size(width, height));
                                }
                                image.AddAudio("./wilhelm.ogg", "output.mp4");
                                _ = Context.Channel.SendFileAsync("./output.mp4");
                            }
                        }
                    }
                }).Start();                            
            }
        }

        [Command("imgonnacome")]
        public void ImGonnaCome()
        {
            AutoResponse("https://youtu.be/NRCf3KUEVyw");
        }

        [Command("donotcome")]
        public void DoNotCome()
        {
            AutoResponse("https://media.tenor.com/images/a7b5e8c66c4214d3f04f3726a5475a65/tenor.gif");
        }

        [Command("portal")]
        [Summary("Opens a portal to another channel. Generally used for off-topic discussion in a channel.")]
        public async Task OpenAPortal([Summary("A # link to the channel to open the portal to.")] ITextChannel portalTargetChannel)
        {

            string username = (Context.User as IGuildUser).Nickname ?? Context.User.Username;
            IUserMessage targetChannelMessage = await portalTargetChannel.SendMessageAsync(PortalConstants.PortalSummoningText.GetRandom());
            IUserMessage sourceChannelMessage = await Context.Channel.SendMessageAsync(PortalConstants.PortalSummoningText.GetRandom());
            _ = targetChannelMessage.ModifyAsync(msg =>
             {
                 msg.Content = "";
                 msg.Embed = PortalUtilities.BuildPortalEmbed(username, Context.Channel.Name, sourceChannelMessage.GetJumpUrl(), true);
             });

            _ = sourceChannelMessage.ModifyAsync(msg =>
            {
                msg.Content = "";
                msg.Embed = PortalUtilities.BuildPortalEmbed(username, portalTargetChannel.Name, targetChannelMessage.GetJumpUrl(), false);
            });
        }

        [Command("imfromarizona")]
        public async Task ImFromArizona()
        {
            AutoResponse("https://cdn.discordapp.com/attachments/701194133074608198/936088371485364255/video0.mov");
        }

        [Command("antoncheckin"), Alias("anton")]
        public async Task AntonCheckIn()
        {
            AutoResponse("https://cdn.discordapp.com/attachments/701194133074608198/939019260754272296/LiveAntonReaction.png");
        }

		[Command("nationaldebt"), Alias("debt", "nd")]
		[Summary("Query the official .gov API to get the current national debt.")]
		public async Task GetNationalDebt()
		{
			using (HttpClient client = new HttpClient())
			{
				Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
				NationalDebtData nationalDebtData = JsonConvert.DeserializeObject<NationalDebtData>(client.GetAsync("https://www.treasurydirect.gov/NP_WS/debt/current?format=json").Result.Content.ReadAsStringAsync().Result);
				EmbedBuilder embed = new EmbedBuilder();
				embed.Title = "Good Morning Senator, here's some bullshit";
				embed.AddField("Effective Date", nationalDebtData.effectiveDate);
				embed.AddField("Total Debt", nationalDebtData.totalDebt.ToString("C"));
				embed.AddField("Public Debt", nationalDebtData.publicDebt.ToString("C"));
				embed.AddField("Government Holdings", nationalDebtData.governmentHoldings.ToString("C"));

                await Context.Channel.SendMessageAsync(embed: embed.Build());
            }
        }

		[Command("ididitlikethis"), Alias("likethis")]
		[Summary("That's how I did it yanno, just like this.")]
		public async Task IDidItLikeThis()
		{
			if(Context.Message.Author.Id == SharedConstants.TheDungeonMaster)
			{
				_ = Context.Message.ReplyAsync("https://y.yarn.co/5bf8015a-95c4-46d0-9d50-3da21e0d8357_text.gif");
			}
			else
			{
				_ = Context.Message.ReplyAsync("https://cdn.discordapp.com/attachments/883466654443507773/904792562777354280/video0.mov");
			}
		}

        public void AutoResponse(string response)
        {
            Context.Channel.SendMessageAsync(response, messageReference: Context.Message.Reference);
        }
    }
}
