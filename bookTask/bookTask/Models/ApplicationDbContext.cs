using Microsoft.EntityFrameworkCore;

namespace bookTask.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>
            options) : base(options)
        {
            
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<BorrowedBook> BorrowedBooks { get; set; }

        public DbSet<Borrower> Borrowers { get; set; }
    }
}
