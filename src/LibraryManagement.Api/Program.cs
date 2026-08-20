using LibraryManagement.Api.Errors;
using LibraryManagement.Application.Abstractions;
using LibraryManagement.Application.Books;
using LibraryManagement.Application.Loans;
using LibraryManagement.Domain.Books;
using LibraryManagement.Domain.Loans;
using LibraryManagement.Domain.Members;
using LibraryManagement.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<InMemoryBookRepository>();
builder.Services.AddSingleton<InMemoryMemberRepository>();
builder.Services.AddSingleton<InMemoryLoanRepository>();

builder.Services.AddSingleton<IBookRepository>(sp => sp.GetRequiredService<InMemoryBookRepository>());
builder.Services.AddSingleton<IMemberRepository>(sp => sp.GetRequiredService<InMemoryMemberRepository>());
builder.Services.AddSingleton<ILoanRepository>(sp => sp.GetRequiredService<InMemoryLoanRepository>());

builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddScoped<BorrowBook>();

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ApiExceptionHandler>();

builder.Services.AddScoped<ReturnBook>(); 
builder.Services.AddSingleton<ILateFeePolicy, CappedDailyLateFeePolicy>();
builder.Services.AddScoped<GetMemberLateFees>();
builder.Services.AddScoped<RegisterBook>(); 
builder.Services.AddScoped<GetBooks>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    var bookRepository = app.Services.GetRequiredService<InMemoryBookRepository>();
    var memberRepository = app.Services.GetRequiredService<InMemoryMemberRepository>();

    var member = new Member(
        Guid.Parse("11111111-1111-1111-1111-111111111111"),
        "Member 1",
        MembershipPlan.Standard);

    var book = new Book(
        Guid.Parse("22222222-2222-2222-2222-222222222222"),
        "Book 1",
        "Author 1",
        2);

    memberRepository.Add(member);
    await bookRepository.AddAsync(book, CancellationToken.None);
}

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program
{
}
