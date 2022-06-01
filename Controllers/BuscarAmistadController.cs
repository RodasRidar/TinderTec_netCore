using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.Rendering;
using AppWeb_TinderTec.Models;
using AppWeb_TinderTec.Controllers;
using Newtonsoft.Json;

namespace AppWeb_TinderTec.Controllers
{
    public class BuscarAmistadController : Controller
    {
        string cadena;
        private IConfiguration Configuration;

        public BuscarAmistadController(IConfiguration _configuration)
        {
            Configuration = _configuration;
            cadena = this.Configuration.GetConnectionString("myDbRichardWork");
            //cadena = this.Configuration.GetConnectionString("myDbJorge");
        }

        private void recuperarUsuario()
        {
            Usuario usu = new Usuario();
            usu = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("usuario"));

            ViewBag.nombre = usu.nombres;
            ViewBag.edad = usu.edad;
            ViewBag.fotoURL = usu.foto1;
        }

        //Realizar metodos para LA VSITA BUSCAR AMISTAD
        //AUTOR :JORGE  

        public IActionResult BuscarAmistad()
        {
            recuperarUsuario();
            return View();
        }




        //Realizar metodos para LA VSITA CHATEAR CON MATCHS
        //AUTOR :Richard  
        public IActionResult Chat()
        {
            recuperarUsuario();
            return View();
        }
    }
}
