using AppWeb_TinderTec.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;

namespace AppWeb_TinderTec.Controllers
{
    public class UsuarioController : Controller
    {
        int cod_current_usu;
        string email_current_usu;
        string cadena;
        private IConfiguration Configuration;
        const string SesionUsuario = "_User";

        public UsuarioController(IConfiguration _configuration)
        {
            Configuration = _configuration;
            cadena = this.Configuration.GetConnectionString("myDbRichardHome");
        }

        private void recuperarUsuario()
        {
            Usuario usu = new Usuario();
            usu = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString(SesionUsuario));
            email_current_usu = usu.email;
            cod_current_usu = usu.cod_usu;
            ViewBag.nombre = usu.nombres;
            ViewBag.edad = usu.edad;
            ViewBag.fotoURL = usu.foto1;
        }


        [HttpGet]
        public async Task<IActionResult> RegistrarUsuario()
        {


            @ViewBag.sedes = new SelectList(await Task.Run(() => Sedes()), "Id", "Name");
            @ViewBag.carreras = new SelectList(await Task.Run(() => Carreras()), "Id", "Name");
            @ViewBag.genero = new SelectList(await Task.Run(() => Genero()), "Id", "Name");
            @ViewBag.generoInteres = new SelectList(await Task.Run(() => Interes()), "Id", "Name");



            // return View();
            return View(await Task.Run(() => new Usuario()));
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarUsuario(Usuario usu)
        {

            string msj = saveUsuario(usu);
            if (msj == "Ups!,Ocurrio un problema en el registro")
            {
                @ViewBag.sedes = new SelectList(await Task.Run(() => Sedes()), "Id", "Name");
                @ViewBag.carreras = new SelectList(await Task.Run(() => Carreras()), "Id", "Name");
                @ViewBag.genero = new SelectList(await Task.Run(() => Genero()), "Id", "Name");
                @ViewBag.generoInteres = new SelectList(await Task.Run(() => Interes()), "Id", "Name");
                ViewBag.msjRegistro = msj;
                return View(await Task.Run(() => new Usuario()));
            }
            else
            {
                TempData["msj"] = msj;
                return RedirectToAction("Login", "Seguridad");
            }




        }

        [HttpGet]
        public async Task<IActionResult> Mantener()
        {
            //enviar primera particion
            recuperarUsuario();

            Usuario usu = new Usuario();
            usu = UsuarioDatos(cod_current_usu);

            List<Usuario> lstFotos = new List<Usuario>();
            lstFotos.Add(usu);

            //enviar Galeria
            ViewBag.lstfotos = lstFotos;
            ViewBag.nroFotos = 1;
            if (usu.foto2.Length > 1)
            {
                ViewBag.nroFotos = 2;
            }
            if (usu.foto3.Length > 1)
            {
                ViewBag.nroFotos = 3;
            }
            if (usu.foto4.Length > 1)
            {
                ViewBag.nroFotos = 4;
            }
            if (usu.foto5.Length > 1)
            {
                ViewBag.nroFotos = 5;
            }

            //enviar Mantenimiento
            ViewBag.Usuario_mantener = usu;
            ViewBag.interes = new SelectList(await Task.Run(() => InteresSolo()), "Id", "Name");
            ViewBag.sede = new SelectList(await Task.Run(() => SedesSolo()), "Id", "Name");
            ViewBag.carrera = new SelectList(await Task.Run(() => CarrerasSolo()), "Id", "Name");
            if (TempData["msjConfirmacionAddFoto"] != null)
            {
                ViewBag.msjConfirmacionAddFoto = TempData["msjConfirmacionAddFoto"].ToString();
            }


            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Mantener(Usuario user)
        {
            recuperarUsuario();
            string msj = updateUsuario(user, cod_current_usu);
            if (msj == "¡Se actualizo el usuario correctamente!")
            {

                ViewBag.msjConfirmacionEditarPerfil = msj;

                //enviar primera particion
                recuperarUsuario();

                Usuario usu = new Usuario();
                usu = UsuarioDatos(cod_current_usu);

                List<Usuario> lstFotos = new List<Usuario>();
                lstFotos.Add(usu);

                //enviar Galeria
                ViewBag.lstfotos = lstFotos;
                ViewBag.nroFotos = 1;
                if (usu.foto2.Length > 1)
                {
                    ViewBag.nroFotos = 2;
                }
                if (usu.foto3.Length > 1)
                {
                    ViewBag.nroFotos = 3;
                }
                if (usu.foto4.Length > 1)
                {
                    ViewBag.nroFotos = 4;
                }
                if (usu.foto5.Length > 1)
                {
                    ViewBag.nroFotos = 5;
                }

                //enviar Mantenimiento
                ViewBag.Usuario_mantener = usu;
                ViewBag.interes = new SelectList(await Task.Run(() => InteresSolo()), "Id", "Name");
                ViewBag.sede = new SelectList(await Task.Run(() => SedesSolo()), "Id", "Name");
                ViewBag.carrera = new SelectList(await Task.Run(() => CarrerasSolo()), "Id", "Name");
                usu = CargarUsuario(email_current_usu);
                HttpContext.Session.SetString(SesionUsuario, JsonConvert.SerializeObject(usu));
                recuperarUsuario();
            }
            else
            {
                ViewBag.msjConfirmacionEditarPerfil = msj;
                //enviar primera particion
                recuperarUsuario();

                Usuario usu = new Usuario();
                usu = UsuarioDatos(cod_current_usu);

                List<Usuario> lstFotos = new List<Usuario>();
                lstFotos.Add(usu);

                //enviar Galeria
                ViewBag.lstfotos = lstFotos;
                ViewBag.nroFotos = 1;
                if (usu.foto2.Length > 1)
                {
                    ViewBag.nroFotos = 2;
                }
                if (usu.foto3.Length > 1)
                {
                    ViewBag.nroFotos = 3;
                }
                if (usu.foto4.Length > 1)
                {
                    ViewBag.nroFotos = 4;
                }
                if (usu.foto5.Length > 1)
                {
                    ViewBag.nroFotos = 5;
                }

                //enviar Mantenimiento
                ViewBag.Usuario_mantener = usu;
                ViewBag.interes = new SelectList(await Task.Run(() => InteresSolo()), "Id", "Name");
                ViewBag.sede = new SelectList(await Task.Run(() => SedesSolo()), "Id", "Name");
                ViewBag.carrera = new SelectList(await Task.Run(() => CarrerasSolo()), "Id", "Name");
            }



            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AgregarFoto(string url_foto)
        {
            try
            {
                recuperarUsuario();
                int fotoPosicion = 1;
                Usuario currentUsu = UsuarioDatos(cod_current_usu);
                if (currentUsu.foto1.Length > 1)
                {
                    fotoPosicion = 2;
                }
                if (currentUsu.foto2.Length > 1)
                {
                    fotoPosicion = 3;
                }
                if (currentUsu.foto3.Length > 1)
                {
                    fotoPosicion = 4;
                }
                if (currentUsu.foto4.Length > 1)
                {
                    fotoPosicion = 5;
                }
                addphoto(cod_current_usu, fotoPosicion, url_foto);
                TempData["msjConfirmacionAddFoto"] = "¡Foto agregada exitosamente!";

                return RedirectToAction("Mantener");
            }
            catch (Exception ex)
            {
                recuperarUsuario();
                int fotoPosicion = 1;
                Usuario currentUsu = UsuarioDatos(cod_current_usu);
                if (currentUsu.foto1.Length > 1)
                {
                    fotoPosicion = 2;
                }
                if (currentUsu.foto2.Length > 1)
                {
                    fotoPosicion = 3;
                }
                if (currentUsu.foto3.Length > 1)
                {
                    fotoPosicion = 4;
                }
                if (currentUsu.foto4.Length > 1)
                {
                    fotoPosicion = 5;
                }
                addphoto(cod_current_usu, fotoPosicion, url_foto);
                TempData["msjConfirmacionAddFoto"] = "Ocurrio un error al añadir tu foto";

                return RedirectToAction("Mantener");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Eliminar()
        {
            try
            {
                recuperarUsuario();
                eliminarUsuario(cod_current_usu);
                TempData["msj"] = "¡Se elimino la cuenta exitosamente!";
                return RedirectToAction("Login", "Seguridad");
            }
            catch
            {
                TempData["msjConfirmacionAddFoto"] = "¡Ocurrio un error al eliminar la cuenta!";
                return RedirectToAction("Mantener");

            }

        }

        private void addphoto(int id, int posicion, string url)
        {
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("USP_USUARIO_INSERTAR_FOTO", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@codigo_usuario", id);
                cmd.Parameters.AddWithValue("@posicion", posicion);
                cmd.Parameters.AddWithValue("@url_foto", url);
                cmd.ExecuteNonQuery();
                cn.Close();
            }

        }

        private void eliminarUsuario(int id)
        {

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();

                SqlCommand cmd = new SqlCommand("USP_USUARIO_ELIMINAR", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@codigo_usuario", id);
                cmd.ExecuteNonQuery();
                cn.Close();
            }

        }

        private string updateUsuario(Usuario u, int id)
        {

            string msjConfirmation = " ";
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("USP_EDITAR_PERFIL", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nombres_sp", u.nombres);
                    cmd.Parameters.AddWithValue("@cod_usu_sp", id);
                    cmd.Parameters.AddWithValue("@descripcion_sp", u.descripcion);
                    cmd.Parameters.AddWithValue("@cod_interes_sp", u.cod_interes);
                    cmd.Parameters.AddWithValue("@cod_carrera_sp", u.cod_carrera);
                    cmd.Parameters.AddWithValue("@cod_sede_sp", u.cod_sede);
                    cmd.ExecuteNonQuery();
                    msjConfirmation = "¡Se actualizo el usuario correctamente!";
                }
                catch (Exception ex)
                {
                    //msjConfirmation = ex.Message;
                    msjConfirmation = "Ups!,Ocurrio un problema al actualizar";
                }

                finally
                {
                    cn.Close();
                }
            }
            return msjConfirmation;
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

        private string saveUsuario(Usuario u)
        {

            string msjConfirmation = " ";
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("USP_USUARIO_REGISTRAR", cn);

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nombre", u.nombres);
                    cmd.Parameters.AddWithValue("@correo", u.email);
                    cmd.Parameters.AddWithValue("@fecnac", u.fecha_naci);
                    cmd.Parameters.AddWithValue("@clave", u.clave);
                    cmd.Parameters.AddWithValue("@idsede", u.cod_sede);
                    cmd.Parameters.AddWithValue("@idcarrera", u.cod_carrera);
                    cmd.Parameters.AddWithValue("@idgenero", u.cod_genero);
                    cmd.Parameters.AddWithValue("@idinteres", u.cod_interes);
                    cmd.Parameters.AddWithValue("@desc", u.descripcion);
                    cmd.Parameters.AddWithValue("@f1", u.foto1);
                    cmd.ExecuteNonQuery();
                    msjConfirmation = "¡Se registro el usuario " + u.nombres + " correctamente!";
                }
                catch (Exception ex)
                {
                    //msjConfirmation = ex.Message;
                    msjConfirmation = "Ups!,Ocurrio un problema en el registro";
                }

                finally
                {
                    cn.Close();
                }
            }

            return msjConfirmation;
        }

        IEnumerable<Sede> Sedes()
        {

            List<Sede> SedeesList = new List<Sede>();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("exec USP_SEDE_LISTAR", cn);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                SedeesList.Add(new Sede()
                {
                    Id = -1,
                    Name = "Seleccione su sede..."
                });
                while (dr.Read())
                {
                    SedeesList.Add(new Sede()
                    {
                        Id = dr.GetInt32(0),
                        Name = dr.GetString(1)
                    });
                }

            }
            return SedeesList;
        }

        IEnumerable<Genero> Genero()
        {

            List<Genero> genList = new List<Genero>();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("exec USP_GENERO_LISTAR", cn);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                genList.Add(new Genero()
                {
                    Id = -1,
                    Name = "Seleccione su genero..."
                });
                while (dr.Read())
                {
                    genList.Add(new Genero()
                    {
                        Id = dr.GetInt32(0),
                        Name = dr.GetString(1)
                    });
                }
            }
            return genList;
        }

        IEnumerable<Interes> Interes()
        {

            List<Interes> intList = new List<Interes>();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("exec USP_INTERES_LISTAR", cn);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                intList.Add(new Interes()
                {
                    Id = -1,
                    Name = "Seleccione su genero de Interes..."
                });
                while (dr.Read())
                {
                    intList.Add(new Interes()
                    {
                        Id = dr.GetInt32(0),
                        Name = dr.GetString(1)
                    });
                }
            }
            return intList;
        }

