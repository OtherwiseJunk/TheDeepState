using DartsDiscordBots.Utilities;
using DeepState.Models.RPGSystemModels.ElectricBastionland;
using DeepState.Service;
using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace DeepState.Modules
{
    [Group("chargen"), Alias("cg")]
    internal class CharacterGeneratorModule : ModuleBase
    {       
        private RandomCharacterImageService _imagingService;
        private FailedCareer[] failedCareers;
        public CharacterGeneratorModule(RandomCharacterImageService randomCharacterImageService) {
            _imagingService = randomCharacterImageService;
            using (StreamReader reader = new StreamReader("FailedCareers.json"))
            {
                failedCareers = JsonSerializer.Deserialize < FailedCareer[]>(reader.ReadToEnd());
            }
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
            int failedCareerRoll = new Dice(failedCareers.Length).Roll();
            FailedCareer career = failedCareers[failedCareerRoll - 1];

            EmbedBuilder builder = new();
            builder.Title = "Your shiny new Electric Bastionland character";
            builder.ThumbnailUrl = _imagingService.CreateAndUploadCharacterImage();
            builder.Description = "I'm sure nothing bad will happen to your precious new character...";
            builder.AddField("Strength", str);
            builder.AddField("Dexterity", dex);
            builder.AddField("Charisma", cha);
            builder.AddField("Cash", money);
            builder.AddField("Health", hp);
            builder.AddField($"Failed Career - {career.Name}:", $"{career.Description}");
            builder.AddField("If you are the youngest player the whole group is £10K in debt to:", career.YoungestPlayerDebtor);
            builder.AddField("You Get:", career.StartingGear);
            builder.AddField(career.MoneyQuestion, career.MoneyAnswers[money - 1]);
            builder.AddField(career.HPQuestion, career.HPAnswers[hp - 1]);

            _ = Context.Message.ReplyAsync(embed: builder.Build());
        }
    }
}
