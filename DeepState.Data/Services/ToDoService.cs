using DartsDiscordBots.Utilities;
using DeepState.Data.Context;
using DeepState.Data.Models;
using Discord;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Text;

namespace DeepState.Data.Services
{
    public class ToDoService
    {
        public IDbContextFactory<ToDoContext> dataContextFactory { get; set; }

        public ToDoService(IDbContextFactory<ToDoContext> contextFactory)
        {
            dataContextFactory = contextFactory;
        }

        public bool ToDoBelongsToUser(ulong userId, int toDoId)
        {
            using (ToDoContext context = dataContextFactory.CreateDbContext())
            {
                return context.ToDoItems.FirstOrDefault(toDoItem => toDoItem.Id == toDoId && toDoItem.DiscordUserId == userId) != null;
            }
        }

        public bool IsToDoItemCompleted(int toDoId)
        {
            using (ToDoContext context = dataContextFactory.CreateDbContext())
            {
                return context.ToDoItems.First(toDoItem => toDoItem.Id == toDoId).IsCompleted;
            }
        }

        public void MarkToDoComplete(int toDoId)
        {
            using (ToDoContext context = dataContextFactory.CreateDbContext())
            {
                ToDoItem item = context.ToDoItems.First(toDoItem => toDoItem.Id == toDoId);
                item.IsCompleted = true;
                context.Attach(item);
                context.Entry(item).Property(i => i.IsCompleted).IsModified = true;
                context.SaveChanges();
            }
        }

        public void ClearAllCompletedToDo(ulong userId)
        {
            using (ToDoContext context = dataContextFactory.CreateDbContext())
            {
                List<ToDoItem> completedItems = context.ToDoItems.Where(toDoItem => toDoItem.IsCompleted && toDoItem.DiscordUserId == userId).ToList();
                context.ToDoItems.RemoveRange(completedItems);
                context.SaveChanges();
            }
        }

        public void AddToDo(ulong userId, string text)
        {
            using (ToDoContext context = dataContextFactory.CreateDbContext())
            {
                context.Add(new ToDoItem(userId, text));
                context.SaveChanges();
            }
        }

        public List<ToDoItem> GetUsersToDos(ulong userId)
        {
            using (ToDoContext context = dataContextFactory.CreateDbContext())
            {
                return context.ToDoItems.Where(toDoItem => toDoItem.DiscordUserId == userId).ToList();
            }
        }

        public string BuildToDoListResponse(IUser user)
        {
            string response;
            StringBuilder builder = new("```");
            List<ToDoItem> toDos = GetUsersToDos(user.Id);
            if (toDos.Count > 0)
            {
                string username = user is IGuildUser ? BotUtilities.GetDisplayNameForUser(user as IGuildUser) : user.Username;
                string title = $"{username}'s TODO list";
                builder.AppendLine(title);
                builder.AppendLine(string.Concat(Enumerable.Repeat("=", title.Length)));
                int biggestIdLength = $"toDos.Max(toDo => toDo.Id)".Length;
                foreach (ToDoItem toDoItem in toDos)
                {
                    string toDoCheckbox = toDoItem.IsCompleted ? "[x]" : "[ ]";
                    string toDoIDText = $"{toDoItem}";
                    while(toDoIDText.Length < biggestIdLength)
                    {
                        toDoIDText += " ";
                    }
                    builder.AppendLine($"-{toDoItem.Id}: {toDoCheckbox} {toDoItem.Text}");
                }
                builder.AppendLine("```");
                response = builder.ToString();
            }
            else
            {
                response = "Sorry, you don't have any TODO items yet";
            }

            return response;
        }
    }
}
