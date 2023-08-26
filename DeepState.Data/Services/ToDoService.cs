using DeepState.Data.Context;
using DeepState.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

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
    }
}
