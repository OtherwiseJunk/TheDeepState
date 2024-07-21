using DartsDiscordBots.Modules.Help;
using DartsDiscordBots.Modules.Bot;
using DartsDiscordBots.Modules.Indecision;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using DeepState.Constants;
using DeepState.Modules;
using DartsDiscordBots.Modules.Help.Interfaces;
using DeepState.Models;
using DartsDiscordBots.Modules.Bot.Interfaces;
using DartsDiscordBots.Modules.Chat;
using DartsDiscordBots.Services.Interfaces;
using DartsDiscordBots.Services;
using DeepState.Data.Context;
using DeepState.Services;
using System.Threading;
using DeepState.Data.Services;
using DeepState.Handlers;
using Utils = DeepState.Utilities.Utilities;
using DeepState.Utilities;
using OMH = DartsDiscordBots.Handlers.OnMessageHandlers;
using ORH = DartsDiscordBots.Handlers.OnReactHandlers;
using OEH = DartsDiscordBots.Utilities.EventUtilities;
using FluentScheduler;
using Serilog;
using DartsDiscordBots.Modules.Jackbox.Interfaces;
using DartsDiscordBots.Modules.NFT;
using DartsDiscordBots.Utilities;
using System.Collections.Generic;
using DartsDiscordBots.Modules.ServerManagement;
using DartsDiscordBots.Modules.ServerManagement.Interfaces;
using Victoria;
using DartsDiscordBots.Modules.Audio;
using DartsDiscordBots.Modules.LockedTomb;
using System.Linq;
using System.IO;
using System.Net;
using DeepState.Models.SlashCommands;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using DartsDiscordBots.SlashCommandModules.ToDoSlashCommandModule.Service;
using DartsDiscordBots.SlashCommandModules.ToDoSlashCommandModule.Context;
using DartsDiscordBots.SlashCommandModules.ToDoSlashCommandModule.Interfaces;
using DartsDiscordBots.SlashCommandModules.ToDoSlashCommandModule;
using System.Web;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using DeepState.Extensions;
using System.Threading.Channels;
using NLog.Attributes;

