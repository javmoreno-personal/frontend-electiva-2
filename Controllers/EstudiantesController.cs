using Microsoft.AspNetCore.Mvc;
using Entity.Models;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.AspNetCore.Mvc.Rendering;
using UniversidadWeb;

namespace Indra.Web.Controllers
{
	public class EstudiantesController : Controller
	{
		private readonly IConfiguration _configuration;

		public EstudiantesController(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public async Task<ViewResult> Index()
		{
			Client client = new(_configuration["UrlBase"]);
			List<Estudiante> estudiantes = await client.GetList<Estudiante>("api/Estudiante/GetAll");
			return View(estudiantes);
		}

		public async Task<ActionResult> Create()
		{
			ViewBag.Generos = await GetGeneros();
			return View();
		}

		[HttpPost]
		public async Task<ActionResult> Create(Estudiante estudiante)
		{
			if (!ModelState.IsValid)
				return ValidationProblem(ModelState);

			var client = new Client(_configuration["UrlBase"]);
			var rpta = await client.Post("api/Estudiante", estudiante);
			if (rpta.Codigo == 1)
			{
				return RedirectToAction("Index");
			}
			ModelState.AddModelError(string.Empty, rpta.Mensaje);
			return View(estudiante);
		}
		
		public async Task<ViewResult> Details(int id)
		{
			var estudiante = await new Client(_configuration["UrlBase"]).GetEntity<Estudiante>($"api/Estudiante/GetById?Id={id}");
            return View(estudiante);
		}

		public async Task<ActionResult> Edit(int id)
		{
            var estudiante = await new Client(_configuration["UrlBase"]).GetEntity<Estudiante>($"api/Estudiante/GetById?Id={id}");
			ViewBag.Generos = await GetGeneros();
            return View(estudiante);
		}

		[HttpPost]
		public async Task<ActionResult> Edit(Estudiante estudiante)
		{
			if (!ModelState.IsValid)
				return ValidationProblem(ModelState);

            var client = new Client(_configuration["UrlBase"]);
			var rpta = await client.Put("api/Estudiante", estudiante);
			if (rpta.Codigo == 1)
            {
                return RedirectToAction("Index");
            }
            ModelState.AddModelError(string.Empty, rpta.Mensaje);
            return View(estudiante);
        }

		public async Task<List<SelectListItem>> GetGeneros()
		{
			Client client = new(_configuration["UrlBase"]);
			var generos = await client.GetList<Genero>("api/Genero/GetAll");
			var items = new List<SelectListItem>();
			foreach (Genero genero in generos)
			{
				items.Add(new SelectListItem
				{
					Value = genero.Id.ToString(),
					Text = genero.Nombre
				});
			}
			return items;
		}
	}
}

