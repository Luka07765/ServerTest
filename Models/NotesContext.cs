using Microsoft.EntityFrameworkCore;

namespace NotesApi.Models
{
    public class NotesContext : DbContext
    {
        public NotesContext(DbContextOptions<NotesContext> options)
            : base(options)
        {
        }

        public DbSet<Note> Notes { get; set; }
    }
}
