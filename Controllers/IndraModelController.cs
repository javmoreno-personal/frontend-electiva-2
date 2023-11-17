using Microsoft.AspNetCore.Mvc;

namespace Indra.Web.Controllers
{
	public class IndraModelController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}
	}
}
