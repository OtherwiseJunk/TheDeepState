using DeepState.Service;
using Discord;
using Discord.Commands;
using System.Linq;
using System.Threading.Tasks;

namespace DeepState.Modules
{
    [Group("Feedback"), Alias("fb", "complain")]
    public class FeedbackModule : ModuleBase
    {
        PanopticonService _service { get; set; }
        ulong FeedbackChannelId = 967308491041693706;
        ulong LibcraftGuildId = 698639095940907048;

        public FeedbackModule(PanopticonService service)
        {
            _service = service;
        }

        [Command()]
        [Summary("Sends feedback to the Libcraft Admins")]
        public async Task CreateFeedback([Remainder] string feedback)
        {
            IGuild libcraftGuild = Context.Client.GetGuildAsync(LibcraftGuildId).Result;
            ITextChannel feedbackChannel = (ITextChannel)libcraftGuild.GetChannelAsync(FeedbackChannelId).Result;
            _service.CreateFeedback(feedback, Context.Message.Author.Id);
            int id = _service.GetAllFeedback().First(fb => fb.Message == feedback).Id;
            EmbedBuilder builder = new EmbedBuilder();
            builder.Title = $"Feedback #{id}";
            builder.Description = feedback;
            await feedbackChannel.SendMessageAsync(embed: builder.Build());
        }
    }
}
