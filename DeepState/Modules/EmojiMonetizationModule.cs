using DeepState.Data.Services;
using DeepState.Constants;
using DeepState.Modules.Preconditions;
using EMC = DeepState.Constants.EmojiMonetizationConstants;
using Utils = DeepState.Utilities.Utilities;

using Discord.Commands;
using Discord;

using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Text;
using DartsDiscordBots.Permissions;
using System.Threading;

namespace DeepState.Modules
{
    // Creates the group of commands to use
    [Group("emojipack"), Alias("ep", "emoji")]
    public class EmojiMonetizationModule : ModuleBase
    {
        private UserRecordsService _userRecordsService { get; set; }
        public Dictionary<string, (List<Emote> emotes, int cost)> _packs;
        public Dictionary<string, IRole> _packRoles;

        public EmojiMonetizationModule(UserRecordsService service)
        {
            _userRecordsService = service;

            // TODO: Finish populating this

            _packs = new() {
                { "LibCraft Basic Pack", (EMC.BasicPack, 800) },
                { "LibCraft Pro Pack", (EMC.ProPack, 1500) },
                { "GorePack", (EMC.GorePack, 1000) },
                { "GwalmsPack", (EMC.GwalmsPack, 1000) },
                { "CrabPack", (EMC.CrabPack, 5000) },
                { "FroggyPack", (EMC.FroggyPack, 1000) },
                { "SympathyPack", (EMC.SympathyPack, 1000) },
                { "AnguishPack", (EMC.AnguishPack, 1000) },
                { "UserpatPack", (EMC.UserpatPack, 1500) },
                { "BootlickerPack", (EMC.BootlickerPack, 10) }
            };

            _packRoles = new();
        }

        private async Task<IRole> CreateRoleIfMissing(KeyValuePair<string, (List<Emote> emotes, int cost)> pack)
        {
            var packRole = Context.Guild.Roles.FirstOrDefault(r => r.Name.Equals(pack.Key));
            if (packRole == null)
            {
                Console.WriteLine($"No role {pack.Key}, creating...");
                packRole = await Context.Guild.CreateRoleAsync(pack.Key, null, null, false, null);
            }

            return packRole;
        }

        [Command("start")]
        [RequireUserPermission(GuildPermission.ManageGuild, Group = "AdminCheck"), RequireOwner(Group = "AdminCheck"), RequireChannel(new ulong[] { 707386561779597332 }, Group = "AdminCheck")]
        public async Task StartBit()
        {

            await Context.Channel.SendMessageAsync("Starting bit...");

            // Gets each role with a name in the pack
            _packRoles = Context.Guild.Roles
                .Where(r => _packs.Any(p => p.Key == r.Name))
                .ToDictionary(r => r.Name, r => r);

            // Creates a new role for any missing packs
            foreach (var pack in _packs)
            {
                var packRole = await CreateRoleIfMissing(pack);
                _packRoles[pack.Key] = packRole;
            }

            var emotes = Context.Guild.Emotes.Select(emote => emote as Emote).ToList();
            var dummyRole = Context.Guild.Roles.FirstOrDefault(role => role.Name.Equals("Dummy Emoji Role"));
            if (dummyRole == null)
            {
                await Context.Channel.SendMessageAsync("Aw fuck. No dummy role?!");
                dummyRole = await Context.Guild.CreateRoleAsync("Dummy Emoji Role", null, null, false, null);
            }
            await AddRoleToEmotes(dummyRole, emotes);

            // Now add the pack roles to each emoji in each pack
            foreach (var packRole in _packRoles)
            {
                await AddRoleToEmotes(packRole.Value, _packs[packRole.Key].emotes);
                Console.WriteLine($"Added role to all emotes for {packRole.Key}");
            }

            await Context.Channel.SendMessageAsync("Bit successfully started!");
        }

        private List<ulong> getRolesForEmote(GuildEmote guildEmote)
        {
            return guildEmote.RoleIds.ToList();
        }

