using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TalentConnect.Data;
using TalentConnect.Models;

namespace TalentConnect.Controllers
{
    public class FormacaoController : Controller
    {
        private readonly TalentConnectBD _context;

        public FormacaoController(TalentConnectBD context)
        {
            _context = context;
        }

        // GET: Formacaos
        public async Task<IActionResult> Index()
        {
            var talentConnectBD = _context.Formacoes.Include(f => f.Candidato);
            return View(await talentConnectBD.ToListAsync());
        }

        // GET: Formacaos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var formacao = await _context.Formacoes
                .Include(f => f.Candidato)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (formacao == null)
            {
                return NotFound();
            }

            return View(formacao);
        }

        // GET: Formacaos/Create
        public IActionResult Create()
        {
            ViewData["CandidatoID"] = new SelectList(_context.Candidatos, "Id", "Bairro");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string[] Instituicoes, string[] Graus, string[] CamposEstudo, DateTime[] DatasInicio, DateTime[]? DatasFim, bool[]? Cursandos)
        {
            var candidatoIdString = HttpContext.Session.GetString("CandidatoID");
            if (string.IsNullOrEmpty(candidatoIdString) || !int.TryParse(candidatoIdString, out int CandidatoID))
            {
                return BadRequest("CandidatoID não encontrado na sessão.");
            }

            var candidato = await _context.Candidatos.Include(c => c.Formacoes).FirstOrDefaultAsync(c => c.Id == CandidatoID);
            if (candidato == null)
            {
                return NotFound(); // Se o candidato não for encontrado
            }

            if (Instituicoes == null || Graus == null || CamposEstudo == null || DatasInicio == null)
            {
                return BadRequest("Dados de entrada inválidos.");
            }

            // Verifique se todos os arrays obrigatórios têm o mesmo tamanho
            if (Instituicoes.Length != Graus.Length || Instituicoes.Length != CamposEstudo.Length || Instituicoes.Length != DatasInicio.Length)
            {
                return BadRequest("Os arrays de dados obrigatórios têm tamanhos diferentes.");
            }

            for (int i = 0; i < Instituicoes.Length; i++)
            {
                bool cursando = Cursandos != null && Cursandos.Length > i && Cursandos[i];
                DateTime? dataFim = cursando ? (DateTime?)null : (DatasFim != null && DatasFim.Length > i ? DatasFim[i] : null);

                // Validação: Data de Início não pode ser no futuro
                if (DatasInicio[i] > DateTime.Now)
                {
                    ModelState.AddModelError("DataInicio", $"A Data de Início para a formação {i + 1} não pode ser no futuro.");
                    return View("~/Views/Registrar/RegistrarFormacao.cshtml");
                }

                // Validação: Data de Fim não pode ser antes da Data de Início
                if (dataFim.HasValue && dataFim.Value < DatasInicio[i])
                {
                    ModelState.AddModelError("DataFim", $"A Data de Fim para a formação {i + 1} não pode ser anterior à Data de Início.");
                    return View("~/Views/Registrar/RegistrarFormacao.cshtml");
                }

                var formacao = new Formacao
                {
                    Instituicao = Instituicoes[i],
                    Grau = Graus[i],
                    CampoEstudo = CamposEstudo[i],
                    DataInicio = DatasInicio[i],
                    DataFim = dataFim,
                    CandidatoID = CandidatoID
                };

                candidato.Formacoes.Add(formacao);
                _context.Formacoes.Add(formacao);
            }

            await _context.SaveChangesAsync();

            return View("~/Views/Detalhes/PerfilCandidato.cshtml", new CandidatoViewModel
            {
                Candidato = candidato,
                TemExperiencia = candidato.Experiencias?.Any() ?? false,
                TemFormacoes = candidato.Formacoes?.Any() ?? false,
                TemPortfolios = candidato.Portfolios?.Any() ?? false
            });
        }








        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Instituicao,Grau,CampoEstudo,DataInicio,DataFim,CandidatoID")] Formacao formacao)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(formacao);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["CandidatoID"] = new SelectList(_context.Candidatos, "Id", "Bairro", formacao.CandidatoID);
        //    return View(formacao);
        //}

        // GET: Formacaos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var formacao = await _context.Formacoes.FindAsync(id);
            if (formacao == null)
            {
                return NotFound();
            }
            ViewData["CandidatoID"] = new SelectList(_context.Candidatos, "Id", "Bairro", formacao.CandidatoID);
            return View(formacao);
        }

        // POST: Formacaos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Instituicao,Grau,CampoEstudo,DataInicio,DataFim,CandidatoID")] Formacao formacao)
        {
            if (id != formacao.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(formacao);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FormacaoExists(formacao.Id))
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
            ViewData["CandidatoID"] = new SelectList(_context.Candidatos, "Id", "Bairro", formacao.CandidatoID);
            return View(formacao);
        }

        // GET: Formacaos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var formacao = await _context.Formacoes
                .Include(f => f.Candidato)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (formacao == null)
            {
                return NotFound();
            }

            return View(formacao);
        }

        // POST: Formacaos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var formacao = await _context.Formacoes.FindAsync(id);
            if (formacao != null)
            {
                _context.Formacoes.Remove(formacao);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FormacaoExists(int id)
        {
            return _context.Formacoes.Any(e => e.Id == id);
        }
    }
}
