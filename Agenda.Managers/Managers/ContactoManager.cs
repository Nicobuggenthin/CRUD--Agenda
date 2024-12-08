using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agenda.Managers.Entidades;
using Agenda.Managers.Repos;

namespace Agenda.Managers
{
    public interface IContactoManager
    {
        IEnumerable<Contacto> GetContactos();
        Contacto GetContacto(int id);
        int CrearContacto(Contacto contacto);
        bool ModificarContacto(int id, Contacto contacto);
        bool ActualizarContacto(Contacto contacto);
        bool EliminarContacto(int id);
        IEnumerable<Contacto> ObtenerContactosEliminados();
        bool RestaurarContacto(int id);
    }

    public class ContactoManager : IContactoManager
    {
        private readonly IContactoRepository _repo;

        public ContactoManager(IContactoRepository repo)
        {
            _repo = repo;
        }


        // Obtener todos los contactos
        public IEnumerable<Contacto> GetContactos()
        {
            return _repo.GetContactos();
        }

        // Obtener un contacto por su Id
        public Contacto GetContacto(int id)
        {
            return _repo.GetContacto(id);
        }

        //Obtener un contacto Eliminado
        public IEnumerable<Contacto> ObtenerContactosEliminados()
        {
            return _repo.ObtenerContactosEliminados();
        }

        //Restaurar contacto Eliminado
        public bool RestaurarContacto(int id)
        {
            return _repo.RestaurarContacto(id);
        }

        // Crear un contacto
        public int CrearContacto(Contacto contacto)
        {
            contacto.Activo = true;
            return _repo.CrearContacto(contacto);
        }

        // Modificar un contacto
        public bool ModificarContacto(int id, Contacto contacto)
        {
            var contactoEnDb = _repo.GetContacto(id);
            if (contactoEnDb == null) return false;

            contactoEnDb.Nombre = contacto.Nombre;
            contactoEnDb.Telefono = contacto.Telefono;
            contactoEnDb.Email = contacto.Email;

            return _repo.ModificarContacto(id, contactoEnDb);
            return _repo.ModificarContacto(id, contacto);

        }

        //Actualizar Contacto
        public bool ActualizarContacto(Contacto contacto)
        {
            if (contacto == null) throw new ArgumentNullException(nameof(contacto));

            return _repo.ModificarContacto(contacto.Id, contacto);
        }

        // Eliminar un contacto
        public bool EliminarContacto(int id)
        {
            return _repo.EliminarContacto(id);
        }
    }
}