﻿using DeepState.Models.SlashCommands;
using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepState.Constants
{
    class SlashCommands
    {
        public const string LeRacisme = "racisme";
        public const string WokeMoralists = "wokemoralists";
        public const string DarkBrandon = "darkbrandon";
        public const string PetersonSex = "petersonsex";
        public const string NotThisTime = "notthistime";
        public const string Clara = "clara";
        public const string EML = "eml";
        public const string TheWeekend = "weekend";
        public const string Crackers = "crackers";
        public const string StupidSonOfAbitch = "stupidsonofabitch";
        public const string TooSpicy = "2spicy";
        public const string ImGonnaCome = "imgonnacome";
        public const string DoNotCome = "donotcome";
        public const string ImFromArizona = "imfromarizona";
        public const string AntonCheckin = "antoncheckin";
        public const string IDidEverythingRight = "idideverythingright";
        public const string TrumpMugshot = "mugshot";
        public const string Learn = "learn";
        public const string Apprendre = "apprendre";
        public const string Prosecuted = "prosecuted";
        public static List<SlashCommandOptionBuilder> LearnOptions = new() {
            new SlashCommandOptionBuilder()
            .WithName("input")
            .WithType(ApplicationCommandOptionType.String)
            .WithMaxLength(72)
            .WithMinLength(1)
            .WithDescription("The input you want people to get ready to learn.")
        };
        public static List<SlashCommandOptionBuilder> ApprendreOptions = new() {
            new SlashCommandOptionBuilder()
            .WithName("apport")
            .WithType(ApplicationCommandOptionType.String)
            .WithMaxLength(72)
            .WithMinLength(1)
            .WithDescription("L'apport que vous souhaitez que les gens se préparent à apprendre.")
        };
        public static List<SlashCommandOptionBuilder> ProsecutedOptions = new() {
            new SlashCommandOptionBuilder()
            .WithName("input")
            .WithType(ApplicationCommandOptionType.String)
            .WithMaxLength(72)
            .WithMinLength(1)
            .WithDescription("The input Kamala Harris prosecuted.")
        };

        public static Dictionary<ulong, List<SlashCommandInformation>> SlashCommandsToInstall = new Dictionary<ulong, List<SlashCommandInformation>> {
            {
                SharedConstants.LibcraftGuildId, new List<SlashCommandInformation>
                {
                    new SlashCommandWithoutOptions(Clara,"Such a silly woman."),
                    new SlashCommandWithoutOptions(EML, "You Know What Youre Getting"),
                    new SlashCommandWithoutOptions(Crackers,"That's just how they feel, yanno?"),
                    new SlashCommandWithoutOptions(TooSpicy,"Anton has a weak stomache."),
                    new SlashCommandWithoutOptions(ImFromArizona,"and so can you!"),
                    new SlashCommandWithoutOptions(AntonCheckin,"Live Anton Reaction"),
                    new SlashCommandWithoutOptions(IDidEverythingRight,"I did EVERYTHING RIGHT and I got INDICTED!"),
                    new SlashCommandWithoutOptions(TrumpMugshot, "I wouldn't like to see ol' Donny wiggle out of this jam, if I'm being honest"),                    
                }                
            },
            {
                670647, new List<SlashCommandInformation>{
                    new SlashCommandWithoutOptions(LeRacisme,"'Le racisme' *applause*"),
                    new SlashCommandWithoutOptions(WokeMoralists,"Jordan is DONE with these woke moralists."),
                    new SlashCommandWithoutOptions(DarkBrandon, "He's Coming For You."),
                    new SlashCommandWithoutOptions(PetersonSex,"I don't know what you're expecting, but it can't be good, right?"),
                    new SlashCommandWithoutOptions(NotThisTime,"Jonathan Frakes doesn't like your chances next time, either."),
                    new SlashCommandWithoutOptions(TheWeekend,"Ladies, Gentlemen, and our friends beyond the binary, The Weekend."),
                    new SlashCommandWithoutOptions(StupidSonOfAbitch,"You're a stupid son of a bitch."),
                    new SlashCommandWithoutOptions(ImGonnaCome,"He'll do it."),
                    new SlashCommandWithoutOptions(DoNotCome,"Just don't."),
                    new SlashCommandWithOptions(Learn, "Get ready to learn <Your Input>, buddy!", LearnOptions),
                    new SlashCommandWithOptions(Apprendre, "Préparez-vous à apprendre <Votre Apport>, mon pote!", ApprendreOptions),
                    new SlashCommandWithOptions(Prosecuted, "She Prosecuted <Your Input>", ProsecutedOptions)
                }
            }
        };
}
}
