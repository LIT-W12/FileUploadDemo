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
/*Create an application where users can upload images and share
it with their friends. However, when an image is uploaded,
the user will be prompted to create a "password" which will
protect the image from being seen by anyone that doesn't have 
the password.

Here's the flow of the application: 

On the home page, there should be a textbox and a file upload
input. The user will then put in a "password" into the textbox
and choose an image to upload. When they hit submit, they should
get taken to a page that says:

"Thank you for uploading your image, here's the link to share with your friends:
http://localhost:123/images/view?id=14
Make sure to give them the password of 'foobar'"

When a user tries to visit an images page, they should first be presented with a 
textbox where they need to put the password saved by the image uploader. If they
enter it correctly, the page should refresh (same url) and they should see the image.
Underneath the image, they should also see a little number that displays how many
times this image has already been viewed (just store this number in the database 
and keep updating it every time it's viewed).  If they put the password in incorrectly,
the page should refresh with an error message saying "please try again".

Once they've put in the password, they should never have to put in the password again
for that image.

Good luck*/
