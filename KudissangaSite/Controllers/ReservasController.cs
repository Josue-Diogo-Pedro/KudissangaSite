using iTextSharp.text.pdf;
using iTextSharp.text;
using KudissangaSite.Areas.Identity.Data;
using KudissangaSite.Extensions;
using KudissangaSite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using Microsoft.AspNetCore.Routing;

namespace KudissangaSite.Controllers;

public class ReservasController : Controller
{
    private readonly KudissangaSiteContext _context;
    private BaseFont fonteBase;
    ////private PdfPTable tabela;

    public ReservasController(KudissangaSiteContext context)
    {
        _context = context;
    }

    // GET: Reservas
    [ClaimsAuthorize("Funcionario", "ListarReserva")]
    public async Task<IActionResult> Index()
    {
        var kudissangaSiteContext = _context.Reservas.Include(r => r.Cliente).Include(r => r.Funcionario);
        return View(await kudissangaSiteContext.ToListAsync());
    }

    [ClaimsAuthorize("Usuario", "ListarReserva")]
    public async Task<IActionResult> Solicitacoes()
    {
        var kudissangaSiteContext = _context.Reservas.Include(r => r.Cliente).Include(r => r.Funcionario).Where(r => r.Aprovada == false);
        return View(await kudissangaSiteContext.ToListAsync());
    }

    // GET: Reservas/Details/5
    [ClaimsAuthorize("Usuario", "DetalhesReserva")]
    public async Task<IActionResult> Details(Guid? id)
    {
        if ((id == null && DadosReserva.ReservaId == null) || _context.Reservas == null)
        {
            return NotFound();
        }

        if (id is null)
            id = DadosReserva.ReservaId;

        var reserva = await _context.Reservas
            .Include(r => r.Cliente)
            .Include(r => r.Funcionario)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (reserva == null)
        {
            return NotFound();
        }

        return View(reserva);
    }

    // GET: Reservas/Create
    //[ClaimsAuthorize("Usuario", "CriarReserva")]
    public async Task<IActionResult> Create(Guid? id)
    {
        ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Id");
        ViewData["FuncionarioId"] = new SelectList(_context.Funcionarios, "Id", "Id");

        var suite = await _context.Suites.FindAsync(id);
        var reserva = new Reserva();

        var userCli = await _context.Clientes.Include(p => p.Pessoa).FirstOrDefaultAsync(p => p.Id == UsuarioLogado.IdUsuarioSessao);
        var userFun = await _context.Funcionarios.Include(p => p.Pessoa).FirstOrDefaultAsync(p => p.Id == UsuarioLogado.IdUsuarioSessao);

        if(userCli is not null)
        {
            DadosReserva.ClienteReserva = userCli;
            DadosReserva.IdCliente = userCli.Id;
        } else if (userFun is not null)
        {
            DadosReserva.FuncionarioReserva = userFun;
            DadosReserva.IdFuncionario = userFun.Id;
        }

        if (_context.Reservas.IsNullOrEmpty())
        {
            DadosReserva.NumeroReserva = 1;
        }
        else
        {
            DadosReserva.NumeroReserva = _context.Reservas.OrderBy(p => p.Numero).LastOrDefault().Numero + 1;
        }

        DadosReserva.IdSuite = suite.Id;
        DadosReserva.SuiteReserva = suite;
        DadosReserva.ReservaId = reserva.Id;

        reserva.Numero = DadosReserva.NumeroReserva;

        return View(reserva);
    }

