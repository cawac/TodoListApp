using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Data
{
    public class TodoListDbContext : DbContext
    {
        public TodoListDbContext(DbContextOptions<TodoListDbContext> options)
            : base(options)
        {
        }

        public DbSet<TodoList> TodoLists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TodoList>().HasData(
                new TodoList { Id = 1, Title = "Work" },
                new TodoList { Id = 2, Title = "Home" },
                new TodoList { Id = 3, Title = "Study" }
            );
        }
    }
}