        IEnumerable<Carreras> Carreras()
        {

            List<Carreras> carrerasList = new List<Carreras>();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("exec USP_CARRERAS_LISTAR", cn);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                carrerasList.Add(new Carreras()
                {
                    Id = -1,
                    Name = "Seleccione su carrera..."
                });
                while (dr.Read())
                {
                    carrerasList.Add(new Carreras()
                    {
                        Id = dr.GetInt32(0),
                        Name = dr.GetString(1)
                    });
                }
            }
            return carrerasList;
        }

        Usuario UsuarioDatos(int id)

        {
            Usuario usu = new Usuario();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("EXEC USP_USUARIO_DATOS @id", cn);
                cmd.Parameters.AddWithValue("@id", id);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    usu.nombres = dr.GetString(0);
                    usu.foto1 = dr.GetString(1);
                    usu.foto2 = dr.GetString(2);
                    usu.foto3 = dr.GetString(3);
                    usu.foto4 = dr.GetString(4);
                    usu.foto5 = dr.GetString(5);
                    usu.fecha_naci = dr.GetDateTime(6);
                    usu.cod_carrera = dr.GetInt32(7);
                    usu.cod_interes = dr.GetInt32(8);
                    usu.cod_sede = dr.GetInt32(9);
                    usu.descripcion = dr.GetString(10);
                }

                cn.Close();
                dr.Close();
                return usu;
            }

        }

        IEnumerable<Interes> InteresSolo()
        {

            List<Interes> intList = new List<Interes>();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("exec USP_INTERES_LISTAR", cn);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    intList.Add(new Interes()
                    {
                        Id = dr.GetInt32(0),
                        Name = dr.GetString(1)
                    });
                }
            }
            return intList;
        }

        IEnumerable<Carreras> CarrerasSolo()
        {

            List<Carreras> carrerasList = new List<Carreras>();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("exec USP_CARRERAS_LISTAR", cn);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    carrerasList.Add(new Carreras()
                    {
                        Id = dr.GetInt32(0),
                        Name = dr.GetString(1)
                    });
                }
            }
            return carrerasList;
        }
        IEnumerable<Sede> SedesSolo()
        {

            List<Sede> SedeesList = new List<Sede>();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("exec USP_SEDE_LISTAR", cn);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    SedeesList.Add(new Sede()
                    {
                        Id = dr.GetInt32(0),
                        Name = dr.GetString(1)
                    });
                }

            }
            return SedeesList;
        }
    }
}
