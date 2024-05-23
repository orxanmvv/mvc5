using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication17.DAL;

namespace WebApplication17.Controllers
{
	public class HomeController : Controller
	{
		
		AppDbContext AppDbContext { get; set; }
        public HomeController(AppDbContext appDbContext)
        {
			AppDbContext = appDbContext;
            
        }
        public IActionResult Index()
		{
			return View(AppDbContext.Experts.ToList());
		}

	
	}
}