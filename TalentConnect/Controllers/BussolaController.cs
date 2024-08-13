using Microsoft.AspNetCore.Mvc;

namespace TalentConnect.Controllers
{
    public class BussolaController : Controller
    {
        public IActionResult RegistrarCandidato()
        {
            return View("~/Views/Registrar/RegistrarCandidato.cshtml");
        }
        public IActionResult RegistrarEmpresa()
        {
            return View("~/Views/Registrar/RegistrarEmpresa.cshtml");
        }
        public IActionResult RegistrarExperiencia()
        {
            return View("~/Views/Registrar/RegistrarExperiencia.cshtml");
        } 
        public IActionResult RegistrarFormacao()
        {
            return View("~/Views/Registrar/RegistrarFormacao.cshtml");
        } 
        public IActionResult RegistrarPortfolio()
        {
            return View("~/Views/Registrar/RegistrarPortfolio.cshtml");
        }
    }
}
