using LibraryManagement.Api.Errors;
using LibraryManagement.Application.Abstractions;
using LibraryManagement.Application.Books;
using LibraryManagement.Application.Loans;
using LibraryManagement.Application.Members;
using LibraryManagement.Domain.Loans;
using LibraryManagement.Infrastructure.Persistence;
using LibraryManagement.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("LibraryDatabase")));
builder.Services.AddScoped<IBookRepository, EfBookRepository>();
builder.Services.AddScoped<IMemberRepository, EfMemberRepository>();
builder.Services.AddScoped<ILoanRepository, EfLoanRepository>();
builder.Services.AddScoped<IUnitOfWork, EfUnitOfWork>();

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

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();
    dbContext.Database.Migrate();
}

app.Run();
public partial class Program
{
}
