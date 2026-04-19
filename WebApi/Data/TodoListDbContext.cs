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

        public DbSet<TodoItem> TodoItems { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<TodoItemTag> TodoItemTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TodoItem>()
                .HasOne(x => x.TodoList)
                .WithMany(x => x.Items)
                .HasForeignKey(x => x.TodoListId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TodoItem>()
                .Property(x => x.Priority)
                .HasConversion<int>();

            modelBuilder.Entity<TodoItemTag>()
                .HasKey(x => new { x.TodoItemId, x.TagId });

            modelBuilder.Entity<TodoItemTag>()
                .HasOne(x => x.TodoItem)
                .WithMany(x => x.ItemTags)
                .HasForeignKey(x => x.TodoItemId);

            modelBuilder.Entity<TodoItemTag>()
                .HasOne(x => x.Tag)
                .WithMany(x => x.ItemTags)
                .HasForeignKey(x => x.TagId);

            modelBuilder.Entity<TodoList>().HasData(
                new TodoList
                {
                    Id = 1,
                    Title = "Work",
                    Description = "Work-related tasks",
                    CreatedAt = new DateTimeOffset(2026, 04, 01, 9, 0, 0, TimeSpan.Zero)
                },
                new TodoList
                {
                    Id = 2,
                    Title = "Home",
                    Description = "Chores and errands",
                    CreatedAt = new DateTimeOffset(2026, 04, 01, 9, 5, 0, TimeSpan.Zero)
                },
                new TodoList
                {
                    Id = 3,
                    Title = "Study",
                    Description = "University / courses",
                    CreatedAt = new DateTimeOffset(2026, 04, 01, 9, 10, 0, TimeSpan.Zero)
                });

            modelBuilder.Entity<Tag>().HasData(
                new Tag { Id = 1, Name = "urgent" },
                new Tag { Id = 2, Name = "shopping" },
                new Tag { Id = 3, Name = "health" },
                new Tag { Id = 4, Name = "backend" });

            modelBuilder.Entity<TodoItem>().HasData(
                new TodoItem
                {
                    Id = 1,
                    TodoListId = 1,
                    Title = "Fix API validation",
                    Notes = "Add validation errors + proper status codes",
                    IsCompleted = false,
                    Priority = TodoPriority.High,
                    CreatedAt = new DateTimeOffset(2026, 04, 10, 10, 0, 0, TimeSpan.Zero),
                    DueAt = new DateTimeOffset(2026, 04, 30, 18, 0, 0, TimeSpan.Zero)
                },
                new TodoItem
                {
                    Id = 2,
                    TodoListId = 1,
                    Title = "Deploy to staging",
                    Notes = "Check connection string + migrations",
                    IsCompleted = false,
                    Priority = TodoPriority.Urgent,
                    CreatedAt = new DateTimeOffset(2026, 04, 11, 12, 0, 0, TimeSpan.Zero),
                    DueAt = new DateTimeOffset(2026, 04, 20, 18, 0, 0, TimeSpan.Zero)
                },
                new TodoItem
                {
                    Id = 3,
                    TodoListId = 2,
                    Title = "Buy groceries",
                    Notes = "Milk, eggs, bread",
                    IsCompleted = false,
                    Priority = TodoPriority.Normal,
                    CreatedAt = new DateTimeOffset(2026, 04, 12, 9, 0, 0, TimeSpan.Zero),
                    DueAt = new DateTimeOffset(2026, 04, 12, 20, 0, 0, TimeSpan.Zero)
                },
                new TodoItem
                {
                    Id = 4,
                    TodoListId = 2,
                    Title = "Pay bills",
                    Notes = "Electricity + internet",
                    IsCompleted = true,
                    Priority = TodoPriority.High,
                    CreatedAt = new DateTimeOffset(2026, 04, 05, 9, 0, 0, TimeSpan.Zero),
                    CompletedAt = new DateTimeOffset(2026, 04, 06, 19, 0, 0, TimeSpan.Zero)
                },
                new TodoItem
                {
                    Id = 5,
                    TodoListId = 3,
                    Title = "Prepare for exam",
                    Notes = "Solve practice tasks",
                    IsCompleted = false,
                    Priority = TodoPriority.High,
                    CreatedAt = new DateTimeOffset(2026, 04, 07, 14, 0, 0, TimeSpan.Zero),
                    DueAt = new DateTimeOffset(2026, 05, 05, 12, 0, 0, TimeSpan.Zero)
                },
                new TodoItem
                {
                    Id = 6,
                    TodoListId = 3,
                    Title = "Read EF Core docs",
                    Notes = "Migrations + relationships",
                    IsCompleted = false,
                    Priority = TodoPriority.Low,
                    CreatedAt = new DateTimeOffset(2026, 04, 08, 16, 0, 0, TimeSpan.Zero)
                });

            modelBuilder.Entity<TodoItemTag>().HasData(
                new TodoItemTag { TodoItemId = 1, TagId = 4 },
                new TodoItemTag { TodoItemId = 2, TagId = 1 },
                new TodoItemTag { TodoItemId = 3, TagId = 2 },
                new TodoItemTag { TodoItemId = 4, TagId = 2 },
                new TodoItemTag { TodoItemId = 5, TagId = 1 },
                new TodoItemTag { TodoItemId = 6, TagId = 4 }
            );
        }
    }
}