using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using FluentAssertions;
using WebApi.Shared.Products;

namespace WebApi.SinglePurpose.Test;

public class ProductApiTests
{
    [Fact]
    public async Task CanCreateAndList_ShouldReturnOk()
    {
        var httpClient = new HttpClient();

        var result = await httpClient.PostAsync("https://dl37krl1ei.execute-api.us-east-1.amazonaws.com",
            new StringContent(JsonSerializer.Serialize(new ProductDTO()
            {
                Name = "test product",
                Price = 10
            }), Encoding.UTF8, "application/json"));

        result.IsSuccessStatusCode.Should().BeTrue();

        var getResult = await httpClient.GetAsync("https://dl37krl1ei.execute-api.us-east-1.amazonaws.com");

        getResult.IsSuccessStatusCode.Should().BeTrue();

        var data = await getResult.Content.ReadFromJsonAsync<List<Product>>();

        data?.Count.Should().BeGreaterThan(0);
    }
}