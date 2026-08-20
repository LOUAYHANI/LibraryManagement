using System.Net;
using System.Net.Http.Json;
using LibraryManagement.Api.Contracts.Books;
using Shouldly;

namespace LibraryManagement.IntegrationTests.Books
{
    public class BookCatalogueTests
    {
        [Fact]
        public async Task Registers_and_returns_book()
        {
            await using var application = new LibraryApiFactory();
            var client = application.CreateClient();

            var request = new RegisterBookRequest
            {
                Title = "Book 2",
                Author = "Author 2",
                CopyCount = 3
            };

            var createResponse = await client.PostAsJsonAsync("/api/books", request);

            createResponse.StatusCode.ShouldBe(HttpStatusCode.Created);

            var createdBook = await createResponse.Content.ReadFromJsonAsync<BookResponse>();

            createdBook.ShouldNotBeNull();
            createdBook.Title.ShouldBe("Book 2");
            createdBook.TotalCopies.ShouldBe(3);
            createdBook.AvailableCopies.ShouldBe(3);

            var getResponse = await client.GetAsync("/api/books");

            getResponse.StatusCode.ShouldBe(HttpStatusCode.OK);

            var books = await getResponse.Content.ReadFromJsonAsync<List<BookResponse>>();

            books.ShouldNotBeNull();
            books.ShouldContain(x => x.Id == createdBook.Id);
        }
    }
}