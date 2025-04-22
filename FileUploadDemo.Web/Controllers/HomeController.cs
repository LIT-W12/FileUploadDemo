using FileUploadDemo.Data;
using FileUploadDemo.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;

namespace FileUploadDemo.Web.Controllers
{
    public class HomeController : Controller
    {
        private IWebHostEnvironment _webHostEnvironment;
        private string _connectionString = @"Data Source=.\sqlexpress; Initial Catalog=ImageUploadDemo;Integrated Security=True;Trust Server Certificate=true;";

        public HomeController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var repo = new ImageRepository(_connectionString);

            return View(new IndexViewModel
            {
                Images = repo.GetAll()
            });
        }

        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Submit(IFormFile image, string title)
        {
            //_webHostEnvironment.WebRootPath //this is the full path from the C drive to the wwwroot folder

            var fileName = $"{Guid.NewGuid()}-{image.FileName}";
            var fullImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", fileName);
            
            using FileStream fs = new FileStream(fullImagePath, FileMode.Create);
            image.CopyTo(fs);
            var repo = new ImageRepository(_connectionString);
            repo.Add(new Image
            {
                Title = title,
                ImagePath = fileName
            });
            return RedirectToAction("index");
        }

    }
}
