using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Agenda.Managers.Entidades;
using System.Data.SqlClient;
using System.Data;

namespace Agenda.Managers.Repos
{
    public interface IContactoRepository
    {
        Contacto GetContacto(int IdContacto);
        IEnumerable<Contacto> GetContactos(bool? SoloActivos = true);
        int CrearContacto(Contacto contacto);
        bool ModificarContacto(int IdContacto, Contacto contacto);
        bool EliminarContacto(int IdContacto);
        IEnumerable<Contacto> ObtenerContactosEliminados();
        bool RestaurarContacto(int id);

    }

    public class ContactoRepository : IContactoRepository
    {
        private string _connectionString;

        public ContactoRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        //Consulta por id
        public Contacto GetContacto(int IdContacto)
        {
            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                string sql = "SELECT * FROM Grupo2.NBUGGENTHIN_Contactos WHERE Id = @IdContacto AND Activo = 1";

                Contacto result = conn.QuerySingleOrDefault<Contacto>(sql, new { IdContacto });

                return result;
            }
        }


        //Consulta, lista de contactos
        public IEnumerable<Contacto> GetContactos(bool? SoloActivos = true)
        {
            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Grupo2.NBUGGENTHIN_Contactos ";
                if (SoloActivos == true)
                    query += "WHERE Activo = 1";  
                IEnumerable<Contacto> results = conn.Query<Contacto>(query);
                return results;

            }
        }

        //Consulta, lista de contactos eliminados
        public IEnumerable<Contacto> ObtenerContactosEliminados()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = @"SELECT * FROM Grupo2.NBUGGENTHIN_Contactos WHERE Activo = 0";
                return db.Query<Contacto>(query).ToList();
            }
        }

        // Restaurar un contacto
        public bool RestaurarContacto(int id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = @"UPDATE Grupo2.NBUGGENTHIN_Contactos SET Activo = 1 WHERE Id = @Id";
                return db.Execute(query, new { Id = id }) == 1;
            }
        }



        //Crear nuevo Contacto
        public int CrearContacto(Contacto contacto)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = @"
            INSERT INTO Grupo2.NBUGGENTHIN_Contactos (Nombre, Telefono, Email, Activo)
            VALUES (@Nombre, @Telefono, @Email, 1);  
            SELECT CAST(SCOPE_IDENTITY() AS INT)";

                contacto.Id = db.QuerySingle<int>(query, contacto);

                return contacto.Id;
            }
        }

        //Modificar Contacto
        public bool ModificarContacto(int IdContacto, Contacto contacto)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = @"UPDATE 
                            Grupo2.NBUGGENTHIN_Contactos
                          SET 
                            Nombre = @Nombre,
                            Telefono = @Telefono,
                            Email = @Email
                          WHERE Id = @IdContacto AND Activo = 1"; 

                var parameters = new
                {
                    Nombre = contacto.Nombre,
                    Telefono = contacto.Telefono,
                    Email = contacto.Email,
                    IdContacto = IdContacto 
                };

                return db.Execute(query, parameters) == 1;
            }
        }


        //Soft Delete 
        public bool EliminarContacto(int IdContacto)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = @"UPDATE
                                    Grupo2.NBUGGENTHIN_Contactos
                                 SET 
                                    Activo = 0
                                    WHERE Id = @IdContacto AND Activo = 1";

                return db.Execute(query, new { IdContacto }) == 1;
            }
        }
    }
}
