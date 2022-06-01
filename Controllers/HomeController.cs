using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.Rendering;
using AppWeb_TinderTec.Models;
using Newtonsoft.Json; //serializar y deserializar en json

namespace AppWeb_TinderTec.Controllers
{
    public class HomeController : Controller
    {
        string cadena;
        private IConfiguration Configuration;

        public HomeController(IConfiguration _configuration)
        {
            Configuration = _configuration;
            cadena = this.Configuration.GetConnectionString("myDbRichardWork");
        }

        Usuario CargarUsuario(string id)

        {
            Usuario usu = new Usuario();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("select top 1 nombres,fecha_naci ,foto1 from tb_usuario where cod_usu =@cod", cn);
                cmd.Parameters.AddWithValue("@cod", id);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    usu.nombres = dr.GetString(0);
                    usu.fecha_naci = dr.GetDateTime(1);
                    usu.foto1 = dr.GetString(2);
                }

                cn.Close();
                dr.Close();
                return usu;
            }

        }

        public async Task<IActionResult> Index()
        {


                Usuario usu = new Usuario();
                usu = CargarUsuario("1");
                HttpContext.Session.SetString("usuario", JsonConvert.SerializeObject(usu));

                ViewBag.nombre = usu.nombres;
                ViewBag.edad = usu.edad;
                ViewBag.fotoURL = usu.foto1;

            return View(await Task.Run(() => usu));
        }

    }
}
