using System.Data.Common;
using LibraryManagement.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LibraryManagement.IntegrationTests
{
    public class LibraryApiFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(x =>
                    x.ServiceType == typeof(DbContextOptions<LibraryDbContext>));

                if (descriptor is not null)
                    services.Remove(descriptor);

                services.AddSingleton<DbConnection>(_ =>
                {
                    var connection = new SqliteConnection("DataSource=:memory:");
                    connection.Open();

                    return connection;
                });

                services.AddDbContext<LibraryDbContext>((serviceProvider, options) =>
                {
                    var connection = serviceProvider.GetRequiredService<DbConnection>();
                    options.UseSqlite(connection);
                });
            });
        }
    }
}