        [Command("end")]
        [RequireUserPermission(GuildPermission.ManageGuild, Group = "AdminCheck"), RequireOwner(Group = "AdminCheck"), RequireChannel(new ulong[] { 707386561779597332 }, Group = "AdminCheck")]
        public async Task EndBit()
        {
            await Context.Channel.SendMessageAsync("Ending bit...");
            var emotes = Context.Guild.Emotes.Select(emote => emote as Emote).ToList();
            // Clear the roles from all emojis
            foreach (var emote in emotes)
            {
                var guildEmote = await Context.Guild.GetEmoteAsync(emote.Id);
                if (guildEmote == null)
                {
                    Console.WriteLine("Sadge");
                    continue;
                }    
                await Context.Guild.ModifyEmoteAsync(guildEmote, emote =>
                {
                    emote.Roles = null;
                });
                await Context.Channel.SendMessageAsync($"baleted");
            }
            foreach (var pack in _packs)
            {
                var role = Context.Guild.Roles.FirstOrDefault((role) => role.Name.Equals(pack.Key));
                if (role == null)
                {
                    continue;
                }

                await role.DeleteAsync();
            }

            await Context.Channel.SendMessageAsync("Bit successfully ended!");
        }

        [Command("purchase")]
        public async Task Purchase(string input)
        {
            // Only allow in #bot-commands and #echo-chamber
            if (Context.Channel.Id != SharedConstants.LCBotCommandsChannel && Context.Channel.Id != 707386561779597332)
            {
                await Context.Channel.SendMessageAsync($"Sorry, {Context.User.Username}, this command is not allowed in this channel.");
                return;
            }

            // Only allow if bit is active
            if (!IsBitActiveForGuild(Context.Guild))
            {
                await Context.Channel.SendMessageAsync($"We understand your enthusiasm! The LibCraft:registered: Emoji Monetization Pilot Program:tm: has ended due to public backlash, but stay tuned while we work to bring more exciting offers to you!");
            }
            else
            {
                // Fetch the role, case-insensitive
                // If role doesn't exist, null
                IRole packRole = Context.Guild.Roles.FirstOrDefault(r => r.Name.ToLower().Replace(" ", string.Empty) == input.ToLower(), null);

                if (packRole == null)
                {
                    GuildEmote emoteBought = Context.Guild.Emotes.FirstOrDefault(r => r.Name.ToLower() == input.ToLower(), null);

                    if (emoteBought == null)
                    {
                        await Context.Channel.SendMessageAsync("Sorry, I can't find that Emoji or LibCraft:registered: Emoji Pack:tm:.");
                    }
                    else
                    {
                        await BuyEmoji(emoteBought, Context);
                    }
                }
                else
                {
                    await BuyPack(packRole, Context);
                }
            }
        }

        [Command("list")]
        public async Task List()
        {
            EmbedBuilder builder = new();
            builder.Title = "Available Emojipacks";

            foreach(var pack in _packs)
            {
                builder.AddField(pack.Key.Replace(" ",""), $"{pack.Value.cost} Libcoin");
            }

            await Context.Channel.SendMessageAsync(embed: builder.Build());
        }

        [Command("debug")]
        [RequireUserPermission(GuildPermission.ManageGuild, Group = "AdminCheck"), RequireOwner(Group = "AdminCheck"), RequireChannel(new ulong[] { 707386561779597332 }, Group = "AdminCheck")]
        public async Task Debug()
        {
            var builder = new StringBuilder();

            if (!IsBitActiveForGuild(Context.Guild))
            {
                await Context.Channel.SendMessageAsync("The bit _shouldn't_ be on... but let's check...");
            }
            foreach (var pack in _packs)
            {
                foreach (var emote in pack.Value.emotes)
                {
                    var guildEmote = await Context.Guild.GetEmoteAsync(emote.Id);
                    if (guildEmote == null)
                    {
                        Console.WriteLine("Sadge");
                    }

                    var roles = guildEmote.RoleIds.ToList();
                    builder.AppendLine($"${guildEmote.Name} Role IDs");
                    foreach (var role in roles)
                    {
                        builder.AppendLine($"{role}");
                    }
                }
            }


            await Context.Channel.SendMessageAsync(builder.ToString());
        }

