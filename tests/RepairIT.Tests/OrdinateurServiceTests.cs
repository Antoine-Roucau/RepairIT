using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using RepairIT.CatalogService.DB;
using RepairIT.CatalogService.Services;
using RepairIT.Shared;

namespace RepairIT.Tests;

public class OrdinateurServiceTests
{
    // BD en memoire differente pour chaque test, logger simule avec Moq
    private static OrdinateurService CreateService()
    {
        var options = new DbContextOptionsBuilder<CatalogDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var logger = new Mock<ILogger<OrdinateurService>>();
        return new OrdinateurService(new CatalogDbContext(options), logger.Object);
    }

    private static OrdinateurRequest CreateRequest(string marque) =>
        new(marque, "Modele Test", "Intel Core i7", 16, 512, TypeOrdinateur.Portable, 999m, 5);

    [Fact]
    public async Task CreateAsync_NewOrdinateur_IncreasesCount()
    {
        var service = CreateService();
        var countBefore = (await service.GetAllAsync()).Count;

        await service.CreateAsync(CreateRequest("Asus"));

        Assert.Equal(countBefore + 1, (await service.GetAllAsync()).Count);
    }

    [Fact]
    public async Task UpdateAsync_ExistingOrdinateur_UpdatesData()
    {
        var service = CreateService();
        var cree = await service.CreateAsync(CreateRequest("Asus"));

        var resultat = await service.UpdateAsync(cree.Id, CreateRequest("Dell"));

        Assert.True(resultat);
        Assert.Equal("Dell", (await service.GetByIdAsync(cree.Id))!.Marque);
    }

    [Fact]
    public async Task UpdateAsync_UnknownOrdinateur_ReturnsFalse()
    {
        var service = CreateService();

        Assert.False(await service.UpdateAsync(9999, CreateRequest("Dell")));
    }

    [Fact]
    public async Task DeleteAsync_ExistingOrdinateur_RemovesIt()
    {
        var service = CreateService();
        var cree = await service.CreateAsync(CreateRequest("Asus"));

        var resultat = await service.DeleteAsync(cree.Id);

        Assert.True(resultat);
        Assert.Null(await service.GetByIdAsync(cree.Id));
    }

    [Fact]
    public async Task DeleteAsync_UnknownOrdinateur_ReturnsFalse()
    {
        var service = CreateService();

        Assert.False(await service.DeleteAsync(9999));
    }
}