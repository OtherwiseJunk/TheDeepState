using System.ComponentModel.DataAnnotations;

namespace DeepState.Data.Models
{
    public record Highlight
    {
        [Key]
        public int HighlightId { get; set; }
        public ulong UserId { get; set; }
        public string TriggerPhrase { get; set; }

        public Highlight(ulong userId, string triggerPhrase)
        {
            UserId = userId;
            TriggerPhrase = triggerPhrase;
        }
    }
}