    // POST: Reservas/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    //[ClaimsAuthorize("Usuario", "CriarReserva")]
    public async Task<IActionResult> Create(Reserva reserva)
    {
        if (ModelState.IsValid)
        {
            var imgPrefixo = Guid.NewGuid() + "_";
            if (!await UploadArquivo(reserva.ComprovativoUpload, imgPrefixo))
            {
                return View(reserva);
            }

            reserva.Comprovativo = imgPrefixo + reserva.ComprovativoUpload.FileName;
            
            reserva.DataReserva = DateTime.Now;
            //reserva.Cliente.Id = DadosReserva.IdCliente;
            //reserva.Funcionario.Id = DadosReserva.IdFuncionario;
            //reserva.ClienteId = DadosReserva.IdCliente;
            //reserva.FuncionarioId = DadosReserva.IdFuncionario;
            //reserva.Suite.Id = DadosReserva.IdSuite;
            reserva.SuiteId = DadosReserva.IdSuite;
            reserva.Numero = DadosReserva.NumeroReserva;
            reserva.Valor = DadosReserva.SuiteReserva.PrecoDiario * reserva.DiasEstadia;
            if (!(DadosReserva.ClienteReserva.Pessoa.Nome is null))
                reserva.NomeCliente = DadosReserva.ClienteReserva.Pessoa.Nome;
            else if(!(DadosReserva.FuncionarioReserva.Pessoa.Nome is null))
                reserva.NomeFuncionario = DadosReserva.FuncionarioReserva.Pessoa.Nome??"";

            reserva.Id = DadosReserva.ReservaId;

            _context.Add(reserva);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details));
        }
        //ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Id", reserva.ClienteId);
        //ViewData["FuncionarioId"] = new SelectList(_context.Funcionarios, "Id", "Id", reserva.FuncionarioId);
        return View(reserva);
    }

    // GET: Reservas/Edit/5
    [ClaimsAuthorize("Gerente", "EditarReserva")]
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null || _context.Reservas == null)
        {
            return NotFound();
        }

        var reserva = await _context.Reservas.FindAsync(id);
        if (reserva == null)
        {
            return NotFound();
        }
        //ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Id", reserva.ClienteId);
        //ViewData["FuncionarioId"] = new SelectList(_context.Funcionarios, "Id", "Id", reserva.FuncionarioId);
        return View(reserva);
    }

    // POST: Reservas/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    [ClaimsAuthorize("Gerente", "EditarReserva")]
    public async Task<IActionResult> Edit(Guid id, Reserva reserva)
    {
        if (id != reserva.Id)
        {
            return NotFound();
        }

        var reservaAtualizacao = await _context.Reservas.FindAsync(id);
        if (ModelState.IsValid)
        {
            try
            {
                reserva.Comprovativo = reservaAtualizacao.Comprovativo;

                if (reserva.ComprovativoUpload != null)
                {
                    var imgPrefixo = Guid.NewGuid() + "_";
                    if (!await UploadArquivo(reserva.ComprovativoUpload, imgPrefixo))
                    {
                        return View(reserva);
                    }

                    reservaAtualizacao.Comprovativo = imgPrefixo + reserva.ComprovativoUpload.FileName;
                }

                reservaAtualizacao.DiasEstadia = reserva.DiasEstadia;
                reservaAtualizacao.TipoPagamento = reserva.TipoPagamento;
                reservaAtualizacao.ValorPago = reserva.ValorPago;
                reservaAtualizacao.Aprovada = reserva.Aprovada;
                if (!(DadosReserva.ClienteReserva.Pessoa.Nome is null))
                    reservaAtualizacao.NomeCliente = DadosReserva.ClienteReserva.Pessoa.Nome;
                else if (!(DadosReserva.FuncionarioReserva.Pessoa.Nome is null))
                    reservaAtualizacao.NomeFuncionario = DadosReserva.FuncionarioReserva.Pessoa.Nome ?? "";

                if (reservaAtualizacao.Aprovada == true)
                {
                    var suite = await _context.Suites.FindAsync(reservaAtualizacao.SuiteId);
                    suite.Disponivel = false;

                    _context.Update(suite);
                }

                _context.Update(reservaAtualizacao);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservaExists(reserva.Id))
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
        //ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Id", reserva.ClienteId);
        //ViewData["FuncionarioId"] = new SelectList(_context.Funcionarios, "Id", "Id", reserva.FuncionarioId);
        return View(reserva);
    }

    // GET: Reservas/Delete/5
    [ClaimsAuthorize("Gerente", "EliminarReserva")]
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null || _context.Reservas == null)
        {
            return NotFound();
        }

        var reserva = await _context.Reservas
            .Include(r => r.Cliente)
            .Include(r => r.Funcionario)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (reserva == null)
        {
            return NotFound();
        }

        return View(reserva);
    }

    // POST: Reservas/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [ClaimsAuthorize("Gerente", "EliminarReserva")]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        if (_context.Reservas == null)
        {
            return Problem("Entity set 'KudissangaSiteContext.Reservas'  is null.");
        }
        var reserva = await _context.Reservas.FindAsync(id);
        if (reserva != null)
        {
            _context.Reservas.Remove(reserva);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ReservaExists(Guid id)
    {
        return _context.Reservas.Any(e => e.Id == id);
    }

    private async Task<bool> UploadArquivo(IFormFile arquivo, string imgPrefixo)
    {
        if (arquivo.Length <= 0) return false;

        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagens", imgPrefixo + arquivo.FileName);

        if (System.IO.File.Exists(path))
        {
            ModelState.AddModelError(string.Empty, "Já existe um arquivo com esse nome!");
            return false;
        }

        using (var stream = new FileStream(path, FileMode.Create))
        {
            await arquivo.CopyToAsync(stream);
        }

        return true;
    }

    public async Task<IActionResult> Relatorio()
    {
        var reservas = _context.Reservas.Include(r => r.Cliente).Include(r => r.Funcionario).ToList();
        if (reservas.Count > 0)
        {
            //Configuração do Documento PDF
            var pixPorMm = 72 / 25.2F;
            var pdf = new Document(PageSize.A4, 15 * pixPorMm, 15 * pixPorMm, 15 * pixPorMm, 20 * pixPorMm);
            var nomeArquivo = $"Reservas.{DateTime.Now.ToString("yyyy.MM.dd.HH.mm.ss")}.pdf";
            var arquivo = new FileStream(nomeArquivo, FileMode.Create);
            var writer = PdfWriter.GetInstance(pdf, arquivo);
            pdf.Open();

            fonteBase = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);

            //Adição do Título
            var fonteParagrafo = new iTextSharp.text.Font(fonteBase, 24, iTextSharp.text.Font.NORMAL, BaseColor.Black);
            var titulo = new Paragraph("Relatório das Reservas\n\n", fonteParagrafo);
            titulo.Alignment = Element.ALIGN_LEFT;
            pdf.Add(titulo);

            //Adição da Tabela
            float[] larguraColunas = { 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f };
            PdfPTable tabela = new PdfPTable(8);
            tabela.SetWidths(larguraColunas);
            tabela.DefaultCell.BorderWidth = 0;
            tabela.WidthPercentage = 100;

            CriarCelula("Data da Reserva", tabela);
            CriarCelula("Número", tabela);
            CriarCelula("Valor Pago", tabela);
            CriarCelula("Troco", tabela);
            CriarCelula("Tipo de Pagamento", tabela);
            CriarCelula("Aprovado", tabela);
            CriarCelula("Funcionário", tabela);
            CriarCelula("Cliente", tabela);

            foreach (var reserva in reservas)
            {
                CriarCelula(reserva.DataReserva.ToString(), tabela);
                CriarCelula(reserva.Numero.ToString(), tabela);
                CriarCelula(reserva.ValorPago.ToString(), tabela);
                CriarCelula(reserva.Troco.ToString(), tabela);
                CriarCelula(reserva.TipoPagamento.ToString(), tabela);
                CriarCelula(reserva.Aprovada.ToString(), tabela);
                CriarCelula(reserva.NomeFuncionario, tabela);
                CriarCelula(reserva.NomeCliente, tabela);
            }

            pdf.Add(tabela);

            pdf.Close();
            arquivo.Close();



            //Abre o PDF no visualizadro Padrão
            var caminhoPDF = Path.Combine(Directory.GetCurrentDirectory(), nomeArquivo);
            if (System.IO.File.Exists(caminhoPDF))
            {
                Process.Start(new ProcessStartInfo()
                {
                    Arguments = $"c:/ start {caminhoPDF}",
                    FileName = "cmd.exe",
                    CreateNoWindow = true
                });
            }
        }

        //var kudissangaSiteContext = _context.Reservas.Include(r => r.Cliente).Include(r => r.Funcionario);
        return RedirectToAction("Index");
    }

    private void CriarCelula(string nome, PdfPTable tabela)
    {
        //Adição das celulas de titulo das colunas
        var fonteCelula = new iTextSharp.text.Font(fonteBase, 12, iTextSharp.text.Font.NORMAL, BaseColor.Black);
        var celula = new PdfPCell(new Phrase(nome, fonteCelula));
        celula.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
        celula.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
        celula.Border = 0;
        celula.BorderWidthBottom = 1;
        celula.FixedHeight = 25;

        tabela.AddCell(celula);
    }
}
