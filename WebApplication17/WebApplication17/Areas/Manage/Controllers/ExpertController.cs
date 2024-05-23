using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication17.DAL;
using WebApplication17.Models;

namespace WebApplication17.Areas.Manage.Controllers
{
	[Area("Manage")]
	public class ExpertController : Controller
	{
		AppDbContext _appdbContext;
		IWebHostEnvironment _environment;
        public ExpertController(AppDbContext appDbContext,IWebHostEnvironment webHostEnvironment)
        {
			_appdbContext = appDbContext;
			_environment = webHostEnvironment;
            
        }
        public IActionResult Index()
		{
			return View(_appdbContext.Experts.ToList());
		}
		public IActionResult Create()
		{
			return View();
		}
		[HttpPost]
		public IActionResult Create(Expert expert)
		{
			if (!expert.ImgFile.ContentType.Contains("image/" ))
			{
				ModelState.AddModelError("ImgFile", "yanlis daxil edilib");
				return View();
			}
			string path = _environment.WebRootPath + @"\Upload\manage\";
			string Filename= Guid.NewGuid() + expert.ImgFile.FileName;
			using (FileStream fileStream = new FileStream(path + Filename, FileMode.Create))
			{
				expert.ImgFile.CopyTo(fileStream);

			}
			expert.ImgUrl = Filename;
			_appdbContext.Experts.Add(expert);
			_appdbContext.SaveChanges();
			return RedirectToAction("Index");


		}
		public async Task<IActionResult> Delete(int id)
		{
			var _expertitem= await _appdbContext.Experts.FirstOrDefaultAsync(x => x.Id == id);
			if (_expertitem != null)
			{
				_appdbContext.Remove(_expertitem);
			await	_appdbContext.SaveChangesAsync();

			}
			 return RedirectToAction("Index");
		}
		[HttpGet] 
		public IActionResult Update(int id)
		{
			 var _expertitem= _appdbContext.Experts.FirstOrDefault(_x => _x.Id == id);
			return View(_expertitem);
		}
		[HttpPost]
		public  IActionResult Update (Expert expert)
		{
			if (!ModelState.IsValid)
			{
				return View(expert);
			}
			if (expert.ImgFile!=null) {
                string path = _environment.WebRootPath + @"\Upload\manage\";
                string Filename = Guid.NewGuid() + expert.ImgFile.FileName;
                using (FileStream fileStream = new FileStream(path + Filename, FileMode.Create))
                {
                    expert.ImgFile.CopyTo(fileStream);

                }
                expert.ImgUrl = Filename;
            }
			_appdbContext.Update(expert);
			_appdbContext.SaveChanges();
			return RedirectToAction("Index");

		}

	}
}
