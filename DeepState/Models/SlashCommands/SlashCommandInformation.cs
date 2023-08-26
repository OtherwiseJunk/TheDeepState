using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepState.Models.SlashCommands
{
    public class SlashCommandInformation
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool DefaultPermission { get; set; }
        public List<SlashCommandOptionBuilder> Options { get; set; }
        public Dictionary<string, string> NameLocalizations { get; set; }
        public Dictionary<string, string> DescriptionLocalizations { get; set; }
    }
}
