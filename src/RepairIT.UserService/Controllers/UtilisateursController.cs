using Microsoft.AspNetCore.Mvc;
using RepairIT.Shared;
using RepairIT.UserService.Services;

namespace RepairIT.UserService.Controllers;

// requetes HTTP pour les utilisateurs
[ApiController]
[Route("api/users")]
public class UtilisateursController(IUtilisateurService utilisateurService) : ControllerBase
{
    // GET /api/users
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<UtilisateurDto>>> GetAll(CancellationToken cancellationToken)
    {
        return Ok(await utilisateurService.GetAllAsync(cancellationToken));
    }

    // GET /api/users/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<UtilisateurDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var utilisateur = await utilisateurService.GetByIdAsync(id, cancellationToken);
        return utilisateur is null ? NotFound() : Ok(utilisateur);
    }

    // POST /api/users (409 si l'email est deja utilise)
    [HttpPost]
    public async Task<ActionResult<UtilisateurDto>> Create(UtilisateurRequest request, CancellationToken cancellationToken)
    {
        var cree = await utilisateurService.CreateAsync(request, cancellationToken);
        if (cree is null)
        {
            return Conflict($"Un utilisateur avec l'email {request.Email} existe déjà");
        }

        return CreatedAtAction(nameof(GetById), new { id = cree.Id }, cree);
    }
}