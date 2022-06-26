using AppWeb_TinderTec.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json; //serializar y deserializar en json
using System.Data;

namespace AppWeb_TinderTec.Controllers
{
    public class HomeController : Controller
    {
        string cadena;
        private IConfiguration Configuration;

        public HomeController(IConfiguration _configuration)
        {
            Configuration = _configuration;
            cadena = this.Configuration.GetConnectionString("myDbRichardHome");
        }

        public async Task<IActionResult> Index()
        {


            Usuario usu = new Usuario();
            usu = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("_User"));

            ViewBag.nombre = usu.nombres;
            ViewBag.edad = usu.edad;
            ViewBag.fotoURL = usu.foto1;

            return View(await Task.Run(() => usu));
        }

    }
}
