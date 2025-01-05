using UnitTests.Fixtures;

namespace UnitTests.Architecture;

public class EndpointArchitectureTests : IClassFixture<EndpointAnalysisFixture>
{
    private readonly EndpointAnalysisFixture _fixture;

    public EndpointArchitectureTests(EndpointAnalysisFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void All_Post_Endpoints_Should_Have_Validators()
    {
        var endpoints = _fixture.FindEndpoints("MapPost");

        foreach (var (file, methodCall, expressionText) in endpoints)
        {
            var hasValidator = expressionText.Contains("WithValidator");
            
            Assert.True(hasValidator, $"Endpoint POST em  {file} deve ter um validator configurado via WithValidator(). \nEndpoint {expressionText}");
        }
    }
    
    [Fact]
    public void All_Put_Endpoints_Should_Have_Validators()
    {
        var endpoints = _fixture.FindEndpoints("MapPut");

        foreach (var (file, methodCall, expressionText) in endpoints)
        {
            var hasValidator = expressionText.Contains("WithValidator");
            
            Assert.True(hasValidator, $"Endpoint PUT em  {file} deve ter um validator configurado via WithValidator(). \nEndpoint {expressionText}");
        }
    }
}