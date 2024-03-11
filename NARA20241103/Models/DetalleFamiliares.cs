using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NARA20241103.Models
{
    public partial class DetalleFamiliares
    {
        public int IdDetalleFamilia { get; set; }

        [Required(ErrorMessage ="El Nombre Del Familiar Es Obligatorio")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage ="Ingrese El Parentesco Que Tienen.")]
        public string Parentesco { get; set; } = null!;


        [Required(ErrorMessage ="El Telefono Es Obligatorio")]
        public string Telefono { get; set; } = null!;

        [Required(ErrorMessage = "El Dui Es Obligatorio")]
        [StringLength(10,MinimumLength =10,ErrorMessage ="Ingrese el Numero De DUI con su Guion.")]
        public string Dui { get; set; } = null!;
        public int? FiadorDetalle { get; set; }

        public virtual Fiador? FiadorDetalleNavigation { get; set; }
    }
}
