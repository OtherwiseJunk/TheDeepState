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
        public const string IDidEverythingRight = "idideverythingright";

        public static Dictionary<ulong, List<SlashCommandInformation>> SlashCommandsToInstall = new Dictionary<ulong, List<SlashCommandInformation>> {
            {
                SharedConstants.LibcraftGuildId, new List<SlashCommandInformation>
                {
                    new AutoResponseCommandInformation(LeRacisme,"'Le racisme' *applause*"),
                    new AutoResponseCommandInformation(WokeMoralists,"Jordan is DONE with these woke moralists."),
                    new AutoResponseCommandInformation(DarkBrandon, "He's Coming For You."),
                    new AutoResponseCommandInformation(PetersonSex,"I don't know what you're expecting, but it can't be good, right?"),
                    new AutoResponseCommandInformation(NotThisTime,"Jonathan Frakes doesn't like your chances next time, either."),
                    new AutoResponseCommandInformation(Clara,"Such a silly woman."),
                    new AutoResponseCommandInformation(EML, "You Know What Youre Getting"),
                    new AutoResponseCommandInformation(TheWeekend,"Ladies, Gentlemen, and our friends beyond the binary, The Weekend."),
                    new AutoResponseCommandInformation(Crackers,"That's just how they feel, yanno?"),
                    new AutoResponseCommandInformation(StupidSonOfAbitch,"You're a stupid son of a bitch."),
                    new AutoResponseCommandInformation(TooSpicy,"Anton has a weak stomache."),
                    new AutoResponseCommandInformation(ImGonnaCome,"He'll do it."),
                    new AutoResponseCommandInformation(DoNotCome,"Just don't."),
                    new AutoResponseCommandInformation(ImFromArizona,"and so can you!"),
                    new AutoResponseCommandInformation(AntonCheckin,"Live Anton Reaction"),
                    new AutoResponseCommandInformation(IDidEverythingRight,"I did EVERYTHING RIGHT and I got INDICTED!")
                }
            },
        };
    }
}
