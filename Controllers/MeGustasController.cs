using AppWeb_TinderTec.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;

namespace AppWeb_TinderTec.Controllers
{
    public class MeGustasController : Controller
    {
        string cadena;
        int codusu;
        string nom, edad, foto;
        private IConfiguration Configuration;

        public MeGustasController(IConfiguration _configuration)
        {
            Configuration = _configuration;
            cadena = this.Configuration.GetConnectionString("myDbRichardHome");
        }


        private void recuperarUsuario()
        {

            Usuario usu = new Usuario();
            usu = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("_User"));

            codusu = usu.cod_usu;
            ViewBag.nombre = usu.nombres;
            ViewBag.edad = usu.edad;
            ViewBag.fotoURL = usu.foto1;
        }


        [HttpGet]
        public async Task<IActionResult> MeGustas()
        {
            if (listadoUsuarios() == null)
            {
                if (TempData["msjEliminarlike"] != null)
                {
                    ViewBag.Eliminado = TempData["msjEliminarlike"].ToString();
                }
                
                recuperarUsuario();
                return View();
                
            }
            else
            {
                if (TempData["msjEliminarlike"] != null)
                {
                    ViewBag.Eliminado = TempData["msjEliminarlike"].ToString();
                }
                ViewBag.MeGustasVacios = "1";
                recuperarUsuario();
                return View(await Task.Run(() => listadoUsuarios()));
            }

        }

        [HttpPost]
        public async Task<IActionResult> Dislike(int cod_usu)
        {
          string msj= EliminarLike(cod_usu);
            TempData["msjEliminarlike"] = msj;


           return RedirectToAction("MeGustas");
        }

        string EliminarLike(int usu_dislike)
        {
            recuperarUsuario();
            string mensaje = "";
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("USP_DISLIKE_POR_USER", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@cod1", codusu);
                    cmd.Parameters.AddWithValue("@cod2", usu_dislike);
                    cmd.ExecuteNonQuery();
                    mensaje = $"Se elimino el like";
                }
                catch (Exception ex)
                {
                    mensaje = ex.Message;
                }
                finally
                {
                    cn.Close();
                }
                return mensaje;
            }
        }


        [HttpGet]
        public IActionResult buscarAmistad()
        {
            return RedirectToAction("BuscarAmistad", "BuscarAmistad");

        }

        public IEnumerable<Usuario> listadoUsuarios()
        {
            List<Usuario> lstUsuarios = new List<Usuario>();
            recuperarUsuario();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("exec USP_LIKE_POR_USER @cod", cn);
                cmd.Parameters.AddWithValue("@cod", codusu);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lstUsuarios.Add(new Usuario()
                    {
                        cod_usu = dr.GetInt32(0),
                        nombres = dr.GetString(1),
                        sede = dr.GetString(2),
                        carrera = dr.GetString(3),
                        foto1 = dr.GetString(4)
                    });
                }
            }
            return lstUsuarios;
        }

    }
}
