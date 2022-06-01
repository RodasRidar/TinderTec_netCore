using AppWeb_TinderTec.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppWeb_TinderTec.Controllers
{
    public class SubscripcionController : Controller
    {
        string cadena;
        private IConfiguration Configuration;

        public SubscripcionController(IConfiguration _configuration)
        {
            Configuration = _configuration;
            cadena = this.Configuration.GetConnectionString("myDbEduardo");
        }

        private void recuperarUsuario()
        {
           Usuario usu = new Usuario();
           usu= JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("usuario"));

            ViewBag.nombre = usu.nombres;
            ViewBag.edad = usu.edad;
            ViewBag.fotoURL = usu.foto1;
        }

        //Realizar metodos para LA VISTA Subscripcion
        //AUTOR :EDUARDO
        public IActionResult Subscripcion()
        {
            recuperarUsuario();
            return View();
        }
    }
}
