using DeepState.Data.Services;
using DeepState.Constants;
using DeepState.Modules.Preconditions;
using EMC = DeepState.Data.Constants.EmojiMonetizationConstants;

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
        private UserRecordsService _userRecordsService { get; set; };
        private bool _bit_active { get; set; };
        public Dictionary<string, (List<Emote> emotes, int cost)> _packs { get; set; };
        
        public EmojiMonetizationModule(UserRecordsService service)
        {
            _userRecordsService = service;
            _bit_active = false;

            // TODO: Finish populating this
            // TODO: Ask Junk about command stuff
            _packs new() {
                { "LibCraft Basic Pack", (SharedConstants.ProPack, 800) },
                { "LibCraft Pro Pack", (SharedConstants.BasicPack, 1500)},
            };
        }

        [Command("start")]
        [RequireUserPermission(GuildPermission.ManageGuild)]
        public async Task StartBit()
        {

            IRole dummyEmojiRole = new("Dummy Emoji Role");

            // Gets each role with a name in the pack
            List<IRole> packRoles = Context.Guild.Roles
                .Where(r => _packs.Any(p => p.Value.))
                .ToDictionary(r => r.Name, r => r)

            // Creates a new role for any missing packs
            foreach (var pack in _packs
                .Where(p => !(Context.Guild.Roles.Any(r => r.Name == p.Key)))
                .ToDictionary(p => p.Key, p => p.Value))
            {
                await newRole = Context.Guild.CreateRoleAsync(pack).Result;
            }

            // Restrict all emojis
            List<GuildEmote> emojis = Context.Guild.Emotes;
            foreach (var emoji in emojis)
            {
                await Context.Guild.ModifyEmoteAsync(emoji =>
                {
                    emoji.Roles = new List<Emote>{dummyEmojiRole};
                })
            }

            // Now add the pack roles to each
            foreach (var pack in packRoles)
            {
                foreach (var emote in _packs[pack].emotes)
                {
                    emote.Roles.Add(pack)
                }
            }

            _bit_active = true;
        }

        [Command("purchase"), Alias("buy")]
        // Placeholder here. Should be 100 times # of emojis when the pack structs are implemented
        // TODO: Update this when structs are done
        [RequireLibcoinBalance(EMC.PerEmojiCost)]

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
                _ = ((IGuildUser)Context.User).AddRoleAsync(packRole)
            }
        }

    }
}