namespace DeepState
{
    public class Program
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly IServiceProvider _services;
        private Program()
        {
            _client = new DiscordSocketClient(new DiscordSocketConfig()
            {
                GatewayIntents = GatewayIntents.All
            });
            _services = ConfigureServices();
            _commands = new CommandService();
            _client.Log += Log;
            _commands.Log += Log;
            _client.ReactionAdded += OnReact;
            JobManager.Initialize();
            JobManager.AddJob(() => HungerGameUtilities.DailyEvent((HungerGamesDataService)_services.GetService(typeof(HungerGamesDataService)), (UserRecordsService)_services.GetService(typeof(UserRecordsService)), _client), s => s.ToRunEvery(1).Days().At(8, 0));
            JobManager.AddJob(() => ((RPGService)_services.GetService(typeof(RPGService))).LongRest(), s => s.ToRunEvery(1).Days().At(8, 0));
            JobManager.AddJob(async () =>
            {
                Console.WriteLine("Attempting an automatic OOC post.");
                OOCService ooc = _services.GetService(typeof(OOCService)) as OOCService;
                Console.WriteLine($"OOC Service null? {ooc == null}");
                IGuild libcraft = _client.GetGuild(SharedConstants.LibcraftGuildId);
                IMessageChannel oocChannel = await libcraft.GetChannelAsync(SharedConstants.LibcraftOutOfContext) as IMessageChannel;
                Console.WriteLine($"Channel null? {oocChannel == null}");
                EmbedBuilder embed = ooc.BuildOOCEmbed(OutOfContextModule.OutOfContextTitle, libcraft, oocChannel, ooc.GetRandomRecord());
                Console.WriteLine($"Embed null? {embed == null}");

                _ = oocChannel.SendMessageAsync("Heard from a reliable source that you're jonesing for some OOC. I gotchu.", embed: embed.Build());
            }, s => s.ToRunEvery(2).Hours().At(0));
            JobManager.AddJob(async () =>
            {
                Console.WriteLine("Checking if it's a notable day.");
                DateTime now = DateTime.Now;
                string ddMM = now.ToString("ddMM");
                Console.WriteLine(ddMM);
                try
                {
                    var libcraftCalendarChannel = _client.GetChannel(893391224092885094) as ITextChannel;
                    switch (ddMM)
                    {
                        case "1405":
                            await libcraftCalendarChannel.ModifyTextChannel(
                                "national-chirp-like-a-penguin-while-holding-ice-cubes-day-(observance)",
                                "Actual holiday is on October 2nd. This is the observance of National Chirp Like a Penguin While Holding Ice Cubes Day");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    while (ex.InnerException != null)
                    {
                        ex = ex.InnerException;
                        Console.WriteLine(ex.Message);
                    }
                }
            }, s => s.ToRunEvery(1).Days().At(0, 35));
        }

        public async Task<string> CursedCheck(SocketTextChannel channel)
        {
            Console.WriteLine("Initializing Cursed Check...");
            Console.WriteLine("Channel found for cursed check");
            var messages = await channel.GetMessagesAsync(10000).FlattenAsync();
            int messageCount = messages.Count();
            Console.WriteLine($"Checking {messageCount} messages...");
            int failingMessageCount = 0;
            foreach (var message in messages)
            {
                Dictionary<char, int> charFrequency = new Dictionary<char, int>();
                var messageArray = message.Content.ToLower().ToCharArray();
                foreach (var c in messageArray)
                {
                    if (charFrequency.ContainsKey(c))
                    {
                        charFrequency[c]++;
                    }
                    else
                    {
                        charFrequency[c] = 1;
                    }
                }
                bool containsTargetLetters = charFrequency.ContainsKey('p') && charFrequency.ContainsKey('r') && charFrequency.ContainsKey('e') && charFrequency.ContainsKey('g') && charFrequency.ContainsKey('s');
                if (containsTargetLetters && charFrequency['p'] >= 2 && charFrequency['r'] >= 2 && charFrequency['e'] >= 2 && charFrequency['g'] >= 2 && charFrequency['s'] >= 1)
                {
                    failingMessageCount++;
                }

            }

            return $"Of the last {messageCount} messages, {failingMessageCount} failed our cursed check.";
        }
        public static void Main(string[] args)
    => new Program().MainAsync().GetAwaiter().GetResult();
        public async Task MainAsync()
        {
            Console.WriteLine($"DEEPSTATE has been INITIALIZED");
            Console.WriteLine($"{Environment.GetEnvironmentVariable("DATABASE")}");

            await InstallCommandsAsync();

            string token = Environment.GetEnvironmentVariable("DEEPSTATE");
            Console.WriteLine("Attempting to retrieve bot token from Environment...");

            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("Please provide the bot token:");
                token = Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Success!");
            }

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            new Thread(() => _ = LibcoinUtilities.LibcraftCoinCheck(_services.GetService<UserRecordsService>())).Start();
            new Thread(() => _ = LibcoinUtilities.LibcoinReactionChecker(_services.GetService<UserRecordsService>())).Start();

#if !DEBUG
            new Thread(() => JackboxUtilities.EnsureDefaultGamesExist(_services.GetService<JackboxContext>())).Start();
#endif
            // Block this task until the program is closed.
            await Task.Delay(-1);
        }
        private static IServiceProvider ConfigureServices()
        {
            var log = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
            string lavalinkpassword = Environment.GetEnvironmentVariable("LAVALINKPW");

            //We don't have any services currently for DI
            //but once we do this is where we would add them.
            var map = new ServiceCollection()
                .AddSingleton<ILogger>(log);
            map.AddSingleton(new DiscordSocketClient(new DiscordSocketConfig()
            {
                GatewayIntents = GatewayIntents.All
            }))
               .AddLavaNode(node =>
                {
                    node.Hostname = "lavalink.cacheblasters.com";
                    node.Port = 2333;
                    node.Authorization = lavalinkpassword;
                    node.SelfDeaf = true;
                });

            map.AddSingleton<IHelpConfig, HelpConfig>()
                .AddSingleton<IBotInformation, BotInformation>()
                .AddSingleton<IMessageReliabilityService, MessageReliabilityService>()
                .AddSingleton<IJackboxService, JackboxService>()
                .AddSingleton(new DartsDiscordBots.Services.ImagingService(Environment.GetEnvironmentVariable("DOPUBLIC"),
                    Environment.GetEnvironmentVariable("DOSECRET"),
                    Environment.GetEnvironmentVariable("DOURL"),
                    Environment.GetEnvironmentVariable("DOBUCKET"))
                )
                .AddSingleton<HungerGamesDataService>()
                .AddSingleton<UserRecordsService>()
                .AddSingleton<ModTeamRequestService>()
                .AddSingleton<RPGService>()
                .AddSingleton<FFMPEGService>()
                .AddSingleton<PanopticonService>()
                .AddSingleton<OOCService>()
                .AddSingleton<FeedbackService>()
                .AddSingleton<AudioService>()
                .AddSingleton<BestOfService>()
                .AddSingleton<IServerManagmentService, ServerManagementService>()
                .AddSingleton<RandomCharacterImageService>()
                .AddSingleton<IToDoService, ToDoService>()
                .AddSingleton<ToDoSlashCommandModule>()
                .AddSingleton<WaffleHouseApocalypseService>()
                .AddSingleton<WaffleHouseApocolypseModule>()
                .AddSingleton<HighlightService>()
                .AddDbContext<ToDoContext>()
                .AddDbContext<GuildUserRecordContext>()
                .AddDbContext<HungerGamesContext>()
                .AddDbContextFactory<GuildUserRecordContext>()
                .AddDbContextFactory<HungerGamesContext>()
                .AddDbContextFactory<ModTeamRequestContext>()
                .AddDbContextFactory<ToDoContext>()
                .AddDbContextFactory<HighlightContext>()
#if !DEBUG
                .AddDbContextFactory<JackboxContext>()
#endif
                .AddDbContextFactory<RPGContext>()
                .AddDbContextFactory<BestOfContext>();




            map.AddHttpClient<PanopticonService>();
            map.AddHttpClient<FFMPEGService>();

            var services = map.BuildServiceProvider();

            return services;
        }
        private Task Log(LogMessage msg)
        {
            Console.WriteLine($"[{DateTime.Now.ToString("g")}] ({msg.Source}): {msg.Message}");
            return Task.CompletedTask;
        }
        public async Task InstallCommandsAsync()
        {

            await _commands.AddModuleAsync<MalarkeyModule>(_services);
            await _commands.AddModuleAsync<HelpModule>(_services);
            await _commands.AddModuleAsync<IndecisionModule>(_services);
            await _commands.AddModuleAsync<BotModule>(_services);
            await _commands.AddModuleAsync<ChatModule>(_services);
            await _commands.AddModuleAsync<OutOfContextModule>(_services);
            await _commands.AddModuleAsync<UserRecordsModule>(_services);
            await _commands.AddModuleAsync<HungerGamesModule>(_services);
            await _commands.AddModuleAsync<ModTeamRequestModule>(_services);
            await _commands.AddModuleAsync<RPGModule>(_services);
            await _commands.AddModuleAsync<NFTModule>(_services);
            await _commands.AddModuleAsync<FeedbackModule>(_services);
            await _commands.AddModuleAsync<ServerManagementModule>(_services);
            await _commands.AddModuleAsync<AudioModule>(_services);
            await _commands.AddModuleAsync<LockedTombModule>(_services);
            await _commands.AddModuleAsync<CharacterGeneratorModule>(_services);
            await _commands.AddModuleAsync<HighlightModule>(_services);
            await _commands.AddModuleAsync<EmojiMonetizationModule>(_services);

#if !DEBUG
			await _commands.AddModuleAsync<JackboxModule>(_services);
#endif
            _client.MessageReceived += (async (SocketMessage messageParam) =>
            {
                try
                {
                    if (messageParam.Channel.Id != SharedConstants.SelfCareChannelId)
                    {
                        _ = OMH.HandleCommandWithSummaryOnError(messageParam, new CommandContext(_client, (SocketUserMessage)messageParam), _commands, _services, BotProperties.CommandPrefix, false);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception from the DDB Command Handler:");
                    Console.WriteLine(ex.Message);
                    while (ex.InnerException != null)
                    {
                        ex = ex.InnerException;
                        Console.WriteLine("Inner Exception: ");
                        Console.WriteLine(ex.Message);
                    }
                }
            });
            _client.GuildScheduledEventCreated += OnEventCreated;
            _client.MessageReceived += OnMessage;
            _client.ButtonExecuted += OnButtonClicked;
            _client.Ready += InstallSlashCommands;
            _client.SlashCommandExecuted += HandleSlashCommands;
            _client.MessageUpdated += OnEdit;
            _client.GuildMemberUpdated += OnGuildUserUpdated;
            _client.Disconnected += OnDisconnected;

            new Thread(() =>
            {
                ToDoSlashCommandModule toDoModule = _services.GetService<ToDoSlashCommandModule>();
                WaffleHouseApocolypseModule waffleHouse = _services.GetService<WaffleHouseApocolypseModule>();
                _client.Ready += async () =>
                {
                    new Thread(async () =>
                    {
                        await waffleHouse.InstallModuleSlashCommands(null, _client);
                        await toDoModule.InstallModuleSlashCommands(null, _client);
                    }).Start();
                };
                _client.SlashCommandExecuted += toDoModule.HandleSocketSlashCommand;
                _client.SlashCommandExecuted += waffleHouse.HandleSocketSlashCommand;
            }).Start();


        }

        private async Task OnDisconnected(Exception exception)
        {
            Console.WriteLine("Got Disconnected 😭");
            Console.WriteLine(exception.Message);
        }

        private async Task OnGuildUserUpdated(Cacheable<SocketGuildUser, ulong> cacheable, SocketGuildUser userPostUpdate)
        {
            if (OnMessageHandlers.IsPreggers(userPostUpdate.DisplayName))
            {
                await userPostUpdate.ModifyAsync((GuildUserProperties user) =>
                {
                    user.Nickname = "idiot";
                });
            }
        }

        private async void CheckDatEmbed()
        {
            var libcraft = _client.GetGuild(698639095940907048);
            ITextChannel oocChannel = libcraft.GetChannel(777400598789095445) as ITextChannel;
            var message = await oocChannel.GetMessageAsync(1140102426515480647);
            message.Embeds.ForEach((IEmbed embed) =>
            {
                Console.WriteLine("Embed from Libcraft OOC");
                Console.WriteLine(embed.Image);
            });
        }
        private async Task OnEdit(Cacheable<IMessage, ulong> cacheableMessage, SocketMessage message, ISocketMessageChannel channel)
        {
            new Thread(async () => { if (message != null && message.Content != null) { await OnMessageHandlers.DeletePreggersMessage(message); } }).Start();
        }

        private string GetReadyToLearn(string input, bool isFrench = false)
        {
            string response;
            string thingToLearn = 

            string memeUrl = isFrench ? $"Preparez--vous_a_apprendre_{thingToLearn}_mon_pote!.png" : $"get_ready_to_learn_{thingToLearn}_buddy..png";

            if (OnMessageHandlers.IsPreggers(thingToLearn))
            {
                response = "https://cacheblasters.nyc3.cdn.digitaloceanspaces.com/YouDidThis.png";
            }
            else
            {
                response = $"https://api.memegen.link/images/custom/_/{memeUrl}?background=https://cacheblasters.nyc3.cdn.digitaloceanspaces.com/Get_Ready_to_Learn.jpg";
            }

            return response;
        }

        private string SanitizeMemeAPIUrl(string url)
        {
            return url.Replace("_", "__")
                        .Replace("-", "--")
                        .Replace("/", "~s")
                        .Replace("?", "~q")
                        .Replace("&", "~a")
                        .Replace("%", "~p")
                        .Replace("#", "~h")
                        .Replace("\\", "~b")
                        .Replace("<", "~l")
                        .Replace(">", "~g")
                        .Replace("\"", "''")
                        .Replace(' ', '_');
        }

        private string GetThingSheProsecuted(string input)
        {
            string response;
            string thingToProsecute = SanitizeMemeAPIUrl(input);

            string memeUrl = $"she_prosecuted_{{thingToProsecute}}.png?background=https://i0.wp.com/calmatters.org/wp-content/uploads/2024/07/071024-WIDE-Kamala-Harris-REUTERS-CM-01.jpg";

            if (OnMessageHandlers.IsPreggers(thingToProsecute))
            {
                response = "https://cacheblasters.nyc3.cdn.digitaloceanspaces.com/YouDidThis.png";
            }
            else
            {
                response = $"https://api.memegen.link/images/custom/_/{memeUrl}?background=https://i0.wp.com/calmatters.org/wp-content/uploads/2024/07/071024-WIDE-Kamala-Harris-REUTERS-CM-01.jpg";
            }

            return response;
        }

        private async Task HandleSlashCommands(SocketSlashCommand command)
        {
            string response = null;
            Embed embed = null;
            switch (command.CommandName)
            {
                case SlashCommands.LeRacisme:
                    response = "https://cdn.discordapp.com/attachments/959323818860621885/959325464093138964/gamer_country.mp4";
                    break;
                case SlashCommands.WokeMoralists:
                    response = "https://vxtwitter.com/bradenisbased/status/1544448370500161543";
                    break;
                case SlashCommands.DarkBrandon:
                    response = "https://cdn.discordapp.com/attachments/999910404316733500/1001980262994956309/dark_brandon.mp4";
                    break;
                case SlashCommands.PetersonSex:
                    response = "https://vxtwitter.com/alaning/status/1544546265828139010?s=20&t=jI8Ujl52cbASY2SN6QFyqw";
                    break;
                case SlashCommands.NotThisTime:
                    response = SharedConstants.JonathanFrakesThatsNotTrue.GetRandom();
                    break;
                case SlashCommands.Clara:
                    response = "https://cdn.discordapp.com/attachments/931723945416228915/952094761907535872/shut_up_silly_woman.mp4";
                    break;
                case SlashCommands.EML:
                    response = "https://cacheblasters.nyc3.digitaloceanspaces.com/eml.png";
                    break;
                case SlashCommands.TheWeekend:
                    response = "https://cdn.discordapp.com/attachments/745024703365644320/840383340790939658/theweekend.mp4";
                    break;
                case SlashCommands.Crackers:
                    response = "https://vocaroo.com/1bINV12mPOSF";
                    break;
                case SlashCommands.StupidSonOfAbitch:
                    response = "https://cdn.discordapp.com/attachments/855227586212134922/937769372406132756/RPReplay_Final1643652007.mov";
                    break;
                case SlashCommands.TooSpicy:
                    response = "https://media.discordapp.net/attachments/701194133074608198/923110430732324914/album_2021-12-22_00-08-55.gif";
                    break;
                case SlashCommands.ImGonnaCome:
                    response = "https://cacheblasters.nyc3.cdn.digitaloceanspaces.com/TrumpGonnaCome.mp4";
                    break;
                case SlashCommands.DoNotCome:
                    response = "https://media.tenor.com/images/a7b5e8c66c4214d3f04f3726a5475a65/tenor.gif";
                    break;
                case SlashCommands.ImFromArizona:
                    response = "https://cdn.discordapp.com/attachments/701194133074608198/936088371485364255/video0.mov";
                    break;
                case SlashCommands.AntonCheckin:
                    response = "https://cdn.discordapp.com/attachments/701194133074608198/939019260754272296/LiveAntonReaction.png";
                    break;
                case SlashCommands.IDidEverythingRight:
                    response = "https://cdn.discordapp.com/attachments/883466654443507773/1118376851329536010/I_Did_Everything_Right_And_They_Indicted_Me.mp4";
                    break;
                case SlashCommands.TrumpMugshot:
                    EmbedBuilder builder = new();
                    builder.Title = "Ladies, Gentlemen, and Friends Beyond The Binary, We Gottem!";
                    builder.ImageUrl = "https://cacheblasters.nyc3.cdn.digitaloceanspaces.com/TrumpMugshot2.webp";
                    embed = builder.Build();
                    break;
                case SlashCommands.Learn:
                    response = GetReadyToLearn((string)command.Data.Options.First().Value);
                    break;
                case SlashCommands.Apprendre:
                    response = GetReadyToLearn((string)command.Data.Options.First().Value, isFrench: true);
                    break;
                case SlashCommands.Prosecuted:
                    response = GetThingSheProsecuted((string)command.Data.Options.First().Value);
                    break;
            }
            if (response != null)
            {
                _ = command.RespondAsync(response);
            }
            if (embed != null)
            {
                _ = command.RespondAsync(embed: embed);
            }
        }

        private async Task InstallSlashCommands()
        {
            new Thread(async () =>
            {
                try
                {
                    Console.WriteLine("--- Starting installation of slash commands ---");
                    foreach (var item in SlashCommands.SlashCommandsToInstall)
                    {
                        IGuild guild = _client.GetGuild(item.Key);
                        if (guild != null && guild.Id == SharedConstants.LibcraftGuildId)
                        {
                            IGuild libcraft = _client.GetGuild(SharedConstants.LibcraftGuildId);
                            var libcraftCommands = libcraft.GetApplicationCommandsAsync().Result;
                            List<IApplicationCommand> commands = libcraftCommands.Where(command => command.Name.StartsWith("todo") || command.Name.StartsWith("learn")).ToList();
                            foreach (IApplicationCommand commandToDelete in commands)
                            {
                                _ = commandToDelete.DeleteAsync();
                            }
                        }
                        SlashCommandBuilder command;
                        foreach (SlashCommandInformation commandInfo in item.Value)
                        {
                            Console.WriteLine($"Installing Command {commandInfo.Name}");
                            command = new SlashCommandBuilder();
                            command.WithName(commandInfo.Name);
                            command.WithDescription(commandInfo.Description);
                            command.WithNameLocalizations(commandInfo.NameLocalizations);
                            command.WithDescriptionLocalizations(commandInfo.DescriptionLocalizations);
                            command.WithDefaultPermission(commandInfo.DefaultPermission);
                            if (commandInfo.Options.Count > 0)
                            {
                                foreach (SlashCommandOptionBuilder option in commandInfo.Options)
                                {
                                    command.AddOption(option);
                                    Console.WriteLine($"Successfully added option {option.Name}");
                                }

                            }
                            if (guild != null)
                            {
                                var installedCommand = await guild.CreateApplicationCommandAsync(command.Build());
                            }
                            else
                            {
                                var installedCommand = await _client.CreateGlobalApplicationCommandAsync(command.Build());
                            }
                        }
                    }
                    Console.WriteLine("--- Ending installation of slash commands ---");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("--- Encountered an issue when attempting to install slash commands ---");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("--- Encountered an issue when attempting to install slash commands ---");
                }
            }).Start();
        }

        private async Task OnEventCreated(SocketGuildEvent arg)
        {
            new Task(() =>
            {
                OEH.AnnounceNewEvent(arg);
            }).Start();
        }

        private async Task OnButtonClicked(SocketMessageComponent component)
        {
            new Thread(() =>
            {
                _ = OnComponentInteractionHandlers.ProgressiveSharesDistributionHandler(component, _services);
            }).Start();
        }
        private async Task OnMessage(SocketMessage messageParam)
        {
            if (messageParam.Content == "Make It So" && messageParam.Author.Id == 94545463906144256)
            {
                new Thread(async () =>
                {
                    await messageParam.Channel.SendMessageAsync(await CursedCheck(messageParam.Channel as SocketTextChannel));
                }).Start();
            }
            //Don't process the command if it was a system message
            SocketUserMessage message = messageParam as SocketUserMessage;
            IGuild guild = ((IGuildChannel)message.Channel).Guild;
            List<ulong> noTwitterDeleteGuilds = new()
            {
                698639095940907048,
                1024862866634969188,
                746570029180518422,
                760308671170215968,
                1200630940469297232
            };
            if (message == null) return;

            if (message.Content == "awfuck" && message.Author.Username == "_dart")
            {
                CheckDatEmbed();
            }

            UserRecordsService urservice = _services.GetService<UserRecordsService>();
            if (message.Author.IsBot)
            {
                //We don't want to process messages from bots. Screw bots, all my homies hate bots.

                //Well, except in these specific cases
                if (message.Channel.Id == SharedConstants.LibcraftBestOfChannel && message.Author.Id == 799039246668398633)
                {
                    string[] footerComponents = message.Embeds.First().Footer.Value.Text.Split('-');
                    ulong bestOfUserId = ulong.Parse(footerComponents[2]);
                    urservice.Grant(bestOfUserId, guild.Id, 10);
                }
                return;
            }
            new Thread(async () => { await OnMessageHandlers.ReplyIfMessageIsRecessionOnlyInUpperCase(messageParam); }).Start();
            new Thread(async () => { await LibcoinUtilities.LibcraftCoinMessageHandler(messageParam, urservice); }).Start();
            new Thread(async () => { await OnMessageHandlers.DownloadUsersForGuild(message, guild); }).Start();
            new Thread(async () => { await OnMessageHandlers.ActiveUserCheck(message); }).Start();
            new Thread(async () => { await OnMessageHandlers.HighlightCheck(message, _services.GetService<HighlightService>()); }).Start();

            if ((guild.Id == SharedConstants.LibcraftGuildId || guild.Id == SharedConstants.BoomercraftGuildId || guild.Id == 1219366266033405952) && message.Channel.Id != SharedConstants.SelfCareChannelId)
            {
                new Thread(async () => { await OnMessageHandlers.DeletePreggersMessage(message); }).Start();
            }

            if (!noTwitterDeleteGuilds.Contains(guild.Id) && !message.Content.StartsWith(BotProperties.CommandPrefix))
            {
                // new Thread(async () => { await OnMessageHandlers.ReplyToAllTwitterLinksWithCVXTwitter(message); }).Start();
            }
            else
            {
                new Thread(async () => { await OnMessageHandlers.UWUIfyFlaggedUserTweets(message); }).Start();
            }

            if (!SharedConstants.NoAutoReactsChannel.Contains(message.Channel.Id))
            {
                new Thread(() => { OnMessageHandlers.EgoCheck(messageParam, Utils.IsMentioningMe(messageParam, _client.CurrentUser)); }).Start();
                new Thread(() => { OnMessageHandlers.Imposter(messageParam, Utils.IsSus(messageParam.Content)); }).Start();
                new Thread(() => { _ = OnMessageHandlers.MalarkeyLevelOfHandler(message); }).Start();
                new Thread(() => { _ = OnMessageHandlers.TableFlipCheck(messageParam, guild, _services.GetService<UserRecordsService>()); }).Start();
            }

        }
        private async Task OnReact(Cacheable<IUserMessage, ulong> cachedMessage, Cacheable<IMessageChannel, ulong> cachedChannel, SocketReaction reaction)
        {
            ISocketMessageChannel channel = (ISocketMessageChannel)cachedChannel.Value;
            IEmote reactionEmote;
            IMessage msg = channel.GetMessageAsync(cachedMessage.Id).Result;
            IGuildUser reactingUser = reaction.User.Value as IGuildUser;
            if ((reaction.Emote as Emote) != null)
            {
                reactionEmote = (Emote)reaction.Emote;
            }
            else
            {
                reactionEmote = (Emoji)reaction.Emote;
            }

            //One of the voting reactions, :x:, can also be used to clear DeepState reacts, so we run this regardless.
            new Thread(() => { _ = OnReactHandlers.ClearDeepStateReactionCheck(reactionEmote, channel, msg, _client.CurrentUser); }).Start();
            new Thread(() => { _ = OnReactHandlers.DeleteTwitterMessage(reactionEmote, reactingUser, msg, _client.CurrentUser); }).Start();
            //We only want to process Msg.Author.IsBot requests here actually, so we put this before too.
            new Thread(() => { _ = ORH.EmbedPagingHandler(reaction, msg, _client.CurrentUser, PagedEmbedConstants.HungerGameTributesEmbedTitle, PagingUtilities.TributeEmbedPagingCallback, _services); }).Start();
            new Thread(() => { _ = ORH.EmbedPagingHandler(reaction, msg, _client.CurrentUser, PagedEmbedConstants.OpenRequestEmbedTitle, PagingUtilities.OpenRequestsPagingCallback, _services); }).Start();
            new Thread(() => { _ = ORH.EmbedPagingHandler(reaction, msg, _client.CurrentUser, PagedEmbedConstants.ClosedRequestEmbedTitle, PagingUtilities.ClosedRequestsPagingCallback, _services); }).Start();
            new Thread(() => { _ = ORH.EmbedPagingHandler(reaction, msg, _client.CurrentUser, PagedEmbedConstants.LibcoinBalancesEmbedTitle, PagingUtilities.LibcoinLeaderboardPagingCallback, _services); }).Start();
            new Thread(() => { _ = ORH.EmbedPagingHandler(reaction, msg, _client.CurrentUser, PagedEmbedConstants.LibcoinActiveUserListTitle, PagingUtilities.ActiveUsersPaginingCallback, _services); }).Start();
            new Thread(() => { _ = LibcoinUtilities.LibcoinReactHandler(reaction, msg.Channel as ISocketMessageChannel, msg); }).Start();

            if (SharedConstants.VotingEmotes.Contains(reaction.Emote.Name) || msg.Author.IsBot)
            {
                return;
            }

            new Thread(() => { _ = OnReactHandlers.SelfReactCheck(reaction, channel, msg); }).Start();

            if (!SharedConstants.NoAutoReactsChannel.Contains(msg.Channel.Id) && !SharedConstants.LibcraftBestOfExclusionList.Contains(msg.Channel.Id))
            {
                new Thread(() => { _ = ORH.BestOfChecker(msg, _services.GetService<BestOfService>(), SharedConstants.LibcraftGuildId, SharedConstants.LibcraftBestOfChannel, 10, SharedConstants.LibcraftBestOfVotingEmotes, reactingUser); }).Start();
            }

            new Thread(() => { _ = OnReactHandlers.KlaxonCheck(reactionEmote, channel, msg); }).Start();
        }

    }
}
