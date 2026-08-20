using LibraryManagement.Domain.Books;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.Infrastructure.Persistence.Configurations
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.ToTable("Books");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Author)
                .IsRequired()
                .HasMaxLength(150);

            builder.HasMany(x => x.Copies)
                .WithOne()
                .HasForeignKey("BookId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(x => x.Copies)
                .UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}