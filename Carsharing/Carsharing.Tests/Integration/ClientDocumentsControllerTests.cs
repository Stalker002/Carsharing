using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using Carsharing.DataAccess;
using Carsharing.DataAccess.Entites;
using Microsoft.Extensions.DependencyInjection;

namespace Carsharing.Tests.Integration;

public class ClientDocumentsControllerTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task GetDocuments_ResponseDoesNotContainSensitiveFields()
    {
        var token = factory.GenerateTestToken(3001, 1);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        using (var scope = factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<CarsharingDbContext>();

            db.ClientDocument.Add(new ClientDocumentEntity
            {
                Id = 4001,
                ClientId = 55,
                Type = "Паспорт",
                LicenseCategory = "B",
                Number = "AB1234567",
                IssueDate = new DateOnly(2024, 1, 10),
                ExpiryDate = new DateOnly(2034, 1, 10),
                FilePath = "private/docs/passport.png"
            });
            await db.SaveChangesAsync();
        }

        var response = await _client.GetAsync("/ClientDocuments");
        var responseContent = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        using var json = JsonDocument.Parse(responseContent);
        var returnedDocument = json.RootElement.EnumerateArray().First(x => x.GetProperty("id").GetInt32() == 4001);

        Assert.False(returnedDocument.TryGetProperty("number", out _));
        Assert.False(returnedDocument.TryGetProperty("filePath", out _));
    }
}
