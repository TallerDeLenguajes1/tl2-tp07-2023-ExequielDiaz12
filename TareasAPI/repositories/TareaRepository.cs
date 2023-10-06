using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using TareasAPI.interfaces;
using TareasAPI.models;

namespace TareasAPI.repositories
{
    public class TareaRepository : ITareaRepository
    {
        private readonly IDbConnection _dbConnection;
        public TareaRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public IEnumerable<Tarea> GetAllTareas()
        {
            using(var connection = _dbConnection)
            {
                connection.Open();
                var query = "SELECT * FROM tarea";
                using(var command = new SQLiteCommand(query, (SQLiteConnection)connection))
                {
                    using(var reader = command.ExecuteReader())
                    {
                        var tareas = new List<Tarea>();
                        while(reader.Read())
                        {
                            tareas.Add(new Tarea
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Titulo = reader["Titulo"].ToString(),
                                Descripcion = reader["Descripcion"].ToString(),
                                Estado = reader["Estado"].ToString()
                            });
                        }
                        return tareas;
                    }
                }
            }
        }
    
        public Tarea? GetTarea(int id)
        {
            using(var connection = _dbConnection)
            {
                connection.Open();
                var query = "SELECT * FROM tarea WHERE Id = @Id";
                using(var command = new SQLiteCommand(query,(SQLiteConnection)connection))
                {
                    command.Parameters.Add(new SQLiteParameter("@Id",id));
                    using(var reader = command.ExecuteReader())
                    {
                        if(reader.Read())
                        {
                            return new Tarea
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Titulo = reader["Titulo"].ToString(),
                                Descripcion = reader["Descripcion"].ToString(),
                                Estado = reader["Estado"].ToString()
                            };
                        }
                        return null;
                    }
                }
            }
        }
    
        public void CreateTarea(Tarea tarea)
        {
            using (var connection = _dbConnection)
            {
                connection.Open();
                var query = "INSERT INTO tarea (Titulo, Descripcion, Estado) VALUES (@Titulo, @Descripcion, @Estado)";
                using(var command = new SQLiteCommand(query,(SQLiteConnection)connection))
                {
                    command.Parameters.Add(new SQLiteParameter("@Titulo",tarea.Titulo));
                    command.Parameters.Add(new SQLiteParameter("@Descripcion",tarea.Descripcion));
                    command.Parameters.Add(new SQLiteParameter("@Estado",tarea.Estado));
                    command.ExecuteNonQuery();
                }
            }
        }
    
        public bool UpdateTarea(int id, Tarea tarea)
        {
            using(var connection = _dbConnection)
            {
                connection.Open();
                var query = "UPDATE tarea SET Titulo = @Titulo, Descripcion = @Descripcion, Estado = @Estado WHERE Id = @Id";
                using(var command = new SQLiteCommand(query,(SQLiteConnection)connection))
                {
                    command.Parameters.Add(new SQLiteParameter("@Id",id));
                    command.Parameters.Add(new SQLiteParameter("@Titulo",tarea.Titulo));
                    command.Parameters.Add(new SQLiteParameter("@Descripcion",tarea.Descripcion));
                    command.Parameters.Add(new SQLiteParameter("@Estado",tarea.Estado));
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }
    
        public bool DeleteTarea(int id)
        {
            using(var connection = _dbConnection)
            {
                var query="DELETE FROM tarea WHERE Id = @Id";
                using(var command = new SQLiteCommand(query,(SQLiteConnection)connection))
                {
                    command.Parameters.Add(new SQLiteParameter("@Id",id));
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }
    
    }
}