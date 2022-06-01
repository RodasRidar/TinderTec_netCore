using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.Rendering;
using AppWeb_TinderTec.Models;

namespace AppWeb_TinderTec.Controllers
{
    public class BuscarAmistadController : Controller
    {

        string cadena = @"Server =DESKTOP-N47UO59;Database= TinderTecBD;Trusted_Connection = true;MultipleActiveResultSets = True;TrustServerCertificate = False;Encrypt = False";
        //string cadena = @"Server =DESKTOP-ME3NR94;Database= TinderTecBD;Trusted_Connection = true;MultipleActiveResultSets = True;TrustServerCertificate = False;Encrypt = False";

        Usuario CargarUsuario(string id)

        {
           Usuario usu= new Usuario();
            
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("select top 1 nombres,fecha_naci from tb_clientes where cod_usu =@cod", cn);
                cmd.Parameters.AddWithValue("@cod", id);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    usu.nombres = dr.GetString(0);
                    usu.fecha_naci = dr.GetDateTime(1);

                }
               
                cn.Close();
                dr.Close();
                return usu;
            }

        }
        /*
        public async Task<IActionResult> Index()
        {
            Usuario usuario = new Usuario();
            
            //ViewBag.nombres = CargarUsuario("RICAR").First().nombres;

            return View(await Task.Run(() => CargarUsuario("RICAR")));
        }*/

        public async Task<IActionResult> Perfil()
        {
            Usuario usuario = new Usuario();

            //ViewBag.nombres = CargarUsuario("RICAR").First().nombres;

            return View(await Task.Run(() => CargarUsuario("RICAR")));
        }

        public async Task<IActionResult> Chats()
        {
            Usuario usuario = new Usuario();

            //ViewBag.nombres = CargarUsuario("RICAR").First().nombres;

            return View(await Task.Run(() => CargarUsuario("RICAR")));
        }

        public async Task<IActionResult> MeGustas()
        {
            Usuario usuario = new Usuario();

           // //ViewBag.nombres = CargarUsuario("RICAR").First().nombres;

            return View(await Task.Run(() => CargarUsuario("RICAR")));
        }
        public async Task<IActionResult> Subscripcion()
        {
            Usuario usuario = new Usuario();

            //ViewBag.nombres = CargarUsuario("RICAR").First().nombres;

            return View(await Task.Run(() => CargarUsuario("RICAR")));
        }
    }
}
