using System.Net.Http.Json;
using Core.Entity;
using IntegrationTests.TestFixtures;
using Microsoft.AspNetCore.Mvc.Testing;
using WebAPI.Requests;

namespace IntegrationTests.Tests;

[Collection("IntegrationTests")]
public class ContatosEndpointsIntegrationTests
{
    private readonly HttpClient _client;
    private const string BaseUrl = "/contatos";

    public ContatosEndpointsIntegrationTests(IntegrationTestsFixture fixture)
    {
        _client = fixture.Factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }
    
    [Fact]
    public async Task Should_Validate_request_when_creating_a_contact()
    {
        var request = new CreateContatoRequest("João da Silva", "emailInvalido", "678", "992638484");
        
        var response = await _client.PostAsJsonAsync(BaseUrl, request);
        
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Should_Create_Contato()
    {
        var request = new CreateContatoRequest("João da Silva", "joao@email.com", "67", "992638484");
        
        var response = await _client.PostAsJsonAsync(BaseUrl, request);
        
        response.EnsureSuccessStatusCode();
        
        var createdContact = await response.Content.ReadFromJsonAsync<Contato>();
        Assert.NotNull(createdContact);
        Assert.NotEqual(Guid.Empty, createdContact.Id);
        Assert.Equal(createdContact.Nome, request.Nome);
    }
    
    [Fact]
    public async Task Should_Create_And_Persist_Contato()
    {
        // Arrange
        var request = new CreateContatoRequest("João da Silva", "joao@email.com", "67", "992638484");
        
        // Act - Create
        var createResponse = await _client.PostAsJsonAsync(BaseUrl, request);
        createResponse.EnsureSuccessStatusCode();
        var createdContact = await createResponse.Content.ReadFromJsonAsync<Contato>();

        // Act - Retrieve
        var getResponse = await _client.GetAsync($"{BaseUrl}/{createdContact!.Id}");
        getResponse.EnsureSuccessStatusCode();
        var retrievedContact = await getResponse.Content.ReadFromJsonAsync<Contato>();
        
        // Assert
        Assert.NotNull(retrievedContact);
        Assert.Equal(createdContact.Id, retrievedContact!.Id);
        Assert.Equal(createdContact.Nome, retrievedContact.Nome);
        Assert.Equal(createdContact.Email, retrievedContact.Email);
        Assert.Equal(createdContact.CodigoArea, retrievedContact.CodigoArea);
        Assert.Equal(createdContact.Telefone, retrievedContact.Telefone);
    }
    

    [Fact]
    public async Task Should_List_Contacts()
    {
        await CreateTestContact();
        await CreateTestContact();
        
        var response = await _client.GetAsync(BaseUrl);
        
        response.EnsureSuccessStatusCode();
        
        var contacts = await response.Content.ReadFromJsonAsync<List<Contato>>();
        Assert.NotNull(contacts);
        Assert.NotEmpty(contacts);
        Assert.True(contacts.Count >= 2);
    }

    [Fact]
    public async Task Should_get_contact_by_id()
    {
        var newContact = await CreateTestContact();
        
        var response = await _client.GetAsync($"{BaseUrl}/{newContact.Id}");
        
        response.EnsureSuccessStatusCode();
        var contact = await response.Content.ReadFromJsonAsync<Contato>();
        Assert.NotNull(contact);
        Assert.Equal(newContact.Id, contact!.Id);
        Assert.Equal(newContact.Nome, contact.Nome);
        Assert.Equal(newContact.Email, contact.Email);
        Assert.Equal(newContact.CodigoArea, contact.CodigoArea);
        Assert.Equal(newContact.Telefone, contact.Telefone);
    }
    
    [Fact]
    public async Task Should_return_NotFound_when_getting_non_existing_contact()
    {
        var nonExistingId = Guid.NewGuid();
        
        var response = await _client.GetAsync($"{BaseUrl}/{nonExistingId}");
        
        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Should_return_NotFound_when_updating_non_existing_contact()
    {
        var nonExistingId = Guid.NewGuid();
        
        var request = new UpdateContatoRequest(
            nonExistingId, "Test Name", "test@email.com", "11", "992638484");
        
        var response = await _client.PutAsJsonAsync($"{BaseUrl}/{nonExistingId}", request);
        
        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }
    

    [Fact]
    public async Task Should_Update_Contact()
    {
        var contact = await CreateTestContact();
        var updatedName = "Updated Name";
        contact.Nome = updatedName;
        
        var response = await _client.PutAsJsonAsync($"{BaseUrl}/{contact.Id}", contact);
        response.EnsureSuccessStatusCode();
        
        var getResponse = await _client.GetAsync($"{BaseUrl}/{contact.Id}");
        var updatedContact = await getResponse.Content.ReadFromJsonAsync<Contato>();
        
        // Talvez aqui precisa buscar novamente?
        Assert.NotNull(updatedContact);
        Assert.Equal(contact.Id, updatedContact!.Id);
        Assert.Equal(updatedName, updatedContact.Nome);
        Assert.Equal(contact.Email, updatedContact.Email);
        Assert.Equal(contact.CodigoArea, updatedContact.CodigoArea);
        Assert.Equal(contact.Telefone, updatedContact.Telefone);
    }

    [Fact]
    public async Task Should_Delete_Contact()
    {
        var contact = await CreateTestContact();
        
        var response = await _client.DeleteAsync($"{BaseUrl}/{contact.Id}");
        
        response.EnsureSuccessStatusCode();
        
        var getResponse = await _client.GetAsync($"{BaseUrl}/{contact.Id}");
        Assert.Equal(System.Net.HttpStatusCode.NotFound, getResponse.StatusCode);
    }
    
    [Fact]
    public async Task Should_return_NotFound_when_deleting_non_existing_contact()
    {
        var nonExistingId = Guid.NewGuid();
        
        var response = await _client.DeleteAsync($"{BaseUrl}/{nonExistingId}");
        
        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }

    private async Task<Contato> CreateTestContact()
    {
        var request = new CreateContatoRequest($"Test Contact {Guid.NewGuid()}", $"test_{Guid.NewGuid()}@email.com",
            "11", "992638484");
        
        var response = await _client.PostAsJsonAsync(BaseUrl, request);
        
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Contato>() ?? throw new InvalidOperationException("Falha ao criar o contato.");
    }
    
}