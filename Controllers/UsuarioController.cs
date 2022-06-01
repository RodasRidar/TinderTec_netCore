using AppWeb_TinderTec.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

namespace AppWeb_TinderTec.Controllers
{
    public class UsuarioController : Controller
    {
        string cadena;
        private IConfiguration Configuration;

        public UsuarioController(IConfiguration _configuration)
        {
            Configuration = _configuration;
           // cadena = this.Configuration.GetConnectionString("myDbPierina");
            cadena = this.Configuration.GetConnectionString("myDbAriana");
        }

        private void recuperarUsuario()
        {
            Usuario usu = new Usuario();
            usu = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("usuario"));

            ViewBag.nombre = usu.nombres;
            ViewBag.edad = usu.edad;
            ViewBag.fotoURL = usu.foto1;
        }
        //Realizar metodos para LA VISTA MANTENER USUARIO 
        //AUTOR :PIERINA
        public IActionResult MantenerUsuario()
        {
            recuperarUsuario();
            return View();
        }



        //Realizar metodos para LA VISTA REGISTRO USUARIO 
        //AUTOR :ARIANA
        public IActionResult RegistrarUsuario()
        {

            return View();
        }
    }
}
