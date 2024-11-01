using System;
using System.Collections.Generic;

namespace PruebaOctubre.Models
{
    public partial class Population
    {
        public int Id { get; set; }
        public string Country { get; set; } = null!;
        public int Quantity { get; set; }
    }
}
