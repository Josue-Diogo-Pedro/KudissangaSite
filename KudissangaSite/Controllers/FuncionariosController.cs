using iTextSharp.text;
using iTextSharp.text.pdf;
using KudissangaSite.Areas.Identity.Data;
using KudissangaSite.Extensions;
using KudissangaSite.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace KudissangaSite.Controllers;

public class FuncionariosController : Controller
{
    private readonly KudissangaSiteContext _context;

    public FuncionariosController(KudissangaSiteContext context)
    {
        _context = context;
    }

    //[ClaimsAuthorize("Funcionario", "ListarFuncionarios")]
    // GET: Funcionarios
    public async Task<IActionResult> Index()
    {
        var kudissangaSiteContext = _context.Funcionarios.Include(f => f.Pessoa);
        return View(await kudissangaSiteContext.ToListAsync());
    }

    // GET: Funcionarios/Details/5
    //[ClaimsAuthorize("Funcionario", "DetalhesFuncionarios")]
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null || _context.Funcionarios == null)
        {
            return NotFound();
        }

        var funcionario = await _context.Funcionarios
            .Include(f => f.Pessoa)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (funcionario == null)
        {
            return NotFound();
        }

        return View(funcionario);
    }

    // GET: Funcionarios/Create
    //[ClaimsAuthorize("Gerente", "CriarFuncionarios")]
    public IActionResult Create()
    {
        ViewData["PessoaId"] = new SelectList(_context.Pessoas, "Id", "Nome");
        return View();
    }

    // POST: Funcionarios/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    //[ClaimsAuthorize("Gerente", "CriarFuncionarios")]
    public async Task<IActionResult> Create(Funcionario funcionario)
    {
        var usuario = await _context.Users.FirstOrDefaultAsync(f => f.Email == funcionario.Email);

        if (ModelState.IsValid && usuario is not null)
        {
            IdentityUserClaim<string> claimUsuario = new IdentityUserClaim<string>();
            claimUsuario.UserId = usuario.Id.ToString();

            if(funcionario.Funcao == Models.ValueObjects.Funcao.Recepcionista)
            {
                claimUsuario.ClaimType = "Usuario, Funcionario, Recepcionista";
                claimUsuario.ClaimValue = "DetalhesCliente, ListarCliente, CriarReserva, ListarReserva, DetalhesReserva, ListarFuncionario, DetalhesFuncionario, ListarSuites, DetalhesSuite";
            } else if(funcionario.Funcao == Models.ValueObjects.Funcao.Gerente)
            {
                claimUsuario.ClaimType = "Usuario, Funcionario, Gerente";
                claimUsuario.ClaimValue = "DetalhesCliente, ListarCliente, EliminarCliente, CriarReserva, ListarReserva, EditarReserva, EliminarReserva, DetalhesReserva, ListarFuncionario, CriarFuncionario, EditarFuncionario, DetalhesFuncionario, ListarSuites, CriarSuite, EditarSuite, EliminarSuite, DetalhesSuite";
            }

            funcionario.Id = Guid.Parse(usuario.Id);
            funcionario.Pessoa.Id = Guid.Parse(usuario.Id);

            _context.Funcionarios.Add(funcionario);
            _context.UserClaims.Add(claimUsuario);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["PessoaId"] = new SelectList(_context.Pessoas, "Id", "Nome", funcionario.PessoaId);
        return View(funcionario);
    }

    // GET: Funcionarios/Edit/5
    //[ClaimsAuthorize("Funcionario", "EditarFuncionarios")]
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null || _context.Funcionarios == null)
        {
            return NotFound();
        }

        var funcionario = await _context.Funcionarios.Include(p => p.Pessoa).FirstOrDefaultAsync(p => p.Id == id);
        if (funcionario == null)
        {
            return NotFound();
        }
        ViewData["PessoaId"] = new SelectList(_context.Pessoas, "Id", "Nome", funcionario.PessoaId);
        return View(funcionario);
    }

    // POST: Funcionarios/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    //[ClaimsAuthorize("Funcionario", "EditarFuncionarios")]
    public async Task<IActionResult> Edit(Guid id, Funcionario funcionario)
    {
        if (id != funcionario.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            var usuario = await _context.Users.FirstOrDefaultAsync(f => f.Email == funcionario.Email);
            funcionario.Pessoa.Id = Guid.Parse(usuario.Id);
            try
            {
                _context.Update(funcionario);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FuncionarioExists(funcionario.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        ViewData["PessoaId"] = new SelectList(_context.Pessoas, "Id", "Nome", funcionario.PessoaId);
        return View(funcionario);
    }

    // GET: Funcionarios/Delete/5
    //[ClaimsAuthorize("Gerente", "EliminarFuncionarios")]
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null || _context.Funcionarios == null)
        {
            return NotFound();
        }

        var funcionario = await _context.Funcionarios
            .Include(f => f.Pessoa)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (funcionario == null)
        {
            return NotFound();
        }

        return View(funcionario);
    }

    // POST: Funcionarios/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    //[ClaimsAuthorize("Gerente", "EliminarFuncionarios")]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        if (_context.Funcionarios == null)
        {
            return Problem("Entity set 'KudissangaSiteContext.Funcionarios'  is null.");
        }
        var funcionario = await _context.Funcionarios.FindAsync(id);
        if (funcionario != null)
        {
            _context.Funcionarios.Remove(funcionario);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool FuncionarioExists(Guid id)
    {
        return _context.Funcionarios.Any(e => e.Id == id);
    }

}
