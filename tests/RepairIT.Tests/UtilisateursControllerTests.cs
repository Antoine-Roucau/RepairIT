using Microsoft.AspNetCore.Mvc;
using Moq;
using RepairIT.Shared;
using RepairIT.UserService.Controllers;
using RepairIT.UserService.Services;

namespace RepairIT.Tests;

// le service est simule avec Moq pour ne tester que les codes HTTP
public class UtilisateursControllerTests
{
    private readonly Mock<IUtilisateurService> _service = new();
    private readonly UtilisateurRequest _request = new("Dupont", "Jean", "jean.dupont@email.com", RoleUtilisateur.Gestionnaire);

    private UtilisateursController CreateController() => new(_service.Object);

    private static UtilisateurDto CreateDto(int id) =>
        new(id, "non-synchronise", "Dupont", "Jean", "jean.dupont@email.com", RoleUtilisateur.Gestionnaire, true);

    [Fact]
    public async Task Create_FreeEmail_Returns201()
    {
        _service.Setup(s => s.CreateAsync(_request, It.IsAny<CancellationToken>())).ReturnsAsync(CreateDto(1));

        var resultat = await CreateController().Create(_request, CancellationToken.None);

        Assert.IsType<CreatedAtActionResult>(resultat.Result);
    }

    [Fact]
    public async Task Create_EmailAlreadyUsed_Returns409()
    {
        _service.Setup(s => s.CreateAsync(_request, It.IsAny<CancellationToken>())).ReturnsAsync((UtilisateurDto?)null);

        var resultat = await CreateController().Create(_request, CancellationToken.None);

        Assert.IsType<ConflictObjectResult>(resultat.Result);
    }

    [Fact]
    public async Task GetById_ExistingUser_Returns200()
    {
        _service.Setup(s => s.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(CreateDto(1));

        var resultat = await CreateController().GetById(1, CancellationToken.None);

        Assert.IsType<OkObjectResult>(resultat.Result);
    }

    [Fact]
    public async Task GetById_UnknownUser_Returns404()
    {
        _service.Setup(s => s.GetByIdAsync(9999, It.IsAny<CancellationToken>())).ReturnsAsync((UtilisateurDto?)null);

        var resultat = await CreateController().GetById(9999, CancellationToken.None);

        Assert.IsType<NotFoundResult>(resultat.Result);
    }
}