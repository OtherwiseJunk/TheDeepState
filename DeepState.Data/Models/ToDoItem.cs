using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepState.Data.Models
{
    public class ToDoItem
    {
        [Key]
        public int Id { get; set; }
        public ulong DiscordUserId { get; set; }
        public string Text { get; set; }
        public bool IsCompleted { get; set; }

        public ToDoItem() { }
        public ToDoItem(ulong discordUserId, string text)
        {
            DiscordUserId = discordUserId;
            Text = text;
            IsCompleted = false;
        }
    }
}
