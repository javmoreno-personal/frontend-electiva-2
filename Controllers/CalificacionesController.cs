using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Entity.Models;
using UniversidadWeb;

namespace Indra.Web.Controllers
{
	public class CalificacionesController : Controller
	{
        private readonly IConfiguration _configuration;

        public CalificacionesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

		private async Task CreateSelectLists()
		{
			ViewBag.Asignaturas = await GetAsignaturas();
			ViewBag.Estudiantes = await GetEstudiantes();
		}

        public async Task<ViewResult> Index()
		{
			List<Calificacion> calificaciones = await new Client(_configuration["UrlBase"]).GetList<Calificacion>("api/Calificacion/GetAll");
            IQueryable<Calificacion> query = calificaciones.AsQueryable();
			query = query.OrderBy(x => x.Id);
			await CreateSelectLists();
			return View(query.ToList());
		}

		public async Task<ActionResult> Create()
		{
			await CreateSelectLists();
		    return View();
		} 
				
		[HttpPost]
		public async Task<ActionResult> Create(Calificacion calificacion)
		{
            if (!ModelState.IsValid)
				return View(calificacion);

			var client = new Client(_configuration["UrlBase"]);
            var rpta = await client.Post("api/Calificacion", calificacion);
			if (rpta.Codigo == 1)
            {
                return RedirectToAction("Index");
            }
            ModelState.AddModelError(string.Empty, rpta.Mensaje);
			await CreateSelectLists();
			return View(calificacion);
        }
				
		public async Task<ActionResult> Edit(int id)
		{
            var calificacion = await new Client(_configuration["UrlBase"]).GetEntity<Calificacion>($"api/Calificaciones/{id}");
			await CreateSelectLists();
			return View(calificacion);
		}
				
		[HttpPost]
		public async Task<ActionResult> Edit(Calificacion calificacion)
		{
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var client = new Client(_configuration["UrlBase"]);
            var rpta = await client.Put("api/Calificacion", calificacion);
			if (rpta.Codigo == 1)
            {
                return RedirectToAction("Index");
            }
            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            await CreateSelectLists();
            return View(calificacion);
        }

		public async Task<List<SelectListItem>> GetAsignaturas()
		{
			Client client = new(_configuration["UrlBase"]);
			var asignaturas = await client.GetList<Asignatura>("api/Asignatura/GetAll");
			var items = new List<SelectListItem>();
			foreach (Asignatura asignatura in asignaturas)
			{
				items.Add(new SelectListItem
				{
					Value = asignatura.Id.ToString(),
					Text = asignatura.Nombre
				});
			}
			return items;
		}

		public async Task<List<SelectListItem>> GetEstudiantes()
		{
			Client client = new(_configuration["UrlBase"]);
			var estudiantes = await client.GetList<Estudiante>("api/Estudiante/GetAll");
			var items = new List<SelectListItem>();
			foreach (Estudiante estudiante in estudiantes)
			{
				items.Add(new SelectListItem
				{
					Value = estudiante.Id.ToString(),
					Text = $"{estudiante.Nombres} {estudiante.Apellidos}"
				});
			}
			return items;
		}
	}
}

