using LibraryManagement.Api.Errors;
using LibraryManagement.Application.Abstractions;
using LibraryManagement.Application.Books;
using LibraryManagement.Application.Loans;
using LibraryManagement.Application.Members;
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
builder.Services.AddScoped<RegisterMember>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program
{
}