        private bool IsBitActiveForGuild(IGuild guild)
        {
            var roles = guild.Roles;
            var bitIsActive = true;
            foreach (var pack in _packs)
            {
                bitIsActive &= roles.FirstOrDefault((role) => role.Name.Equals(pack.Key)) != null;
            }

            return bitIsActive;
        }

        private async Task BuyPack(IRole packRole, ICommandContext Context)
        {
            if (((IGuildUser)Context.User).RoleIds.Any(r => r == packRole.Id))
            {
                await Context.Channel.SendMessageAsync($"Sorry, {Context.User.Username}, but you already own {packRole.Name}.");
                return;
            }

            int packCost = _packs[packRole.Name].cost;
            double senderBalance = _userRecordsService.GetUserBalance(Context.User.Id, Context.Guild.Id);
            string senderName = Context.User.Username;
            Random rand = Utils.CreateSeededRandom();


            if (packCost > senderBalance)
            {
                await Context.Channel.SendMessageAsync($"Sorry, {senderName}, but this LibCraft:registered: Emoji Pack:tm: costs {packCost} LibCoins:registered:, and you only have {senderBalance}.");
            }
            else
            {
                await ((IGuildUser)Context.User).AddRoleAsync(packRole);

                _userRecordsService.Deduct(Context.User.Id, Context.Guild.Id, packCost);
                await Context.Channel.SendMessageAsync($"Congratulations, {senderName}! You are now the proud owner of the {packRole.Name}:tm:, for the low price of just {packCost} LibCoins:registered:.\nFor your convenience, a restart of your discord client is required for this change to take effect. That's the power of your Libcoin at work!\n\n{EMC.ThankYouMessages[rand.Next(0, EMC.ThankYouMessages.Count())]}");
            }
        }

        private async Task BuyEmoji(GuildEmote emoteBought, ICommandContext Context)
        {
            if (((IGuildUser)Context.User).RoleIds.Any(r => emoteBought.RoleIds.Contains(r)))
            {
                await Context.Channel.SendMessageAsync($"Sorry, {Context.User.Username}, but you already own {emoteBought.Name}.");
                return;
            }

            double senderBalance = _userRecordsService.GetUserBalance(Context.User.Id, Context.Guild.Id);
            string senderName = Context.User.Username;
            Random rand = Utils.CreateSeededRandom();

            int emojiCost = emoteBought.Animated ? 150 : 100;

            // Get the role for the emoji if it exists, otherwise create one
            var er = _packRoles
                .FirstOrDefault(r => r.Key.ToLower().Replace(" ", string.Empty) == emoteBought.Name.ToLower()).Value;

            if (er != null)
            {
                await ((IGuildUser)Context.User).AddRoleAsync(er);
            }
            else
            {
                er = await Context.Guild.CreateRoleAsync(emoteBought.Name + " Emoji", null, null, false, null);
                _packRoles.Add(emoteBought.Name, er);
                await AddRoleToEmotes(er, new List<Emote>() { emoteBought });
                await ((IGuildUser)Context.User).AddRoleAsync(er);
            }

            _userRecordsService.Deduct(Context.User.Id, Context.Guild.Id, emojiCost);
            await Context.Channel.SendMessageAsync($"Congratulations, {senderName}! You have bought {emoteBought.Name} for {emojiCost}.\n{EMC.ThankYouMessages[rand.Next(0, EMC.ThankYouMessages.Count())]} \n In order for the changes to take place, you may have to restart your Discord client.");
        }

        private async Task AddRoleToEmotes(IRole role, List<Emote> emotes)
        {
            foreach (var emote in emotes)
            {
                var guildEmote = await Context.Guild.GetEmoteAsync(emote.Id);
                if (guildEmote == null)
                {
                    Console.WriteLine("Failed to cast the emote, bailing :dAmn:");
                }
                await Context.Guild.ModifyEmoteAsync(guildEmote, e =>
                {
                    List<IRole> requiredRoles = new() { role };
                    foreach (var emoteId in guildEmote.RoleIds)
                    {
                        requiredRoles.Add(Context.Guild.GetRole(emoteId));
                    }
                    e.Roles = requiredRoles.Distinct().ToList();
                });
            }
        }
    }
}
