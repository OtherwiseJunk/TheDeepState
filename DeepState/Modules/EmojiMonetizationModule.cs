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


namespace DeepState.Modules
{
    // Creates the group of commands to use
    [Group("emojipack"), Alias("ep", "emoji")]
    public class EmojiMonetizationModule : ModuleBase
    {
        private UserRecordsService _userRecordsService { get; set; }
        private bool _bit_active { get; set; }
        public Dictionary<string, (List<Emote> emotes, int cost)> _packs { get; set; }
        public Dictionary<string, IRole> _packRoles { get; set; }
        
        public EmojiMonetizationModule(UserRecordsService service)
        {
            _userRecordsService = service;
            _bit_active = false;

            // TODO: Finish populating this
            // TODO: Ask Junk about command stuff

            _packs = new() {/*
                { "LibCraft Basic Pack", (EMC.BasicPack, 800)},
                { "LibCraft Pro Pack", (EMC.ProPack, 1500)},*/
                { "LibCraft Test Pack", (EMC.TestPack, 1)}
            };

            _packRoles = new();
        }

        [Command("start")]
        [RequireUserPermission(GuildPermission.ManageGuild)]
        public async Task StartBit()
        {

            await Context.Channel.SendMessageAsync("Starting bit...");

            var dummyEmojiRole = await Context.Guild.CreateRoleAsync("Dummy Emoji Role", null, null, false, null);

            // Gets each role with a name in the pack
            _packRoles = Context.Guild.Roles
                .Where(r => _packs.Any(p => p.Key == r.Name))
                .ToDictionary(r => r.Name, r => r);

            // Creates a new role for any missing packs
            foreach (var pack in _packs
                .Where(p => !Context.Guild.Roles.Any(r => r.Name == p.Key))
                .ToDictionary(p => p.Key, p => p.Value))
            {
                var newRole = await Context.Guild.CreateRoleAsync(pack.Key, null, null, false, null);
                _packRoles.Add(pack.Key, newRole);
            }

            // Restrict all emojis
            // TODO: uncomment when finished with testing
            /*
            var emotes = Context.Guild.Emotes;
            */
            var emotes = EMC.TestPack;
            foreach (var emote in emotes)
            {
                await Context.Guild.ModifyEmoteAsync((GuildEmote)emote, e =>
                {
                    e.Roles = new List<IRole>{dummyEmojiRole};
                });
            }

            // Now add the pack roles to each emoji in each pack
            foreach (var packRole in _packRoles)
            {
                foreach (var emote in _packs[packRole.Key].emotes)
                {
                    await Context.Guild.ModifyEmoteAsync((GuildEmote)emote, e =>
                    {
                        var roles = e.Roles.GetValueOrDefault();
                        if (roles != null)
                        {
                            e.Roles = new List<IRole> { dummyEmojiRole, packRole.Value };
                        }
                    });
                }
            }

            _bit_active = true;

            await Context.Channel.SendMessageAsync("Bit successfully started!");
        }

        [Command("end")]
        public async Task EndBit()
        {
            await Context.Channel.SendMessageAsync("Ending bit...");

            // Clear the roles from all emojis
            foreach(var emote in Context.Guild.Emotes)
            {
                await Context.Guild.ModifyEmoteAsync(emote, e =>
                {
                    e.Roles = new();
                });
            }
        }

        [Command("purchase")]
        public async void Purchase(string input)
        {
            // Only allow in #bot-commands
            if(Context.Channel.Id != /*SharedConstants.LCBotCommandsChannel*/ 707386561779597332)
            {
                await Context.Channel.SendMessageAsync($"Sorry, {Context.User.Username}, this command is not allowed in this channel.");
                return;
            }

            // Only allow if bit is active
            if(!_bit_active)
            {
                await Context.Channel.SendMessageAsync($"We understand your enthusiasm! The LibCraft:registered: Emoji Monetization Pilot Program:tm: has ended due to public backlash, but stay tuned while we work to bring more exciting offers to you!");

            }

            // Fetch the role, case-insensitive
            // If role doesn't exist, null
            IRole packRole = Context.Guild.Roles.FirstOrDefault(r => r.Name.ToLower().Replace(" ", string.Empty) == input.ToLower(), null);

            if (packRole == null)
            {
                GuildEmote emoteBought = Context.Guild.Emotes.FirstOrDefault(r => r.Name.ToLower() == input.ToLower(), null);
                
                if(emoteBought == null)
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

        private async Task BuyPack(IRole packRole, ICommandContext Context)
        {
            if(((IGuildUser)Context.User).RoleIds.Any(r => r == packRole.Id))
            {
                await Context.Channel.SendMessageAsync($"Sorry, {Context.User.Username}, but you already own {packRole.Name}.");
                return;
            }

            int packCost = _packs[packRole.Name].cost;
            double senderBalance = _userRecordsService.GetUserBalance(Context.User.Id, Context.Guild.Id);
            string senderName = Context.User.Username;
            Random rand = Utils.CreateSeededRandom();
           
                
            if(packCost > senderBalance)
            {
                await Context.Channel.SendMessageAsync($"Sorry, {senderName}, but this LibCraft:registered: Emoji Pack:tm: costs {packCost} LibCoins:registered:, and you only have {senderBalance}.");
            }
            else
            {
                await ((IGuildUser)Context.User).AddRoleAsync(packRole);

                _userRecordsService.Deduct(Context.User.Id, Context.Guild.Id, packCost);
                await Context.Channel.SendMessageAsync($"Congratulations, {senderName}! You are now the proud owner of the {packRole.Name}:tm:, for the low price of just {packCost} LibCoins:registered:.\n{EMC.ThankYouMessages[rand.Next(0, EMC.ThankYouMessages.Count())]}");
            }
        }

        private async Task BuyEmoji(GuildEmote emoteBought, ICommandContext Context)
        {
            if(((IGuildUser)Context.User).RoleIds.Any(r => emoteBought.RoleIds.Contains(r)))
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

            IRole? emoteRole = er.Equals(default(IRole)) ? null : er;

            if(emoteRole != null)
            {
                await ((IGuildUser)Context.User).AddRoleAsync(emoteRole);
            }
            else
            {
                emoteRole = await Context.Guild.CreateRoleAsync(emoteBought.Name + " Emoji", null, null, false, null);
                _packRoles.Add(emoteBought.Name, emoteRole);

            }

            _userRecordsService.Deduct(Context.User.Id, Context.Guild.Id, emojiCost);
            await Context.Channel.SendMessageAsync($"Congratulations, {senderName}! You have bought {emoteBought.Name} for {emojiCost}.\n{EMC.ThankYouMessages[rand.Next(0, EMC.ThankYouMessages.Count())]}");
        }
    }
}