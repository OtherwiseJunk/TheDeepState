using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepState.Constants
{
    public static class CommandErrorConstants
    {
        public const string RequiredMessageReferenceMissingError = "Citizen, this command requires you to be replying to a message. Please try again.";
        public const string NotReplyingToMeError = "I'm sorry Citizen, but this command required that you be replying specifically to a message sent by me.";
        public static string AttachmentCountError(int requiredCount) => $"I can't find the needed attachment{(requiredCount == 1 ? string.Empty : 's')}. This command needs {requiredCount} attachment{(requiredCount == 1 ? string.Empty : 's')}, Citizen.";
        public static string EmbedCountError(int requiredCount) => $"I can't find the needed embed{(requiredCount == 1 ? string.Empty : 's')}. This command needs {requiredCount} embed{(requiredCount == 1 ? string.Empty : 's')}, Citizen.";
        public static string CannotFindNeededEmbedError = "Citizen I just can't find the embed that we need for this command, did you reply to the wrong message?";
    }
}
