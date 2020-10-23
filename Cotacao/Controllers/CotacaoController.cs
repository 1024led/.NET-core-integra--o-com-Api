using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cotacao.Models;
using CotacaoDatabase.Models;

namespace Cotacao.Controllers
{
    public class CotacaoController : Controller
    {
        private readonly CotacaoContext _context;

        public CotacaoController(CotacaoContext context)
        {
            _context = context;
        }

        // GET: Cotacao
        public async Task<IActionResult> Index()
        {
            return View(await _context.Cotacao.ToListAsync());
        }

        // GET: Cotacao/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cotacao = await _context.Cotacao
                .FirstOrDefaultAsync(m => m.CotacaoId == id);
            if (cotacao == null)
            {
                return NotFound();
            }

            return View(cotacao);
        }

        // GET: Cotacao/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Data")] Cotacao.Models.Cotacao cotacao)
        {

            Models.Services.RequisicaoService requisicao = new Models.Services.RequisicaoService();

            DateTime dataT = cotacao.Data;            
            requisicao.Dia = Convert.ToString(cotacao.Data.Day, 10);
            requisicao.Mes = Convert.ToString(cotacao.Data.Month, 10);
            requisicao.Ano = Convert.ToString(cotacao.Data.Year, 10);

            requisicao.VerificaFimSemana(cotacao.Data);

            cotacao.Data = dataT;

            requisicao.moeda = "USD";

            CotacaoService coletor = CotacaoService.Coletar(requisicao).Result;


            cotacao.MoedaOrigem = "Dolar";
            cotacao.MoedaDestino = "Real";

            cotacao.ValorCompra = coletor.cotacaoCompra;
            cotacao.ValorVenda = coletor.cotacaoVenda;
            

            if (!CotacaoExists(cotacao.Data)) 
            {
                if ((ModelState.IsValid) & (cotacao.ValorCompra != null))
                {
                    _context.Add(cotacao);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return View("errorData");
                    
                }
                
            }
            else
            {
                return View("errorExist");
                
            }

            return View(cotacao);

        }


        // GET: Cotacao/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cotacao = await _context.Cotacao
                .FirstOrDefaultAsync(m => m.CotacaoId == id);
            if (cotacao == null)
            {
                return NotFound();
            }

            return View(cotacao);
        }

        // POST: Cotacao/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cotacao = await _context.Cotacao.FindAsync(id);
            _context.Cotacao.Remove(cotacao);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CotacaoExists(DateTime data)
        {
            return _context.Cotacao.Any(e => e.Data.Date == data.Date);
        }
    }
}
