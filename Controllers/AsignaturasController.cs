using Microsoft.AspNetCore.Mvc;
using Entity.Models;
using UniversidadWeb;

namespace Indra.Web.Controllers
{
	public class AsignaturasController : Controller
	{
		private readonly IConfiguration _configuration;
        private readonly string Baseurl;

		public AsignaturasController(IConfiguration configuration)
		{
			_configuration = configuration;
			this.Baseurl = configuration["UrlBase"];
		}

		public async Task<ViewResult> Index()
		{
			Client client = new(Baseurl);
			List<Asignatura> estudiantes = await client.GetList<Asignatura>("api/Asignatura/GetAll");
            IQueryable<Asignatura> query = estudiantes.AsQueryable();
			return View(query.ToList());
		}

		public ActionResult Create()
		{
		    return View();
		}


		[HttpPost]
		public async Task<ActionResult> Create(Asignatura estudiante)
		{
			if (!ModelState.IsValid)
				return ValidationProblem(ModelState);

			var client = new Client(Baseurl);
			var rpta = await client.Post("api/Asignatura", estudiante);
			if (rpta.Codigo == 1)
			{
				return RedirectToAction("Index");
			}
			ModelState.AddModelError(string.Empty, rpta.Mensaje);
			return View(estudiante);
		}

		public async Task<ActionResult> Edit(int id)
		{
            var asignatura = await new Client(Baseurl).GetEntity<Asignatura>($"api/Asignatura/GetById?Id={id}");
            return View(asignatura);
		}

		[HttpPost]
		public async Task<ActionResult> Edit(Asignatura asignatura)
		{
			if (!ModelState.IsValid)
				return View(asignatura);

			var client = new Client(Baseurl);
			var rpta = await client.Put("api/Asignatura", asignatura);
			if (rpta.Codigo == 1)
            {
                return RedirectToAction("Index");
            }
            ModelState.AddModelError(string.Empty, rpta.Mensaje);
            return View(asignatura);
        }
	}
}

