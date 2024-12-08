using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Agenda.Managers;
using Agenda.Managers.Entidades;
using Agenda.Managers.Repos;
using Agenda.Web.Models;
using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;

namespace Agenda.Web.Controllers
{
    public class ContactosController : Controller
    {
        private IContactoManager _contactoManager;

        public ContactosController(IContactoManager contactoManager)
        {
            _contactoManager = contactoManager;
        }

        // GET: ContactosController
        public ActionResult Index()
        {
            var contactos = _contactoManager.GetContactos();
            return View(contactos);
        }

        // GET: ContactosController/Details/5
        public ActionResult Details(int id)
        {
            var contacto = _contactoManager.GetContacto(id);

            // Pasar directamente el Contacto a la vista
            return View(contacto);
        }


        // GET: ContactosController/Create
        public ActionResult Create()
        {
            //ContactoModel contactoModel= new ContactoModel();
            //contactoModel.model = null;
            return View(new Contacto());
        }

        // POST: ContactosController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Contacto contacto)
        {
            // Validación del teléfono
            if (string.IsNullOrWhiteSpace(contacto.Telefono) || !Regex.IsMatch(contacto.Telefono, @"^\d{1,15}$"))
            {
                ModelState.AddModelError("Telefono", "El teléfono debe contener solo números y tener entre 1 y 15 dígitos.");
            }

            // Validación del email 
            if (string.IsNullOrEmpty(contacto.Email) || !contacto.Email.Contains("@"))
            {
                ModelState.AddModelError("Email", "El correo electrónico debe ser válido y contener un '@'.");
            }

            if (ModelState.IsValid)
            {
                _contactoManager.CrearContacto(contacto);
                TempData["SuccessMessage"] = "Contacto creado correctamente.";
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = "Por favor, corrija los errores indicados.";
            return View(contacto);
        }



        // GET: ContactosController/Edit/5
        public ActionResult Edit(int id)
        {
            var contacto = _contactoManager.GetContacto(id);
            if (contacto == null)
            {
                return NotFound();
            }

            return View(contacto);
        }


        // POST: Contactos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Contacto contacto)
        {
            if (id != contacto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _contactoManager.ActualizarContacto(contacto); // Método que actualiza el contacto
                    TempData["SuccessMessage"] = "Contacto actualizado correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    TempData["ErrorMessage"] = "Hubo un error al actualizar el contacto.";
                    return View(contacto);
                }
            }
            return View(contacto);
        }

        // GET: Contactos/Delete/5
        public IActionResult Delete(int id)
        {
            var contacto = _contactoManager.GetContacto(id);
            if (contacto == null)
            {
                return NotFound();  
            }
            return View(contacto);  
        }


        // POST: Contactos/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var result = _contactoManager.EliminarContacto(id); 

            if (result)
            {
                TempData["SuccessMessage"] = "El contacto se eliminó correctamente.";  
                return RedirectToAction(nameof(Index)); 
            }
            else
            {
                TempData["ErrorMessage"] = "Hubo un error al eliminar el contacto.";  
                return RedirectToAction(nameof(Index));  
            }
        }

        // Acción para ver los contactos eliminados
        public IActionResult Restore()
        {
            var contactosEliminados = _contactoManager.ObtenerContactosEliminados();
            return View(contactosEliminados);
        }

        // Acción para restaurar un contacto eliminado
        public IActionResult RestoreDeleted(int id)
        {
            var contactoRestaurado = _contactoManager.RestaurarContacto(id);
            if (contactoRestaurado)
            {
                TempData["SuccessMessage"] = "Contacto restaurado correctamente.";
            }
            else
            {
                TempData["ErrorMessage"] = "Hubo un error al restaurar el contacto.";
            }
            return RedirectToAction("Index");
        }




    }
}

