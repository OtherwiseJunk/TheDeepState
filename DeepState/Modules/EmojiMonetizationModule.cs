using DeepState.Data.Services;
using DeepState.Constants;
using DeepState.Modules.Preconditions;
using EMC = DeepState.Constants.EmojiMonetizationConstants;

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
        
        public EmojiMonetizationModule(UserRecordsService service)
        {
            _userRecordsService = service;
            _bit_active = false;

            // TODO: Finish populating this
            // TODO: Ask Junk about command stuff
            _packs = new() {
                { "LibCraft Basic Pack", (EMC.BasicPack, 800) },
                { "LibCraft Pro Pack", (EMC.ProPack, 1500)},
            };
        }

        [Command("start")]
        [RequireUserPermission(GuildPermission.ManageGuild)]
        public async Task StartBit()
        {

            var dummyEmojiRole = await Context.Guild.CreateRoleAsync("Dummy Emoji Role", null, null, false, null);

            // Gets each role with a name in the pack
            Dictionary<string, IRole> packRoles = Context.Guild.Roles
                .Where(r => _packs.Any(p => p.Key == r.Name))
                .ToDictionary(r => r.Name, r => r);

            // Creates a new role for any missing packs
            foreach (var pack in _packs
                .Where(p => !Context.Guild.Roles.Any(r => r.Name == p.Key))
                .ToDictionary(p => p.Key, p => p.Value))
            {
                await Context.Guild.CreateRoleAsync(pack.Key, null, null, false, null);
            }

            // Restrict all emojis
            var emotes = Context.Guild.Emotes;
            foreach (var emote in emotes)
            {
                await Context.Guild.ModifyEmoteAsync(emote, e =>
                {
                    e.Roles = new List<IRole>{dummyEmojiRole};
                });
            }

            // Now add the pack roles to each emoji in each pack
            foreach (var packRole in packRoles)
            {
                foreach (var emote in _packs[packRole.Value.Name].emotes)
                {
                    await Context.Guild.ModifyEmoteAsync((GuildEmote)emote, e =>
                    {
                        e.Roles.Value.ToList().Add(packRole.Value);
                    }
                    );
                }
            }

            _bit_active = true;
        }

        [Command("purchase"), Alias("buy")]
        public async Task BuyPack(string PackName)
        {
            // Fetch the role, case-insensitive
            // If role doesn't exist, null
            IRole packRole = Context.Guild.Roles.FirstOrDefault(r => r.Name.ToLower() == PackName.ToLower());

            if(packRole == null)
            {
                await Context.Channel.SendMessageAsync("Sorry, I couldn't find that pack!");
            }
            else
            {
                double packCost = _packs[PackName].cost;
                double senderBalance = _userRecordsService.GetUserBalance(Context.User.Id, Context.Guild.Id);
                
                if(packCost > senderBalance)
                {
                    // TODO: Say something funny here
                    await Context.Channel.SendMessageAsync("");
                }
                else
                {
                    await ((IGuildUser)Context.User).AddRoleAsync(packRole);
                }
            };
        }
    }
}