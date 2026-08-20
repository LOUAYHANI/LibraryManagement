using LibraryManagement.Domain.Books;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.Infrastructure.Persistence.Configurations
{
    public class BookCopyConfiguration : IEntityTypeConfiguration<BookCopy>
    {
        public void Configure(EntityTypeBuilder<BookCopy> builder)
        {
            builder.ToTable("BookCopies");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.State)
                .IsRequired();
        }
    }
}