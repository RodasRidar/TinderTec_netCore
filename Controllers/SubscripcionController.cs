using AppWeb_TinderTec.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;

namespace AppWeb_TinderTec.Controllers
{
    public class SubscripcionController : Controller
    {
        /*********************************************************************/


        //VARIABLES GLOBALES

        //Para mi cadena de conexion
        string cadena;
        //Para configurar y tener acceso a mi BD de forma local
        private IConfiguration Configuration;
        //Para recuperar el codigo del usuario en sesion
        int cod_usu_actual;


        /*********************************************************************/


        //Configurando para obtener mi BD local

        public SubscripcionController(IConfiguration _configuration)
        {
            Configuration = _configuration;
            cadena = this.Configuration.GetConnectionString("myDbRichardHome");
        }


        /*********************************************************************/


        //Metodo que me recupera al Usuario en sesion

        private void recuperarUsuario()
        {
            Usuario usu = new Usuario();
            usu = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("_User"));

            //Seteamos nuestra variable global cod_usu_actual con el código
            //de usuario que esta en sesion
            cod_usu_actual = usu.cod_usu;
            ViewBag.nombre = usu.nombres;
            ViewBag.edad = usu.edad;
            ViewBag.fotoURL = usu.foto1;
        }


        /*********************************************************************/

        //Realizar metodos para LA VISTA Subscripcion
        //AUTOR :EDUARDO

        /*********************************************************************/


        //Metodo para listar los planes de subcripcion (Nos retorna una lista)

