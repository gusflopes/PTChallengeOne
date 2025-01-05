using IntegrationTests.TestFixtures;

namespace IntegrationTests;

[CollectionDefinition("IntegrationTests")]
public class IntegrationTestCollection : ICollectionFixture<IntegrationTestsFixture>
{
}