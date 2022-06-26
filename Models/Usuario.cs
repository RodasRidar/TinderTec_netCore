using System.ComponentModel.DataAnnotations;

namespace AppWeb_TinderTec.Models
{

    public class Usuario
    {


        [Display(Name = "Codigo")]
        public int cod_usu { get; set; }

        [Display(Name = "Nombres")]
        public string nombres { get; set; }

        [Display(Name = "Correo")]
        public string email { get; set; }


        [Display(Name = "Contraseña")]
        public string clave { get; set; }


        public string foto1 { get; set; }
        public string foto2 { get; set; }
        public string foto3 { get; set; }
        public string foto4 { get; set; }
        public string foto5 { get; set; }
        public DateTime fecha_naci { get; set; }
        public int edad { get { return ((DateTime.Now - fecha_naci).Days) / 365; } }



        public string carrera { get; set; }
        public int cod_carrera { get; set; }
        public string sede { get; set; }

        public int cod_sede { get; set; }

        public string descripcion { get; set; }

        public string interes { get; set; }

        public int cod_interes { get; set; }

        public int cod_genero { get; set; }

        public string genero { get; set; }

        public int cod_suscrip { get; set; }

        public int intentos { get; set; }

        public DateTime fechabloqueo { get; set; }

    }


}
