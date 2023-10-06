using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TareasAPI.models
{
    public class Tarea
    {
        public int Id { get; set; }
        public string? Titulo { get; set; }
        public string? Descripcion { get; set; }
        public string? Estado { get; set; }
    }
}