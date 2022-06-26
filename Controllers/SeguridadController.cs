using AppWeb_TinderTec.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

namespace AppWeb_TinderTec.Controllers
{
    public class SeguridadController : Controller
    {
        string cadena;
        private IConfiguration Configuration;
        const string SesionUsuario = "_User";

        public SeguridadController(IConfiguration _configuration)
        {
            Configuration = _configuration;
            cadena = this.Configuration.GetConnectionString("myDbRichardHome");
        }



        public async Task<IActionResult> Login()
        {
            if (TempData["msj"] != null)
            {
                ViewBag.msjRegistro = TempData["msj"].ToString();
            }


            return View(await Task.Run(() => new Usuario()));
        }

        public async Task<IActionResult> CerrarSesion()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Login", "Seguridad");
        }

        [HttpPost]
        public async Task<IActionResult> Login(Usuario reg)
        {
            //validar ModelState.Isvalid, si no esta validado etiquetas
            if (string.IsNullOrEmpty(reg.email) || string.IsNullOrEmpty(reg.clave))
            {
                ModelState.AddModelError("", "Ingrese los datos solicitado");
                return View(await Task.Run(() => reg));
            }
            //si estan correcto el ingreso ejecutar verifica
            string mensaje = verificar(reg.email, reg.clave);

            if (mensaje != "OK")
            {
                HttpContext.Session.SetString(SesionUsuario, "");
                ModelState.AddModelError("", mensaje);
                return View(await Task.Run(() => reg));
            }

            else
            {
                ModelState.AddModelError("", "");
                Usuario usu = new Usuario();
                usu = CargarUsuario(reg.email);
                HttpContext.Session.SetString(SesionUsuario, JsonConvert.SerializeObject(usu));
                return RedirectToAction("Index", "Home");
            }
        }

        string verificar(string usuario, string clave)
        {
            string mensaje = "";

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("DECLARE @mensaje varchar(60) EXEC usp_usuario_acceso @email, @clave, @mensaje OUTPUT SELECT @mensaje", cn);
                    cmd.Parameters.AddWithValue("@email", usuario);
                    cmd.Parameters.AddWithValue("@clave", clave);

                    cn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        mensaje = dr.GetString(0);
                    }
                }
                catch (Exception ex)
                {
                    mensaje = ex.Message;
                }
                finally
                {
                    cn.Close();
                }
            }
            return mensaje;
        }

        Usuario CargarUsuario(string email)

        {
            Usuario usu = new Usuario();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("EXEC USP_USUARIO_CARGAR @email", cn);
                cmd.Parameters.AddWithValue("@email", email);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {

                    usu.nombres = dr.GetString(0);
                    usu.fecha_naci = dr.GetDateTime(1);
                    usu.foto1 = dr.GetString(2);
                    usu.cod_usu = dr.GetInt32(3);
                    usu.email = dr.GetString(4);
                }

                cn.Close();
                dr.Close();
                return usu;
            }

        }


    }


}
