using LibraryManagement.Domain.Books;
using LibraryManagement.Domain.Loans;
using LibraryManagement.Domain.Members;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.Infrastructure.Persistence.Configurations
{
    public class LoanConfiguration : IEntityTypeConfiguration<Loan>
    {
        public void Configure(EntityTypeBuilder<Loan> builder)
        {
            builder.ToTable("Loans");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.BorrowedOn)
                .IsRequired();

            builder.Property(x => x.DueDate)
                .IsRequired();

            builder.Property(x => x.LateFeeAmount)
                .HasPrecision(10, 2);

            builder.Ignore(x => x.IsActive);

            builder.HasOne<Member>()
                .WithMany()
                .HasForeignKey(x => x.MemberId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<BookCopy>()
                .WithMany()
                .HasForeignKey(x => x.BookCopyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.BookCopyId)
                .IsUnique()
                .HasFilter("\"ReturnedOn\" IS NULL");
        }
    }
}