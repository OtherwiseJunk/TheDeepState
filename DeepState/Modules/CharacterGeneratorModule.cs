using DartsDiscordBots.Utilities;
using DeepState.Models.RPGSystemModels.ElectricBastionland;
using DeepState.Services;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using DeepState.Constants.CharacterGeneration;
using System;
using System.Linq;

namespace DeepState.Modules
{
    [Group("chargen"), Alias("cg")]
    internal class CharacterGeneratorModule : ModuleBase
    {
        private RandomCharacterImageService _imagingService;
        public CharacterGeneratorModule(RandomCharacterImageService randomCharacterImageService)
        {
            _imagingService = randomCharacterImageService;
        }

        [Command("electricbastionland"), Alias("eb")]
        public async Task GenerateElectricBastionlandCharacter(string avatarType = AvatarConstants.Adventurer)
        {
            if (!AvatarConstants.AvatarTypes.Contains(avatarType.ToLower()))
            {
                _ = Context.Message.ReplyAsync("Sorry, that I don't have that avatar type in my list. Run `>cg al` to see the accepted list of avatar types.");
                return;
            }
            Dice d6 = new(6);
            int str = d6.Roll(3).Total;
            int dex = d6.Roll(3).Total;
            int cha = d6.Roll(3).Total;
            int hp = d6.Roll();
            int money = d6.Roll();
            int failedCareerRoll = new Dice(ElectricBastionlandConstants.FailedCareers.Length).Roll();
            FailedCareer career = ElectricBastionlandConstants.FailedCareers[failedCareerRoll - 1];

            EmbedBuilder builder = new();
            builder.Title = "Your shiny new Electric Bastionland character";
            builder.ThumbnailUrl = _imagingService.CreateAndUploadCharacterImage(avatarType);
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

            await Context.Message.ReplyAsync(embed: builder.Build());
        }

        [Command("AvatarList"), Alias("al"), RequireOwner]
        public async Task GetAvatarList()
        {
            _ = Context.Message.ReplyAsync($"Sure, here's a list of all the avatar types character gen creation commands can accept: {string.Join(Environment.NewLine, AvatarConstants.AvatarTypes)}");
        }
    }
}
