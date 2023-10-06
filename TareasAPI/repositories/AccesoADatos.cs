using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TareasAPI.models;

namespace TareasAPI.repositories
{
    public class AccesoADatos
    {
        private readonly string _path;

        public AccesoADatos(string path)
        {
            _path = path;
        }

        public List<Tarea>? ObtenerTodasLasTareasJson()
        {
            if (File.Exists(_path))
            {
                var json = File.ReadAllText(_path);
                return JsonConvert.DeserializeObject<List<Tarea>>(json);
            }

            return new List<Tarea>();
        }

        public void GuardarTareasJson(List<Tarea> tareas)
        {
            var json = JsonConvert.SerializeObject(tareas);
            File.WriteAllText(_path, json);
        }
    }
}