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
    public class ExperienciasController : Controller
    {
        private readonly TalentConnectBD _context;

        public ExperienciasController(TalentConnectBD context)
        {
            _context = context;
        }

        // GET: Experiencias
        public async Task<IActionResult> Index()
        {
            var talentConnectBD = _context.Experiencias.Include(e => e.Candidato);
            return View(await talentConnectBD.ToListAsync());
        }

        // GET: Experiencias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var experiencia = await _context.Experiencias
                .Include(e => e.Candidato)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (experiencia == null)
            {
                return NotFound();
            }

            return View(experiencia);
        }

        // GET: Experiencias/Create
        public IActionResult Create()
        {
            ViewData["CandidatoID"] = new SelectList(_context.Candidatos, "Id", "Bairro");
            return View();
        }

        // POST: Experiencias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create(string[] Cargos, string[] Empresas, DateTime[] DatasInicio, DateTime[]? DatasFim, string[] Descricoes)
        {
            var candidatoIdString = HttpContext.Session.GetString("CandidatoID");
            if (string.IsNullOrEmpty(candidatoIdString) || !int.TryParse(candidatoIdString, out int CandidatoID))
            {
                return BadRequest("CandidatoID não encontrado na sessão.");
            }

            var candidato = await _context.Candidatos.Include(c => c.Experiencias).FirstOrDefaultAsync(c => c.Id == CandidatoID);
            if (candidato == null)
            {
                return NotFound(); // Se o candidato não for encontrado
            }

            // Verificação de comprimento dos arrays obrigatórios
            if (Cargos.Length == 0 || Empresas.Length == 0 || DatasInicio.Length == 0 || Descricoes.Length == 0)
            {
                ModelState.AddModelError("Erro", "Todos os campos obrigatórios devem ter pelo menos um elemento.");
                return View("~/Views/Registrar/RegistrarExperiencia.cshtml");
            }

            // Verificação de consistência dos arrays
            int minLength = new[] { Cargos.Length, Empresas.Length, DatasInicio.Length, Descricoes.Length }.Min();
            if (DatasFim != null && DatasFim.Length > 0)
            {
                minLength = Math.Min(minLength, DatasFim.Length);
            }

            for (int i = 0; i < minLength; i++)
            {
                // Verificação das datas
                if (DatasFim != null && DatasFim.Length > i && DatasInicio[i] > DatasFim[i])
                {
                    ModelState.AddModelError("DataInicio", $"A data de início não pode ser posterior à data de fim para a experiência {i + 1}.");
                    return View("~/Views/Registrar/RegistrarExperiencia.cshtml");
                }

                var experiencia = new Experiencia
                {
                    Cargo = Cargos[i],
                    Empresa = Empresas[i],
                    DataInicio = DatasInicio[i],
                    DataFim = DatasFim != null && DatasFim.Length > i ? DatasFim[i] : null,
                    Descricao = Descricoes[i],
                    CandidatoID = CandidatoID
                };

                candidato.Experiencias.Add(experiencia);
                _context.Experiencias.Add(experiencia);
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


        // GET: Experiencias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var experiencia = await _context.Experiencias.FindAsync(id);
            if (experiencia == null)
            {
                return NotFound();
            }
            ViewData["CandidatoID"] = new SelectList(_context.Candidatos, "Id", "Bairro", experiencia.CandidatoID);
            return View(experiencia);
        }

        // POST: Experiencias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Cargo,Empresa,DataInicio,DataFim,Descricao,CandidatoID")] Experiencia experiencia)
        {
            if (id != experiencia.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(experiencia);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExperienciaExists(experiencia.Id))
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
            ViewData["CandidatoID"] = new SelectList(_context.Candidatos, "Id", "Bairro", experiencia.CandidatoID);
            return View(experiencia);
        }

        // GET: Experiencias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var experiencia = await _context.Experiencias
                .Include(e => e.Candidato)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (experiencia == null)
            {
                return NotFound();
            }

            return View(experiencia);
        }

        // POST: Experiencias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var experiencia = await _context.Experiencias.FindAsync(id);
            if (experiencia != null)
            {
                _context.Experiencias.Remove(experiencia);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExperienciaExists(int id)
        {
            return _context.Experiencias.Any(e => e.Id == id);
        }
    }
}
