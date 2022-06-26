using AppWeb_TinderTec.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;

namespace AppWeb_TinderTec.Controllers
{
    public class BuscarAmistadController : Controller
    {
        string cadena;
        int cod_usu;
        private IConfiguration Configuration;

        public BuscarAmistadController(IConfiguration _configuration)
        {
            Configuration = _configuration;
            cadena = this.Configuration.GetConnectionString("myDbRichardHome");
        }

        private void recuperarUsuario()
        {
            Usuario usu = new Usuario();
            usu = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("_User"));
            cod_usu = usu.cod_usu;
            ViewBag.nombre = usu.nombres;
            ViewBag.edad = usu.edad;
            ViewBag.fotoURL = usu.foto1;
        }

        //Realizar metodos para LA VSITA BUSCAR AMISTAD
        //AUTOR :JORGE  

        //metodo de optener usuario disponible 
        Usuario UsuarioBuscarAmistad()
        {
            Usuario usu = new Usuario();
            usu = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("_User"));

            Usuario temporal = new Usuario();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("USP_USUARIO_LISTAR", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@cod_usu", usu.cod_usu);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {

                    temporal.cod_usu = dr.GetInt32(0);
                    temporal.nombres = dr.GetString(1);
                    temporal.foto1 = dr.GetString(4);
                    temporal.fecha_naci = dr.GetDateTime(9);
                    temporal.descripcion = dr.GetString(14);
                    temporal.carrera = dr.GetString(18);
                    temporal.sede = dr.GetString(19);


                }
                cn.Close();
            }

            return temporal;
        }


        //metodo para insertar el like
        string insertarLike(int cod_usuario2)
        {

            string mensaje = "";
            Usuario usu = new Usuario();
            usu = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("_User"));

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                try
                {

                    SqlCommand cmd = new SqlCommand("USP_INSERTAR_LIKE", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@mensaje", SqlDbType.VarChar, 300).Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("@cod_usu_1", usu.cod_usu);
                    cmd.Parameters.AddWithValue("@cod_usu_2", cod_usuario2);
                    cmd.ExecuteNonQuery();

                    string match = cmd.Parameters["@mensaje"].Value.ToString();
                    if (match == "MATCH")
                    {
                        mensaje = match;
                    }



                }
                catch (Exception e)
                {
                    mensaje = e.Message;//capturo el mensaje de error
                    cn.Close();
                }


            }
            return mensaje;
        }


        //metodo para inserta el dislike
        string insertarDisLike(int cod_usu_2)
        {
            string mensaje = "";
            Usuario usu = new Usuario();
            usu = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("_User"));

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                try
                {

                    SqlCommand cmd = new SqlCommand("USP_INSERTAR_DISLIKE", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@cod_usu", usu.cod_usu);
                    cmd.Parameters.AddWithValue("@cod_usu_2", cod_usu_2);
                    cmd.ExecuteNonQuery();


                    mensaje = "DISLIKE";


                }
                catch (Exception e)
                {
                    mensaje = e.Message;
                    cn.Close();
                }


            }
            return mensaje;
        }


        public IActionResult BuscarAmistad(Usuario usu)
        {
            recuperarUsuario();
            if (usu.cod_usu == 0)
            {

                ViewBag.msjBuscarAmistadNull = "¡Nada que mostrar ,amplia tus preferencias para una mejor experiencia!";
                ViewBag.idValidacion = UsuarioBuscarAmistad().cod_usu;
                return View(UsuarioBuscarAmistad());

            }

            else
            {
                return View(usu);
            }

        }

        //buscar por codigo
        Usuario Buscar(int cod_usua)
        {
            Usuario temporal = new Usuario();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("USP_USUARIO_BUSCAR_ID", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@cod_usu", cod_usua);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {

                    temporal.cod_usu = dr.GetInt32(0);
                    temporal.nombres = dr.GetString(1);
                    temporal.foto1 = dr.GetString(4);
                    temporal.fecha_naci = dr.GetDateTime(9);
                    temporal.descripcion = dr.GetString(14);
                    temporal.carrera = dr.GetString(18);
                    temporal.sede = dr.GetString(19);


                }
                cn.Close();
            }

            return temporal;
        }

        //metodo post like

        [HttpPost]
        public IActionResult Like(int codUsuarioliked)
        {
            string mensaje = insertarLike(codUsuarioliked);
            recuperarUsuario();

            ViewBag.mensajeLike = mensaje;

            if (mensaje != "MATCH")
            {
                return RedirectToAction("BuscarAmistad");
            }
            else
                return View("BuscarAmistad", Buscar(codUsuarioliked));

        }

        // metodo post dislike

        [HttpPost]
        public IActionResult dislike(int codUsuarioDisliked)
        {
            string validacion = insertarDisLike(codUsuarioDisliked);
            recuperarUsuario();


            return RedirectToAction("BuscarAmistad");

        }




        //Realizar metodos para LA VSITA CHATEAR CON MATCHS
        //AUTOR :Richard  


        //-------------------------------------------------------------- MATCH
        public async Task<IActionResult> Matchs()
        {
            recuperarUsuario();

            //ViewBag.lstMatch = lstMatch();
            string MSJdeleteMatchAndMsj;

            if (HttpContext.Session.GetString("MSJdeleteMatchAndMsj") == null || HttpContext.Session.GetString("MSJdeleteMatchAndMsj") == "")
            {

                MSJdeleteMatchAndMsj = "";
            }
            else
            {
                MSJdeleteMatchAndMsj = JsonConvert.DeserializeObject<string>(HttpContext.Session.GetString("MSJdeleteMatchAndMsj"));
            }

            ViewBag.MSJdeleteMatchAndMsj = MSJdeleteMatchAndMsj;

            HttpContext.Session.SetString("MSJdeleteMatchAndMsj", JsonConvert.SerializeObject(""));

            if (lstMatch().Any())
            {
                return View(await Task.Run(() => ViewBag.lstMatch = lstMatch()));

            }
            else
            {
                ViewBag.msjMatchsNULL = " Ve busca amistades 🔎";
                return View(await Task.Run(() => ViewBag.lstMatch = lstMatch()));
            }
            // return View();

        }



        //-------------------------------------------------------------- CHATEAR




        [HttpPost]
        public async Task<IActionResult> Chat(Match match)

        {
            HttpContext.Session.SetString("chat", JsonConvert.SerializeObject(match));


            if (lstChat(match.id).Any())

            {
                recuperarUsuario();
                ViewBag.cod_usu_now = cod_usu;
                ViewBag.foto = match.foto1;
                ViewBag.nom = match.nombres;
                //ViewBag.lstChat = lstChat(match.id);
                ViewBag.id = match.id;

                return View(await Task.Run(() => ViewBag.lstChat = lstChat(match.id)));
            }
            else
            {
                ViewBag.cod_usu_now = cod_usu;
                ViewBag.foto = match.foto1;
                ViewBag.nom = match.nombres;
                ViewBag.msjNULL = "¡Tu match esta esperando, enviale un mensaje 📨!";
                ViewBag.id = match.id;
                return View(await Task.Run(() => ViewBag.lstChat = new List<Chat>()));
            }
        }
        public async Task<IActionResult> Chat()

        {

            recuperarUsuario();
            Match auxiliar = JsonConvert.DeserializeObject<Match>(HttpContext.Session.GetString("chat"));
            ViewBag.cod_usu_now = cod_usu;
            ViewBag.foto = auxiliar.foto1;
            ViewBag.nom = auxiliar.nombres;
            //ViewBag.lstChat = lstChat(auxiliar.id);
            ViewBag.id = auxiliar.id;

            //return View();
            return View(await Task.Run(() => ViewBag.lstChat = lstChat(auxiliar.id)));
        }

        //-------------------------------------------------------------- ENVIAR MENSAJE

        [HttpPost]
        public async Task<IActionResult> EnviarMensaje(int usu_envia, string mensaje)
        {

            ViewBag.mensaje = sendMsj(usu_envia, mensaje);//usar el mensaje para poner verificacion de entrega del mensajes ✅



            return RedirectToAction("Chat");
        }


        //-------------------------------------------------------------- VerPerfil

        [HttpPost]
        public async Task<IActionResult> VerPerfil(int usu_envia)
        {





            return RedirectToAction("Matchs");
        }


        //-------------------------------------------------------------- CANCERLAR MATCH


        [HttpPost]
        public async Task<IActionResult> CancelarMatch(int usu_envia)
        {
            string msj = deleteMatchAndMsj(usu_envia);
            HttpContext.Session.SetString("MSJdeleteMatchAndMsj", JsonConvert.SerializeObject(msj));

            return RedirectToAction("Matchs");
        }


        //-------------------------------------------------------------- METODOS
        IEnumerable<Match> lstMatch()
        {
            //retorna la lista de Usuarios y su credito de cada uno
            List<Match> lstMatch = new List<Match>();
            recuperarUsuario();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("exec USP_LISTAR_MATCH_POR_USUARIO @cod_usu1", cn);
                cmd.Parameters.AddWithValue("@cod_usu1", cod_usu);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lstMatch.Add(new Match()
                    {
                        id = dr.GetInt32(0),
                        nombres = dr.GetString(1),
                        foto1 = dr.GetString(2),


                    });
                }
            }
            return lstMatch;
        }


        IEnumerable<Chat> lstChat(int cod_usu_match)
        {
            List<Chat> lstChat = new List<Chat>();
            recuperarUsuario();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("USP_LISTAR_CHAT_POR_USUARIO", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@cod_usu1", cod_usu);
                cmd.Parameters.AddWithValue("@cod_usu2", cod_usu_match);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {

                    lstChat.Add(new Chat()
                    {
                        cod_chat = dr.GetInt32(0),
                        cod_usu1 = dr.GetInt32(1),
                        cod_usu2 = dr.GetInt32(2),
                        mensaje = dr.GetString(3),
                        fecha = dr.GetDateTime(4),
                        ft_u1 = dr.GetString(5),
                        nombres = dr.GetString(6)

                    });
                }
            }
            return lstChat;
        }


        private string sendMsj(int usu_envia, string mensaje)
        {
            recuperarUsuario();
            string msj = " ";
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("USP_REGISTRAR_CHAT", cn);

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@cod_usu1", cod_usu);
                    cmd.Parameters.AddWithValue("@cod_usu2", usu_envia);
                    cmd.Parameters.AddWithValue("@mensaje", mensaje);
                    cmd.ExecuteNonQuery();
                    msj = "Mensaje enviado";
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

            return msj;
        }


        private string deleteMatchAndMsj(int usu_envia)
        {
            recuperarUsuario();
            string msjConfirmation = " ";
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("USP_ELIMINAR_MATCH_POR_USUARIO", cn);

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@cod_usu1", cod_usu);
                    cmd.Parameters.AddWithValue("@cod_usu2", usu_envia);
                    cmd.ExecuteNonQuery();
                    msjConfirmation = "Match y mensajes eliminados";
                }
                catch (Exception ex)
                {
                    msjConfirmation = ex.Message;
                }

                finally
                {
                    cn.Close();
                }
            }

            return msjConfirmation;
        }
    }


}
