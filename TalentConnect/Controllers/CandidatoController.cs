using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TalentConnect.Data;
using TalentConnect.Models;
using TalentConnect.Validadores;

namespace TalentConnect.Controllers
{
    public class CandidatoController : Controller
    {
        private readonly TalentConnectBD _context;

        public CandidatoController(TalentConnectBD context)
        {
            _context = context;
        }

        // GET: Candidatoes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Candidatos.ToListAsync());
        }

        // GET: Candidatoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var candidato = await _context.Candidatos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (candidato == null)
            {
                return NotFound();
            }

            return View(candidato);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        private async Task<IActionResult> IdentificarLogin(string email, string senha)
        {
            var candidato = await _context.Candidatos.FirstOrDefaultAsync(c => c.Email == email);
            if (candidato != null && BCrypt.Net.BCrypt.Verify(senha, candidato.Senha))
            {
               return await LoginEfetivo(candidato);
            }

            var empresa = await _context.Empresas.FirstOrDefaultAsync(e => e.Email == email);
            if (empresa != null && BCrypt.Net.BCrypt.Verify(senha, empresa.Senha))
            {
                HttpContext.Session.SetString("EmpresaID", empresa.Id.ToString());
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("Senha", "Email ou senha incorretos");
            return View("~/Views/Home/Index.cshtml");
        }

        private async Task<IActionResult> LoginEfetivo(Candidato candidato)
        {
            HttpContext.Session.SetString("CandidatoID", candidato.Id.ToString());
            if (candidato.Experiencias == null)
            {
                
            }
            return View("~/Views/Home/MenuPrincipal.cshtml",candidato);
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string senha)
        {
            return await IdentificarLogin(email, senha);
        }

        public async Task<IActionResult> EnviarEmail(Candidato candidato)
        {
            var emailService = new Email("smtp.gmail.com", "talentconnectsuporte@gmail.com", "qmknbgwicjsqojjj");
            var emaillist = new List<string> { candidato.Email };
            string subject = "Registro Concluído";
            string body = $"{candidato.Nome}, seu registro como candidato foi concluído com sucesso!";

            await Task.Run(() => emailService.SendEmail(emaillist, subject, body));

            return Ok("Email enviado com sucesso");
        }

        public async Task<IActionResult> Perfil()
        {
            var Candidato = await CandidatoExists(int.Parse(HttpContext.Session.GetString("CandidatoID")));

            bool hasExperiences = Candidato.Experiencias != null && Candidato.Experiencias.Any();
            bool hasFormacoes = Candidato.Formacoes != null && Candidato.Formacoes.Any();
            bool hasPortfolios = Candidato.Portfolios != null && Candidato.Portfolios.Any();

            CandidatoViewModel CandidatoViewModel = new CandidatoViewModel
            {
                Candidato = Candidato,
                TemExperiencia = hasExperiences,
                TemFormacoes = hasFormacoes,
                TemPortfolios = hasPortfolios
            };

            return View("~/Views/Detalhes/PerfilCandidato.cshtml",CandidatoViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CPF,ResumoProfissional,Habilidades,Disponibilidade,PretensaoSalarial,Experiencias,Formacoes,Portfolio,Id,Nome,Email,Senha,DataCadastro,Telefone,CEP,Bairro,Cidade,Estado,Endereco")] Candidato candidato)
        {
            if (ModelState.IsValid)
            {
                bool ValidarCPF = ValidadorCpf.ValidaCPF(candidato.CPF);
                if (!ValidarCPF)
                {
                    ModelState.AddModelError("CPF", "CPF inválido");
                    return View("~/Views/Registrar/RegistrarCandidato.cshtml",candidato);
                }

                var candidatoExistente = _context.Candidatos.FirstOrDefault(c => c.CPF == candidato.CPF);
                if (candidatoExistente != null)
                {
                    ModelState.AddModelError("CPF", "CPF já cadastrado");
                    return View("~/Views/Registrar/RegistrarCandidato.cshtml",candidato);
                }
                
                var EmailExistente = _context.Candidatos.FirstOrDefault(c => c.Email == candidato.Email);
                if (EmailExistente != null)
                {
                    ModelState.AddModelError("Email", "Email já cadastrado");
                    return View("~/Views/Registrar/RegistrarCandidato.cshtml",candidato);
                }

                bool telefone = ValidarTelefone.ValidarTeleone(candidato.Telefone);
                if (telefone == false)
                {
                    ModelState.AddModelError("Telefone", "Telefone invalido");
                    return View("~/Views/Registrar/RegistrarCandidato.cshtml", candidato);
                }

                var ValidarSenha = Validadores.ValidarSenha.ValidaSenha(candidato.Senha);
                if (ValidarSenha != null)
                {
                    ModelState.AddModelError("Senha", ValidarSenha);
                    return View("~/Views/Registrar/RegistrarCandidato.cshtml", candidato);
                }

                EnviarEmail(candidato);

                string senha = candidato.Senha;
                candidato.Senha = Criptografia.GerarHash(senha);
                candidato.DataCadastro = DateTime.Now;

                _context.Add(candidato);
                await _context.SaveChangesAsync();
                return View("~/Views/Home/Index.cshtml");
            }
            return View("~/Views/Registrar/RegistrarCandidato.cshtml", candidato);
        }

        // GET: Candidatoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var candidato = await _context.Candidatos.FindAsync(id);
            if (candidato == null)
            {
                return NotFound();
            }
            return View(candidato);
        }

        // POST: Candidatoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("CPF,ResumoProfissional,Habilidades,Disponibilidade,PretensaoSalarial,Id,Nome,Email,Senha,DataCadastro,Telefone,CEP,Bairro,Cidade,Estado,Endereco")] Candidato candidato)
        //{
        //    if (id != candidato.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(candidato);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!CandidatoExists(candidato.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(candidato);
        //}

        // GET: Candidatoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var candidato = await _context.Candidatos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (candidato == null)
            {
                return NotFound();
            }

            return View(candidato);
        }

        // POST: Candidatoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var candidato = await _context.Candidatos.FindAsync(id);
            if (candidato != null)
            {
                _context.Candidatos.Remove(candidato);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<Candidato> CandidatoExists(int id)
        {
            return await _context.Candidatos.FirstOrDefaultAsync(c => c.Id == id);
        }

    }
}
