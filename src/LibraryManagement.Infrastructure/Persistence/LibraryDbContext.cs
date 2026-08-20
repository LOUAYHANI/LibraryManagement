using LibraryManagement.Domain.Books;
using LibraryManagement.Domain.Loans;
using LibraryManagement.Domain.Members;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Persistence
{
    public class LibraryDbContext : DbContext
    {
        public DbSet<Book> Books => Set<Book>();
        public DbSet<BookCopy> BookCopies => Set<BookCopy>();
        public DbSet<Member> Members => Set<Member>();
        public DbSet<Loan> Loans => Set<Loan>();

        public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(LibraryDbContext).Assembly);
        }
    }
}