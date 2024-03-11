using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NARA20241103.Models
{
    public partial class Fiador
    {
        public Fiador()
        {
            DetalleFamiliares = new List<DetalleFamiliares>();
        }

        public int IdFiador { get; set; }
        [Required(ErrorMessage ="El Nombre Del Fiador Es Obligatorio.")]
        public string Nombre { get; set; } = null!;


        [Required(ErrorMessage ="La Fecha Es Obligatoria.")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage ="El Correlativo Es Necesario")]
        public int Correlativo { get; set; }

        [Required(ErrorMessage ="Ingrese La Catidad De Dinero Fiado.")]
        public decimal DineroFiado { get; set; }

        public virtual List<DetalleFamiliares> DetalleFamiliares { get; set; }
    }
}
