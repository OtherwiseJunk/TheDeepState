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
        // This creates an instance of the service that checks user balance
        private UserRecordsService _userRecordsService { get; set; }
        private bool _bit_active { get; set; }
        
        public EmojiMonetizationModule(UserRecordsService service)
        {
            _userRecordsService = service;
            _bit_active = false;
        }

        [Command("start")]
        [RequireUserPermission(GuildPermission.ManageGuild)]
        public async Task StartBit()
        {
            // This gets all the roles in the list of packs
            List<IRole> PackRoles = Context.Guild.Roles.Where(r => EMC.EmojiPackRoles.Contains(r.Name)).ToList();

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
                
            }
        }

    }
}