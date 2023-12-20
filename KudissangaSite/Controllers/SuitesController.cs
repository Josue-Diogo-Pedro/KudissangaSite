using KudissangaSite.Areas.Identity.Data;
using KudissangaSite.Extensions;
using KudissangaSite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KudissangaSite.Controllers;

public class SuitesController : Controller
{
    private readonly KudissangaSiteContext _context;

    public SuitesController(KudissangaSiteContext context)
    {
        _context = context;
    }

    // GET: Suites
    [ClaimsAuthorize("Funcionario", "ListarSuites")]
    public async Task<IActionResult> Index()
    {
        return View(await _context.Suites.ToListAsync());
    }

    // GET: Suites/Details/5
    [ClaimsAuthorize("Funcionario", "DetalhesSuites")]
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null || _context.Suites == null)
        {
            return NotFound();
        }

        var suite = await _context.Suites
            .FirstOrDefaultAsync(m => m.Id == id);
        if (suite == null)
        {
            return NotFound();
        }

        return View(suite);
    }

    // GET: Suites/Create
    [ClaimsAuthorize("Gerente", "CriarSuite")]
    public IActionResult Create()
    {
        return View();
    }

    // POST: Suites/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    [ClaimsAuthorize("Gerente", "CriarSuite")]
    public async Task<IActionResult> Create(Suite suite)
    {
        if (ModelState.IsValid)
        {
            var imgPrefixo = Guid.NewGuid() + "_";
            if(!await UploadArquivo(suite.ImagemUpload, imgPrefixo))
            {
                return View(suite);
            }

            suite.Imagem = imgPrefixo + suite.ImagemUpload.FileName;

            _context.Add(suite);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(suite);
    }

    // GET: Suites/Edit/5
    [ClaimsAuthorize("Gerente", "EditarSuite")]
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null || _context.Suites == null)
        {
            return NotFound();
        }

        var suite = await _context.Suites.FindAsync(id);
        if (suite == null)
        {
            return NotFound();
        }
        return View(suite);
    }

    // POST: Suites/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    [ClaimsAuthorize("Gerente", "EditarSuite")]
    public async Task<IActionResult> Edit(Guid id, Suite suite)
    {
        if (id != suite.Id) return NotFound();

        var suiteAtualizacao = await _context.Suites.FindAsync(id);
        if (ModelState.IsValid)
        {
            try
            {
                suite.Imagem = suiteAtualizacao.Imagem;

                if(suite.ImagemUpload != null)
                {
                    var imgPrefixo = Guid.NewGuid() + "_";
                    if (!await UploadArquivo(suite.ImagemUpload, imgPrefixo))
                    {
                        return View(suite);
                    }

                    suiteAtualizacao.Imagem = imgPrefixo + suite.ImagemUpload.FileName;
                }

                suiteAtualizacao.Numero = suite.Numero;
                suiteAtualizacao.Descricao = suite.Descricao;
                suiteAtualizacao.Capacidade = suite.Capacidade;
                suiteAtualizacao.PrecoDiario = suite.PrecoDiario;
                suiteAtualizacao.DataCadastro = suite.DataCadastro;
                suiteAtualizacao.Disponivel = suite.Disponivel;

                _context.Update(suiteAtualizacao);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SuiteExists(suite.Id))
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
        return View(suite);
    }

    // GET: Suites/Delete/5
    [ClaimsAuthorize("Gerente", "EliminarSuite")]
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null || _context.Suites == null)
        {
            return NotFound();
        }

        var suite = await _context.Suites
            .FirstOrDefaultAsync(m => m.Id == id);
        if (suite == null)
        {
            return NotFound();
        }

        return View(suite);
    }

    // POST: Suites/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [ClaimsAuthorize("Gerente", "EliminarSuite")]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        if (_context.Suites == null)
        {
            return Problem("Entity set 'KudissangaSiteContext.Suites'  is null.");
        }
        var suite = await _context.Suites.FindAsync(id);
        if (suite != null)
        {
            _context.Suites.Remove(suite);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool SuiteExists(Guid id)
    {
        return _context.Suites.Any(e => e.Id == id);
    }

    private async Task<bool> UploadArquivo(IFormFile arquivo, string imgPrefixo)
    {
        if (arquivo.Length <= 0) return false;

        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagens", imgPrefixo + arquivo.FileName);

        if (System.IO.File.Exists(path)){
            ModelState.AddModelError(string.Empty, "Já existe um arquivo com esse nome!");
            return false;
        }

        using(var stream = new FileStream(path, FileMode.Create))
        {
            await arquivo.CopyToAsync(stream);
        }

        return true;
    }
}
