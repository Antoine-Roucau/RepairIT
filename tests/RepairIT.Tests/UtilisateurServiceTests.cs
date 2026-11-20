using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using RepairIT.Shared;
using RepairIT.UserService.DB;
using RepairIT.UserService.Entite;
using RepairIT.UserService.Services;

namespace RepairIT.Tests;

public class UtilisateurServiceTests
{
    // BD en memoire differente pour chaque test
    private static UserDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<UserDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new UserDbContext(options);
    }

    // logger et configuration simules avec Moq
    private static UtilisateurService CreateService(string? defaultKeycloakUserId = null)
    {
        var logger = new Mock<ILogger<UtilisateurService>>();
        var configuration = new Mock<IConfiguration>();
        configuration.Setup(c => c["Keycloak:UserIdParDefaut"]).Returns(defaultKeycloakUserId);

        return new UtilisateurService(CreateContext(), logger.Object, configuration.Object);
    }

    private static UtilisateurRequest CreateRequest(string email, RoleUtilisateur role = RoleUtilisateur.Gestionnaire) =>
        new("Dupont", "Jean", email, role);

    [Fact]
    public async Task CreateAsync_NewUser_IncreasesCount()
    {
        var service = CreateService();
        var countBefore = (await service.GetAllAsync()).Count;

        await service.CreateAsync(CreateRequest("jean.dupont@email.com"));

        Assert.Equal(countBefore + 1, (await service.GetAllAsync()).Count);
    }

    [Fact]
    public async Task CreateAsync_NewUser_IsActiveWithRequestData()
    {
        var service = CreateService();

        var cree = await service.CreateAsync(CreateRequest("lea.martin@email.com", RoleUtilisateur.Administrateur));

        Assert.NotNull(cree);
        Assert.True(cree.EstActif);
        Assert.Equal("lea.martin@email.com", cree.Email);
        Assert.Equal(RoleUtilisateur.Administrateur, cree.Role);
    }

    [Fact]
    public async Task CreateAsync_WithConfiguration_UsesConfiguredKeycloakUserId()
    {
        var service = CreateService("valeur-de-test");

        var cree = await service.CreateAsync(CreateRequest("jean.dupont@email.com"));

        Assert.Equal("valeur-de-test", cree!.KeycloakUserId);
    }

    [Fact]
    public async Task CreateAsync_WithoutConfiguration_UsesDefaultKeycloakUserId()
    {
        var service = CreateService();

        var cree = await service.CreateAsync(CreateRequest("jean.dupont@email.com"));

        Assert.Equal(Utilisateur.KeycloakUserIdNonSynchronise, cree!.KeycloakUserId);
    }

    [Fact]
    public async Task CreateAsync_EmailAlreadyUsed_ReturnsNull()
    {
        var service = CreateService();
        await service.CreateAsync(CreateRequest("jean.dupont@email.com"));

        // meme email avec des majuscules
        var resultat = await service.CreateAsync(CreateRequest("JEAN.DUPONT@email.com"));

        Assert.Null(resultat);
        Assert.Single(await service.GetAllAsync());
    }

    [Fact]
    public async Task GetByIdAsync_ExistingUser_ReturnsUser()
    {
        var service = CreateService();
        var cree = await service.CreateAsync(CreateRequest("jean.dupont@email.com"));

        var resultat = await service.GetByIdAsync(cree!.Id);

        Assert.NotNull(resultat);
        Assert.Equal("jean.dupont@email.com", resultat.Email);
    }

    [Fact]
    public async Task GetByIdAsync_UnknownUser_ReturnsNull()
    {
        var service = CreateService();

        Assert.Null(await service.GetByIdAsync(9999));
    }
}