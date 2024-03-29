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
        public Dictionary<string, List<string>> _packs { get; set; };
        
        public EmojiMonetizationModule(UserRecordsService service)
        {
            _userRecordsService = service;
            _bit_active = false;

            // TODO: Finish populating this
            // TODO: Ask Junk about command stuff
            _packs = Dictionary<string, List<Emote>> Packs = new() {
                { "LibCraftProPack", SharedConstants.ProPack },
                { "pro", SharedConstants.BasicPack },
            };
        }

        [Command("start")]
        [RequireUserPermission(GuildPermission.ManageGuild)]
        public async Task StartBit()
        {
            // This creates a new role for each in the list
            List<IRole> PackRoles = new();

            

            List<GuildEmote> emojis = Context.Guild.Emotes;

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