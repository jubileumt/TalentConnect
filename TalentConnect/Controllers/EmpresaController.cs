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
    public class EmpresaController : Controller
    {
        private readonly TalentConnectBD _context;
        
        public EmpresaController(TalentConnectBD context)
        {
            _context = context;
        }

        // GET: Empresa
        public async Task<IActionResult> Index()
        {
            return View(await _context.Empresas.ToListAsync());
        }

        // GET: Empresa/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empresa = await _context.Empresas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (empresa == null)
            {
                return NotFound();
            }

            return View(empresa);
        }

        // GET: Empresa/Create
        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> EnviarEmail(Empresa empresa)
        {
            var emailService = new Email("smtp.gmail.com", "talentconnectsuporte@gmail.com", "qmknbgwicjsqojjj");
            var emaillist = new List<string> { empresa.Email };
            string subject = "Registro Concluído";
            string body = $"{empresa.Nome}, seu registro como candidato foi concluído com sucesso!";

            await Task.Run(() => emailService.SendEmail(emaillist, subject, body));

            return Ok("Email enviado com sucesso");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CNPJ,DescricaoEmpresa,Localizacao,Site,TamanhoEmpresa,Id,Nome,Email,Senha,DataCadastro,Telefone,CEP,Bairro,Cidade,Estado,Endereco")] Empresa empresa)
        {
            if (ModelState.IsValid)
            {
                bool validarCNPJ = ValidadorCNPJ.ValidaCNPJ(empresa.CNPJ);
                if (!validarCNPJ)
                {
                    ModelState.AddModelError("CNPJ", "CNPJ inválido");
                    return View("~/Views/Registrar/RegistrarEmpresa.cshtml", empresa);
                }

                var empresaExistente = _context.Empresas.FirstOrDefault(e => e.CNPJ == empresa.CNPJ);
                if (empresaExistente != null)
                {
                    ModelState.AddModelError("CNPJ", "CNPJ já cadastrado");
                    return View("~/Views/Registrar/RegistrarEmpresa.cshtml", empresa);
                }

                var EmailExistenteEmpresa = _context.Empresas.FirstOrDefault(e => e.Email == empresa.Email);
                var EmailExistenteCandidato = _context.Candidatos.FirstOrDefault(c => c.Email == empresa.Email);
                if (EmailExistenteEmpresa != null || EmailExistenteCandidato != null )
                {
                    ModelState.AddModelError("Email", "Email já cadastrado");
                    return View("~/Views/Registrar/RegistrarEmpresa.cshtml", empresa);
                }

                bool telefoneValido = ValidarTelefone.ValidarTeleone(empresa.Telefone);
                if (!telefoneValido)
                {
                    ModelState.AddModelError("Telefone", "Telefone inválido");
                    return View("~/Views/Registrar/RegistrarEmpresa.cshtml", empresa);
                }

                var validarSenha = Validadores.ValidarSenha.ValidaSenha(empresa.Senha);
                if (validarSenha != null)
                {
                    ModelState.AddModelError("Senha", validarSenha);
                    return View("~/Views/Registrar/RegistrarEmpresa.cshtml", empresa);
                }

                string senha = empresa.Senha;
                empresa.Senha = Criptografia.GerarHash(senha);
                empresa.DataCadastro = DateTime.Now;

                _context.Add(empresa);
                await _context.SaveChangesAsync();
                return View("~/Views/Home/Index.cshtml");
            }
            return View(empresa);
        }

        // GET: Empresa/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empresa = await _context.Empresas.FindAsync(id);
            if (empresa == null)
            {
                return NotFound();
            }
            return View(empresa);
        }

        // POST: Empresa/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CNPJ,DescricaoEmpresa,Localizacao,Site,TamanhoEmpresa,Id,Nome,Email,Senha,DataCadastro,Telefone,CEP,Bairro,Cidade,Estado,Endereco")] Empresa empresa)
        {
            if (id != empresa.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(empresa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmpresaExists(empresa.Id))
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
            return View(empresa);
        }

        // GET: Empresa/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empresa = await _context.Empresas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (empresa == null)
            {
                return NotFound();
            }

            return View(empresa);
        }

        // POST: Empresa/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var empresa = await _context.Empresas.FindAsync(id);
            if (empresa != null)
            {
                _context.Empresas.Remove(empresa);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmpresaExists(int id)
        {
            return _context.Empresas.Any(e => e.Id == id);
        }
    }
}
