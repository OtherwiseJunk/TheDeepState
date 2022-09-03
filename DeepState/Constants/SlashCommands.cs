using DeepState.Models;
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

        public static Dictionary<ulong, List<SlashCommandInformation>> SlashCommandsToInstall = new Dictionary<ulong, List<SlashCommandInformation>> {
            {
                SharedConstants.LibcraftGuildId, new List<SlashCommandInformation>
                {
                    new SlashCommandInformation {
                        Name = LeRacisme,
                        Description = "'Le racisme' *applause*",
                        DefaultPermission = true,
                        Options = null
                    },
                    new SlashCommandInformation {
                        Name = WokeMoralists,
                        Description = "Jordan is DONE with these woke moralists.",
                        DefaultPermission = true,
                        Options = null
                    },
                    new SlashCommandInformation {
                        Name = DarkBrandon,
                        Description = "He's Coming For You.",
                        DefaultPermission = true,
                        Options = null
                    },
                    new SlashCommandInformation {
                        Name = PetersonSex,
                        Description = "I don't know what you're expecting, but it can't be good, right?",
                        DefaultPermission = true,
                        Options = null
                    },
                    new SlashCommandInformation {
                        Name = NotThisTime,
                        Description = "Jonathan Frakes doesn't like your chances next time, either.",
                        DefaultPermission = true,
                        Options = null
                    },
                    new SlashCommandInformation {
                        Name = Clara,
                        Description = "Such a silly woman.",
                        DefaultPermission = true,
                        Options = null
                    },
                   new SlashCommandInformation {
                        Name = EML,
                        Description = "You Know What Youre Getting",
                        DefaultPermission = true,
                        Options = null
                    },
                    new SlashCommandInformation {
                        Name = TheWeekend,
                        Description = "Ladies, Gentlemen, and our friends beyond the binary, The Weekend.",
                        DefaultPermission = true,
                        Options = null
                    },
                    new SlashCommandInformation {
                        Name = Crackers,
                        Description = "That's just how they feel, yanno?",
                        DefaultPermission = true,
                        Options = null
                    },
                    new SlashCommandInformation {
                        Name = StupidSonOfAbitch,
                        Description = "You're a stupid son of a bitch.",
                        DefaultPermission = true,
                        Options = null
                    },
                    new SlashCommandInformation {
                        Name = TooSpicy,
                        Description = "Anton has a weak stomache.",
                        DefaultPermission = true,
                        Options = null
                    },
                    new SlashCommandInformation {
                        Name = ImGonnaCome,
                        Description = "He'll do it.",
                        DefaultPermission = true,
                        Options = null
                    },
                    new SlashCommandInformation {
                        Name = DoNotCome,
                        Description = "Just don't.",
                        DefaultPermission = true,
                        Options = null
                    },
                    new SlashCommandInformation {
                        Name = ImFromArizona,
                        Description = "and so can you!",
                        DefaultPermission = true,
                        Options = null
                    },
                    new SlashCommandInformation {
                        Name = AntonCheckin,
                        Description = "Live Anton Reaction",
                        DefaultPermission = true,
                        Options = null
                    },
                }
            },
        };
    }
}
