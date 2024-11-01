using System;
using System.Collections.Generic;

namespace PruebaOctubre.Models
{
    public partial class Proveedore
    {
        public int ProveedorId { get; set; }
        public string Nit { get; set; } = null!;
        public string RazonSocial { get; set; } = null!;
        public string Direccion { get; set; } = null!;
        public string Ciudad { get; set; } = null!;
        public string Departamento { get; set; } = null!;
        public string Correo { get; set; } = null!;
        public bool? Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string NombreContacto { get; set; } = null!;
        public string CorreoContacto { get; set; } = null!;
    }
}
