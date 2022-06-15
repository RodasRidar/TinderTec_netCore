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
        string cadena;
        private IConfiguration Configuration;

        public UsuarioController(IConfiguration _configuration)
        {
            Configuration = _configuration;
           // cadena = this.Configuration.GetConnectionString("myDbPierina");
            cadena = this.Configuration.GetConnectionString("myDbRichardHome");
        }

        private void recuperarUsuario()
        {
            Usuario usu = new Usuario();
            usu = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("_User"));

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
        [HttpGet]
        public async Task<IActionResult> RegistrarUsuario()
        {

            
            @ViewBag.sedes=new SelectList(await Task.Run(() => Sedes()), "Id", "Name");
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
            if (msj== "Ups!,Ocurrio un problema en el registro")
            {
                @ViewBag.sedes = new SelectList(await Task.Run(() => Sedes()), "Id", "Name");
                @ViewBag.carreras = new SelectList(await Task.Run(() => Carreras()), "Id", "Name");
                @ViewBag.genero = new SelectList(await Task.Run(() => Genero()), "Id", "Name");
                @ViewBag.generoInteres = new SelectList(await Task.Run(() => Interes()), "Id", "Name");
                ViewBag.msjRegistro=msj;
                return View(await Task.Run(() => new Usuario()));
            }
            else
            {
                TempData["msj"] = msj;
                return RedirectToAction("Login", "Seguridad");
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
                    cmd.Parameters.AddWithValue("@nombre",u.nombres );
                    cmd.Parameters.AddWithValue("@correo", u.email);
                    cmd.Parameters.AddWithValue("@fecnac", u.fecha_naci);
                    cmd.Parameters.AddWithValue("@clave", u.clave);
                    cmd.Parameters.AddWithValue("@idsede", u.cod_sede);
                    cmd.Parameters.AddWithValue("@idcarrera", u.cod_carrera);
                    cmd.Parameters.AddWithValue("@idgenero", u.cod_genero);
                    cmd.Parameters.AddWithValue("@idinteres", u.cod_interes);
                    cmd.Parameters.AddWithValue("@desc", u.descripcion);
                    cmd.Parameters.AddWithValue("@f1",u.foto1 );
                    cmd.ExecuteNonQuery();
                    msjConfirmation = "¡Se registro el usuario "+u.nombres+ " correctamente!";
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
    }
}
