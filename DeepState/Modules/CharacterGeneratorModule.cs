using DartsDiscordBots.Utilities;
using DeepState.Service;
using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepState.Modules
{
    [Group("chargen"), Alias("cg")]
    internal class CharacterGeneratorModule : ModuleBase
    {       
        RandomCharacterImageService _imagingService;
        public CharacterGeneratorModule(RandomCharacterImageService randomCharacterImageService) {
            _imagingService = randomCharacterImageService;
        }

        [Command("electricbastionland"), Alias("eb")]
        public async Task GenerateElectricBastionlandCharacter()
        {
            Dice d6 = new(6);
            int str = d6.Roll(3).Total;
            int dex = d6.Roll(3).Total;
            int cha = d6.Roll(3).Total;
            int hp = d6.Roll();
            int money = d6.Roll();
            int failedCareer = new Dice(100).Roll();

            EmbedBuilder builder = new();
            builder.Title = "Your shiny new Electric Bastionland character";
            builder.ThumbnailUrl = _imagingService.CreateAndUploadCharacterImage();
            builder.Description = "I'm sure nothing bad will happy to your precious new character...";
            builder.AddField("Strength", str);
            builder.AddField("Dexterity", dex);
            builder.AddField("Charisma", cha);
            builder.AddField("Cash", money);
            builder.AddField("Health", hp);
            builder.AddField("Failed Career Code", failedCareer);

            _ = Context.Message.ReplyAsync(embed: builder.Build());
        }
    }
}
