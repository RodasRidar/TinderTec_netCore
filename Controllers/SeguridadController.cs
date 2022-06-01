using Microsoft.AspNetCore.Mvc;

namespace AppWeb_TinderTec.Controllers
{
    public class SeguridadController : Controller
    {
        string cadena;
        private IConfiguration Configuration;

        public SeguridadController(IConfiguration _configuration)
        {
            Configuration = _configuration;
            cadena = this.Configuration.GetConnectionString("myDbAriana");
        }

        //Realizar metodos para LA VISTA Inicio de Sesion
        //AUTOR :ARIANA
        public IActionResult Login()
        {
            return View();
        }
    }
}
