using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;


namespace IntroTask.Models
{
    public class ToDoContext : DbContext
    {
        public ToDoContext(DbContextOptions<ToDoContext> options)
            : base(options)
        {
        }

        public DbSet<ToDoText> ToDoTexts { get; set; } = null!;
    }
}
