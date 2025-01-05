using IntegrationTests.TestFixtures;
using Microsoft.AspNetCore.Mvc.Testing;

namespace IntegrationTests.Tests;

[Collection("IntegrationTests")]
public class SampleIntegrationTest
{
    private readonly HttpClient _client;

    public SampleIntegrationTest(IntegrationTestsFixture fixture)
    {
        _client = fixture.Factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }
    
    [Fact]
    public async Task Should_be_able_to_query_the_api()
    {
        var response = await _client.GetAsync("/healthcheck");
        
        response.EnsureSuccessStatusCode();
    }
    
}