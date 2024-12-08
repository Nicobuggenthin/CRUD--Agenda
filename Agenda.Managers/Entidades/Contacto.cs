using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agenda.Managers.Entidades
{
    public class Contacto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        [RegularExpression(@"^[0-9]{1,15}$", ErrorMessage = "El teléfono debe contener solo números y debe tener entre 1 y 15 dígitos.")]
        [StringLength(15, MinimumLength = 1, ErrorMessage = "El teléfono debe tener entre 1 y 15 caracteres.")]
        public string Telefono { get; set; }


        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "El correo electrónico debe ser válido.")]
        public string Email { get; set; }
        public bool Activo { get; set; }

    }
}
