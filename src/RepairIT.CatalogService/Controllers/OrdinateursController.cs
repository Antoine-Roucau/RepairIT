using Microsoft.AspNetCore.Mvc;
using RepairIT.CatalogService.Services;
using RepairIT.Shared;

namespace RepairIT.CatalogService.Controllers;

// requetes HTTP pour les ordinateurs
[ApiController]
[Route("api/ordinateurs")]
public class OrdinateursController(IOrdinateurService ordinateurService) : ControllerBase
{
    // GET /api/ordinateurs
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<OrdinateurDto>>> GetAll(CancellationToken cancellationToken)
    {
        return Ok(await ordinateurService.GetAllAsync(cancellationToken));
    }

    // GET /api/ordinateurs/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<OrdinateurDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var ordinateur = await ordinateurService.GetByIdAsync(id, cancellationToken);
        return ordinateur is null ? NotFound() : Ok(ordinateur);
    }

    // POST /api/ordinateurs
    [HttpPost]
    public async Task<ActionResult<OrdinateurDto>> Create(OrdinateurRequest request, CancellationToken cancellationToken)
    {
        var cree = await ordinateurService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = cree.Id }, cree);
    }

    // PUT /api/ordinateurs/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, OrdinateurRequest request, CancellationToken cancellationToken)
    {
        return await ordinateurService.UpdateAsync(id, request, cancellationToken) ? NoContent() : NotFound();
    }

    // DELETE /api/ordinateurs/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        return await ordinateurService.DeleteAsync(id, cancellationToken) ? NoContent() : NotFound();
    }
}