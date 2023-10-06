using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TareasAPI.interfaces;
using TareasAPI.models;
using TareasAPI.repositories;

namespace TareasAPI.Controllers
{
    [Route("[controller]")]
    public class TareaController : Controller
    {
        private readonly ILogger<TareaController> _logger;
        private readonly ITareaRepository _tareaRepository;
        private readonly AccesoADatos _accesoADatos;
        public TareaController(ILogger<TareaController> logger, ITareaRepository tareaRepository, AccesoADatos accesoADatos)
        {
            _logger = logger;
            _tareaRepository = tareaRepository;
            _accesoADatos = accesoADatos;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Tarea>> ObtenerTodasLasTareasJson()
        {
            var tareas = _accesoADatos.ObtenerTodasLasTareasJson();
            return Ok(tareas);
        }

        [HttpPost]
        public ActionResult<int> CrearTareas([FromBody] Tarea tarea)
        {
            
            var tareas = _accesoADatos.ObtenerTodasLasTareasJson();
            tarea.Id = tareas.Max(t=>t.Id) + 1;
            tareas.Add(tarea);

            _accesoADatos.GuardarTareasJson(tareas);
            return Ok(tarea);
        }

        [HttpGet]
        public IActionResult GetAllTareas()
        {
            try
            {
                var tareas = _tareaRepository.GetAllTareas();
                return Ok(tareas);
            }
            catch (System.Exception)
            {
                _logger.LogError("Error al solicitar todas las tareas");
                throw;
            }
        }

        [HttpGet("Completada")]
        public IActionResult GetAllTareasCompletadas()
        {
            try
            {
                var tareasCompletadas = _tareaRepository.GetAllTareas().Where(t=> t.Estado == "Completado");
                return Ok(tareasCompletadas);
            }
            catch (System.Exception)
            {
                _logger.LogWarning("No se pudo obtener las tareas completadas");
                throw;
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var tarea = _tareaRepository.GetTarea(id);

                if (tarea == null)
                {
                    _logger.LogWarning($"Intento de acceder a la tarea con id {id} pero no se encontro");
                }

                return Ok(tarea);
            }
            catch (System.Exception)
            {
                _logger.LogError($"Error al acceder a la tarea con id {id}");
                throw;
            }
        }
    
        [HttpPost]
        public IActionResult Post([FromBody]Tarea tarea){
            try
            {   
                tarea.Estado= "Pendiente";
                _tareaRepository.CreateTarea(tarea);
                return CreatedAtAction(nameof(Get),new{id = tarea.Id}, tarea);
            }
            catch (System.Exception)
            {
                _logger.LogError($"Error al Crear la una tarea");
                throw;
            }
        }
    
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Tarea tarea)
        {
            try
            {
                if (_tareaRepository.UpdateTarea(id,tarea))
                {
                    return NoContent();
                }else{
                    _logger.LogWarning($"no se pudo editar la tarea con id {id}");
                    return NotFound();
                }
            }
            catch (System.Exception)
            {
                _logger.LogError($"error al actualizar la tarea con id {id}");
                throw;
            }
        }
    
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                if(_tareaRepository.DeleteTarea(id))
                {
                    return NoContent();
                }else{
                    _logger.LogWarning($"no se pudo borrar la tarea con id {id}");
                    return NotFound();
                }
            }
            catch (System.Exception)
            {
                _logger.LogError($"error al actualizar la tarea con id {id}");
                throw;
            }
        }

        [HttpPut("{id}")]
        public IActionResult CambiarEstadoAEnProgreso(int id)
        {
            var tarea = _tareaRepository.GetTarea(id);
            if(tarea == null)
            {
                _logger.LogError($"No existe el estado de la tarea con id{id}");
                return NotFound();
            }

            if (tarea.Estado == "En Progreso")
            {
                _logger.LogWarning($"No se puede cambiar el estado de la tarea con id {id}");
                return BadRequest("No se puede realizar esta accion");
            }

            tarea.Estado = "En Progreso";
            _tareaRepository.UpdateTarea(id,tarea);
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult CambiarEstadoACompletado(int id)
        {
            var tarea = _tareaRepository.GetTarea(id);
            if(tarea == null)
            {
                _logger.LogError($"No existe el estado de la tarea con id{id}");
                return NotFound();
            }

            if (tarea.Estado == "Completado")
            {
                _logger.LogWarning($"No se puede cambiar el estado de la tarea con id {id}");
                return BadRequest("No se puede realizar esta accion");
            }

            tarea.Estado = "Completado";
            _tareaRepository.UpdateTarea(id,tarea);
            return Ok();
        }
    }
}