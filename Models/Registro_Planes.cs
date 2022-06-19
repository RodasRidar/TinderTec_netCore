
namespace AppWeb_TinderTec.Models
{
    public class Registro_Planes
    {
        public int cod_plan { get; set; }
        public String nom_plan { get; set; }
        public String desc_plan { get; set; }
        public decimal costo_plan { get; set; }
        public int duracion_plan { get; set; }

        //Campo a introducir al momento de agregar un plan a nuestro carrito
        public int cantidad { get; set; }
        //Campos que se calculan al momento de agregar un plan a nuestro carrito
        public int total_dias { get { return cantidad * duracion_plan; } }
        public decimal monto_pagar { get { return cantidad * costo_plan; } }



    }
}
