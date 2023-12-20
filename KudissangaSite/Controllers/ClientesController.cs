using KudissangaSite.Areas.Identity.Data;
using KudissangaSite.Extensions;
using KudissangaSite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace KudissangaSite.Controllers;

public class ClientesController : Controller
{
    private readonly KudissangaSiteContext _context;

    public ClientesController(KudissangaSiteContext context)
    {
        _context = context;
    }

    //[ClaimsAuthorize("Gerente", "ListarClientes")]
    // GET: Clientes
    public async Task<IActionResult> Index()
    {
        var usuarioCli = await _context.Clientes.FindAsync(UsuarioLogado.IdUsuarioSessao);
        var usuarioFunc = await _context.Funcionarios.FindAsync(UsuarioLogado.IdUsuarioSessao);

        if (usuarioCli is null && usuarioFunc is null)
        {
            return RedirectToAction(nameof(Create));
        }
        else
        {
            var kudissangaSiteContext1 = _context.Clientes.Include(c => c.Pessoa);
            var kudissangaSiteContext2 = _context.Clientes.Include(c => c.Pessoa).Where(c => c.Id == UsuarioLogado.IdUsuarioSessao);

            if (usuarioFunc is not null)
                return View(await kudissangaSiteContext1.ToListAsync());
            else
                return View(await kudissangaSiteContext2.ToListAsync());
        }
    }

    //[ClaimsAuthorize("Usuario", "DetalhesCliente")]
    // GET: Clientes/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null || _context.Clientes == null)
        {
            return NotFound();
        }

        var cliente = await _context.Clientes
            .Include(c => c.Pessoa)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (cliente == null)
        {
            return NotFound();
        }

        return View(cliente);
    }

    [Authorize]
    // GET: Clientes/Create
    public IActionResult Create()
    {
        ViewData["PessoaId"] = new SelectList(_context.Pessoas, "Id", "Nome");
        return View();
    }

    // POST: Clientes/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> Create(Cliente cliente)
    {
        var usuario = UsuarioLogado.IdUsuarioSessao;
        if (ModelState.IsValid)
        {

            IdentityUserClaim<string> claimUsuario = new IdentityUserClaim<string>();
            claimUsuario.UserId = usuario.ToString();
            claimUsuario.ClaimType = "Usuario, Cliente";
            claimUsuario.ClaimValue = "DetalhesCliente, CriarCliente, EditarCliente, CriarReserva, DetalhesReserva";

            cliente.Id = usuario;
            cliente.Pessoa.Id = usuario;

            _context.Clientes.Add(cliente);
            _context.UserClaims.Add(claimUsuario);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        ViewData["PessoaId"] = new SelectList(_context.Pessoas, "Id", "Nome", cliente.PessoaId);
        return View(cliente);
    }

    // GET: Clientes/Edit/5
    //[ClaimsAuthorize("Cliente", "EditarCliente")]
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null || _context.Clientes == null)
        {
            return NotFound();
        }

        var cliente = await _context.Clientes.Include(p => p.Pessoa).FirstOrDefaultAsync(p => p.Id == id);
        if (cliente == null)
        {
            return NotFound();
        }
        ViewData["PessoaId"] = new SelectList(_context.Pessoas, "Id", "Nome", cliente.PessoaId);
        return View(cliente);
    }

    // POST: Clientes/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    //[ClaimsAuthorize("Cliente", "EditarCliente")]
    public async Task<IActionResult> Edit(Guid id, Cliente cliente)
    {
        if (id != cliente.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            cliente.Pessoa.Id = UsuarioLogado.IdUsuarioSessao;
            try
            {
                _context.Update(cliente);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClienteExists(cliente.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        return RedirectToAction(nameof(Index));

        ViewData["PessoaId"] = new SelectList(_context.Pessoas, "Id", "Nome", cliente.PessoaId);
        return View(cliente);
    }

    // GET: Clientes/Delete/5
    //[ClaimsAuthorize("Gerente", "EliminarCliente")]
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null || _context.Clientes == null)
        {
            return NotFound();
        }

        var cliente = await _context.Clientes
            .Include(c => c.Pessoa)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (cliente == null)
        {
            return NotFound();
        }

        return View(cliente);
    }

    // POST: Clientes/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    //[ClaimsAuthorize("Gerente", "EliminarCliente")]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        if (_context.Clientes == null)
        {
            return Problem("Entity set 'KudissangaSiteContext.Clientes'  is null.");
        }
        var cliente = await _context.Clientes.FindAsync(id);
        var user = await _context.Users.FindAsync(id);
        var claimUser = await _context.UserClaims.FindAsync(id);

        if (cliente != null)
        {
            _context.Clientes.Remove(cliente);
            _context.Users.Remove(user);
            _context.UserClaims.Remove(claimUser);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ClienteExists(Guid id)
    {
        return _context.Clientes.Any(e => e.Id == id);
    }
}
