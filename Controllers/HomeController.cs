using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.Rendering;
using AppWeb_TinderTec.Models;
using Microsoft.Extensions.Configuration;

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
            
          

            ViewBag.nombre = CargarUsuario("1").nombres;
            ViewBag.edad= CargarUsuario("1").edad;
            ViewBag.fotoURL = CargarUsuario("1").foto1;


            return View(await Task.Run(() => CargarUsuario("1")));
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
    }
}
