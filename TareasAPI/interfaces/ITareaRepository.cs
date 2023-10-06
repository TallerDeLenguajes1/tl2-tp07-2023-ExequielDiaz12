using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TareasAPI.models;

namespace TareasAPI.interfaces
{
    public interface ITareaRepository
    {
        IEnumerable<Tarea> GetAllTareas();
        Tarea? GetTarea(int id);
        void CreateTarea(Tarea tarea);
        bool UpdateTarea(int id, Tarea tarea);
        bool DeleteTarea(int id);
        
    }
}