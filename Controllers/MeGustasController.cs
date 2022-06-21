using AppWeb_TinderTec.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AppWeb_TinderTec.Controllers
{
    public class MeGustasController : Controller
    {
        string cadena;
        private IConfiguration Configuration;

        public MeGustasController(IConfiguration _configuration)
        {
            Configuration = _configuration;
            cadena = this.Configuration.GetConnectionString("myDbJorge");
        }


        private void recuperarUsuario()
        {
            Usuario usu = new Usuario();
            usu = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("_User"));

            ViewBag.nombre = usu.nombres;
            ViewBag.edad = usu.edad;
            ViewBag.fotoURL = usu.foto1;
        }

        //Realizar metodos para LA VISTA  MANTENER ME GUSTAS
        //AUTOR :Hansel
        public IActionResult Megustas()
        {
            recuperarUsuario();
            return View();
        }
    }
}
