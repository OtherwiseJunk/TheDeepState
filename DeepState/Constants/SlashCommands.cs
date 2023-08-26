using DeepState.Models.SlashCommands;
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
        public const string ToDoAdd = "todoadd";
        public const string ToDoList = "todo";
        public const string ToDoComplete = "todocomplete";
        public const string ToDoClear = "todoclear";

        public static Dictionary<ulong, List<SlashCommandInformation>> SlashCommandsToInstall = new Dictionary<ulong, List<SlashCommandInformation>> {
            {
                SharedConstants.LibcraftGuildId, new List<SlashCommandInformation>
                {
                    new SlashCommandWithoutOptions(LeRacisme,"'Le racisme' *applause*"),
                    new SlashCommandWithoutOptions(WokeMoralists,"Jordan is DONE with these woke moralists."),
                    new SlashCommandWithoutOptions(DarkBrandon, "He's Coming For You."),
                    new SlashCommandWithoutOptions(PetersonSex,"I don't know what you're expecting, but it can't be good, right?"),
                    new SlashCommandWithoutOptions(NotThisTime,"Jonathan Frakes doesn't like your chances next time, either."),
                    new SlashCommandWithoutOptions(Clara,"Such a silly woman."),
                    new SlashCommandWithoutOptions(EML, "You Know What Youre Getting"),
                    new SlashCommandWithoutOptions(TheWeekend,"Ladies, Gentlemen, and our friends beyond the binary, The Weekend."),
                    new SlashCommandWithoutOptions(Crackers,"That's just how they feel, yanno?"),
                    new SlashCommandWithoutOptions(StupidSonOfAbitch,"You're a stupid son of a bitch."),
                    new SlashCommandWithoutOptions(TooSpicy,"Anton has a weak stomache."),
                    new SlashCommandWithoutOptions(ImGonnaCome,"He'll do it."),
                    new SlashCommandWithoutOptions(DoNotCome,"Just don't."),
                    new SlashCommandWithoutOptions(ImFromArizona,"and so can you!"),
                    new SlashCommandWithoutOptions(AntonCheckin,"Live Anton Reaction"),
                    new SlashCommandWithoutOptions(IDidEverythingRight,"I did EVERYTHING RIGHT and I got INDICTED!"),
                }
            },
            {
                0, new List<SlashCommandInformation>
                {
                    new SlashCommandWithoutOptions(ToDoList, "List your TODO items"),
                    new SlashCommandWithOptions(ToDoAdd, "Add an item to your TODO list. No more than 50 characters.", new(){new SlashCommandOptionBuilder
                    {
                        Name = "text",
                        Type = ApplicationCommandOptionType.String,
                        MaxLength = 150,
                        IsRequired = true,
                        Description = "The text to add to the list",
                        MinLength = 3,
                    }
                    }),
                    new SlashCommandWithOptions(ToDoComplete, "Mark the specified TODO item as complete, by ID. For multiple provide a comma separated list.", new(){new SlashCommandOptionBuilder
                    {
                        Name = "identifier",
                        Type = ApplicationCommandOptionType.String,
                        IsRequired = true,
                        Description = "The ID of the TODO item to mark as completed",
                        MinLength = 1,
                    }
                    }),
                    new SlashCommandWithoutOptions(ToDoClear, "Remove all TODO items marked as complete."),
                }
            }
        };
    }
}