        public IEnumerable<Concepto_Planes> listadoPlanes()
        {
            List<Concepto_Planes> listaSubscripciones = new List<Concepto_Planes>();

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("exec USP_LISTAR_CONCEPTO_PLANES", cn);

                cn.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    listaSubscripciones.Add(new Concepto_Planes()
                    {
                        cod_plan = dr.GetInt32(0),
                        nom_plan = dr.GetString(1),
                        desc_plan = dr.GetString(2),
                        costo_plan = dr.GetDecimal(3),
                        duracion_plan = dr.GetInt32(4)
                    });
                }
            }
            return listaSubscripciones;
        }


        /*********************************************************************/


        //Metodo que nos devuelve la clase de subscripcion del usuario
        //(Nos retorna un string)

        public String PlanActual()
        {
            string clase_usu_sub = "";
            recuperarUsuario();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("exec USP_CLASE_SUBSCRIPCION_USUARIO @cod_usu ", cn);
                cmd.Parameters.AddWithValue("@cod_usu", cod_usu_actual);
                cn.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    clase_usu_sub = dr.GetString(0).ToString();
                }
            }
            return clase_usu_sub;
        }


        /*********************************************************************/


        //Metodo que nos retorna un objeto de "Concepto_Planes" según su id
        //Este metodo lo vamos usar al momento de Agregar planes al carrito pq al momento
        //de darle agregar necesitamos que nos salga la info del plan seleccionado.

        Concepto_Planes Buscar(int id)
        {
            //Lo que hace aca es según el plan que seleccionemos, nos va retornar
            //un objeto de ese plan con sus datos
            return listadoPlanes().FirstOrDefault(cp => cp.cod_plan.Equals(id));
        }


        /*********************************************************************/

        //GET de Vista "Planes" (Esta vista muestra los planes de subscripcion)

        public async Task<IActionResult> Planes()
        {
            recuperarUsuario();

            //Si es la primera vez que ingresas a Planes, inicializar session canasta almacenando un json
            if (HttpContext.Session.GetString("canasta") == null)
                HttpContext.Session.SetString("canasta", JsonConvert.SerializeObject(new List<Registro_Planes>()));

            //En un ViewBag contenemos la ejecucion del metodo PlanActual()
            //previamente creado, que nos devuelve la clase de subscripcion 
            //en la que se encuentra el usuario actualmente
            ViewBag.plan_actual = await Task.Run(() => PlanActual());

            //Retornamos la vista con el metodo listadoPlanes(), que nos retorna
            //un listado de los planes de Subscripcion
            return View(await Task.Run(() => listadoPlanes()));
        }

        /*********************************************************************/


        //GET de Vista "Agregar" (Esta vista nos muestra un Detalle (Info)
        //del Plan que queremos agregar a nuestro carrito)

        //OJO: Para que esta vista se ejecute es necesario pasarle mediante un 
        //parametro el id de un Plan de Subscripcion sino no se ejecutara

        public async Task<IActionResult> Agregar(int? id = null)
        {
            recuperarUsuario();
            //si id es null, ir a la vista Planes, sino enviar la info del Plan seleccionado
            if (id == null)
                return RedirectToAction("Planes");
            else
                //Ya que enviamos la info de un Plan, retornaremos la vista al
                //momento de que el metodo Buscar() encuentre un objeto Plan con
                //dicho Id (Mas info en el metodo Buscar declarado previamente)
                return View(await Task.Run(() => Buscar((int)id)));
        }

        /*********************************************************************/


        //POST de la vista "Agregar" (En el metodo POST de esta vista lo que
        //necesitamos es obtener tanto el codigo del plan como la cantidad
        //que vamos a comprar de dicho plan, y esta info nos servira para
        //enviarla a nuestro carrito)

        //OJO: Ya que el Get de "Agregar" solo nos devuelve un Detalle (Info) de un
        //objeto Plan, como hariamos para enviar cantidad en el Post? Si no tenemos
        //ese atributo en el objeto Plan. Pues lo que hacemos es crear manualmente
        //en la vista "Agregar" un formulario (Mas info en el Agregar.cshtml)

        [HttpPost]
        public async Task<IActionResult> Agregar(int codigo, int cantidad)
        {
            recuperarUsuario();
            //VERIFICAR QUE NO SE REPITA EL PLAN EN EL CARRITO
            //Deserializar el Session canasta almacenando en una variable auxiliar
            List<Registro_Planes> auxiliar =
             JsonConvert.DeserializeObject<List<Registro_Planes>>(HttpContext.Session.GetString("canasta"));

            if (auxiliar.FirstOrDefault(x => x.cod_plan == codigo) != null)
            {
                //si lo encontro
                ViewBag.mensaje = "El Plan ya se encuentra agregado";
                return View(await Task.Run(() => Buscar(codigo)));
            }

            //si no lo encontro, almacenar dicho plan en un objeto de su clase
            Concepto_Planes plan = Buscar(codigo);

            //Agregar dicho plan a la clase Registro_Planes (Es una clase que
            //simula la funcion de una clase Carrito)
            Registro_Planes item = new Registro_Planes()
            {
                cod_plan = plan.cod_plan,
                nom_plan = plan.nom_plan,
                desc_plan = plan.desc_plan,
                costo_plan = plan.costo_plan,
                duracion_plan = plan.duracion_plan,
                cantidad = cantidad
            };

            //agregar item al auxiliar
            auxiliar.Add(item);

            //almacenar auxiliar en el Session
            HttpContext.Session.SetString("canasta", JsonConvert.SerializeObject(auxiliar));

            ViewBag.mensaje = "Plan registrado";

            //Retornamos la vista cuando el objeto plan termine de crearse
            return View(await Task.Run(() => plan));
        }


        /*********************************************************************/


        //GET de vista "Carrito" (En esta vista vemos un listado de los planes que
        //se han ido agregando en el session de "canasta"). Además tambien podremos
        //eliminar planes que ya habiamos añadido a nuestro carrito, eso gracias
        //a un metodo que hemos creado ("Delete", esta mas abajo)

        public async Task<IActionResult> Carrito()
        {
            recuperarUsuario();
            //el objetivo es listar el contenido del Session canasta
            //1.recuperar el Session y almacenar en una lista
            List<Registro_Planes> auxiliar = JsonConvert.DeserializeObject<List<Registro_Planes>>(
                HttpContext.Session.GetString("canasta"));

            //Si el carrito esta vacio, nos retorna a la vista "Planes"
            if (auxiliar.Count == 0) return RedirectToAction("Planes");

            //Sino nos retorna una vista con el listado de los planes que hay
            //dentro del carrito
            return View(await Task.Run(() => auxiliar));
        }


        /*********************************************************************/


        //Este metodo como tal no, nos retorna una vista, sino que nos redirecciona
        //otra a la vista Carrito peeeeeero en manera de refresh, ya que el fin de este
        //metodo es poder remover planes ya agregados en el carrito

        public IActionResult Delete(int id)
        {
            //Objetivo: Eliminar un plan que ya esta almacenado en el session "canasta"

            //1.recuperar el Session y almacenar en una lista
            List<Registro_Planes> auxiliar = JsonConvert.DeserializeObject<List<Registro_Planes>>(
                HttpContext.Session.GetString("canasta"));

            //2.eliminar el registro que coincidan con el valor de id
            Registro_Planes reg = auxiliar.FirstOrDefault(x => x.cod_plan == id);
            auxiliar.Remove(reg);

            //3.almacenar auxiliar en el Session
            HttpContext.Session.SetString("canasta", JsonConvert.SerializeObject(auxiliar));

            //4.redireccion para refrescar el carrito de planes
            return RedirectToAction("Carrito");
        }


        /*********************************************************************/


        //GET de vista "Subscripcion" (Esta vista nos devuelve un listado de
        //todos los planes que hemos añadido a nuestro carrito).

        public async Task<IActionResult> Subscripcion()
        {
            recuperarUsuario();
            //Obtenemos en una lista los datos de la canasta
            List<Registro_Planes> auxiliar = JsonConvert.DeserializeObject<List<Registro_Planes>>(
                HttpContext.Session.GetString("canasta"));

            //Enviamos a nuestra vista dicha lista con los planes del carrito
            return View(await Task.Run(() => auxiliar));

        }


        /*********************************************************************/


        //POST de Subscripcion (En el metodo POST de "Subscripcion" lo unico que
        //necesitamos obtener es el cod_medio_pago, ya que este atributo lo debemos
        //enviar al momento de insertar una subscripcion en la BD, los demas atributos
        //lo obtenemos directamente de la session de canasta).
        //Luego con el detalle de subscripcion no hay problema en enviar ningun dato
        //porque todos sus atributos lo obtenemos directamente de la canasta.

        [HttpPost]
        public IActionResult Subscripcion(int cod_medio_pago)//variables del formulario
        {
            string mensaje = "";
            //Delcaramos el metodo recuperarUsuario() para poder tener acceso a los
            //datos que retorna este metodo (En realidad solo deseamos obtener
            //el codigo de usuario)
            recuperarUsuario();


            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                SqlTransaction tr = cn.BeginTransaction(IsolationLevel.Serializable);//bloquear los datos de las operaciones
                try
                {
                    //recuperar el contenido del Session canasta en aux
                    List<Registro_Planes> aux = JsonConvert.DeserializeObject<List<Registro_Planes>>(
                                                    HttpContext.Session.GetString("canasta"));


                    //1.ejecutar el procedure de insertar tb_pedido
                    SqlCommand cmd = new SqlCommand("USP_INSERTAR_SUBSCRIPCION", cn, tr);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@cod_sub", SqlDbType.VarChar, 8).Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("@cod_usu", cod_usu_actual);
                    cmd.Parameters.AddWithValue("@monto_total", aux.Sum(x => x.monto_pagar));
                    cmd.Parameters.AddWithValue("@cod_med_pago", cod_medio_pago);
                    cmd.Parameters.AddWithValue("@dias", aux.Sum(x => x.total_dias));
                    cmd.ExecuteNonQuery();

                    //almaceno el valor del parametro @cod_sub
                    String codigo_sub = cmd.Parameters["@cod_sub"].Value.ToString();

                    //2.ejecutar el procedure donde insertamos el detalle de subscripcion
                    aux.ForEach(x =>
                    {
                        cmd = new SqlCommand("exec USP_INSERTAR_DETALLE_SUBSCRIPCION @cod_sub, @cod_plan, @cantidad, @precio, @dias", cn, tr);
                        cmd.Parameters.AddWithValue("@cod_sub", codigo_sub);
                        cmd.Parameters.AddWithValue("@cod_plan", x.cod_plan);
                        cmd.Parameters.AddWithValue("@cantidad", x.cantidad);
                        cmd.Parameters.AddWithValue("@precio", x.costo_plan);
                        cmd.Parameters.AddWithValue("@dias", x.duracion_plan);
                        cmd.ExecuteNonQuery();
                    });

                    //3.ejecutar el procedure donde actualice la clase de subscripcion del usuario
                    aux.ForEach(p =>
                    {
                        cmd = new SqlCommand("exec USP_ACTUALIZAR_CLASE_SUBSCRPICION @cod_usu", cn, tr);

                        cmd.Parameters.AddWithValue("@cod_usu", cod_usu_actual);
                        cmd.ExecuteNonQuery();
                    });

                    //si todo esta OK
                    tr.Commit();
                    mensaje = $"Se ha registrado tu subscripcion";
                }
                catch (Exception ex)
                {
                    mensaje = ex.Message; //capturo el mensaje de error
                    tr.Rollback();
                }
                finally { cn.Close(); }
            }
            return RedirectToAction("Planes");
        }

        /*********************************************************************/

    }
}
