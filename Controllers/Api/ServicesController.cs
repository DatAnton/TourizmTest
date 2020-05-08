using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Microsoft.EntityFrameworkCore;

using TourizmTest.Models;
using TourizmTest.Serializers;

namespace TourizmTest.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class ServicesController : Controller
    {
        private UserManager<User> _userManager;
        private ApplicationContext _db;
        private IWebHostEnvironment _environment;
        public ServicesController(UserManager<User> userManager, 
            ApplicationContext db, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _db = db;
            _environment = env;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm]ServiceModelSerializer model)
        {
            if(ModelState.IsValid)
            {
                User user = await _userManager.FindByEmailAsync(_userManager.GetUserId(HttpContext.User));
                int FileId = UploadFile(model.PhotoFile);
                if(FileId == 0)
                    return BadRequest("Cannot save photo!");
                
                Service service = new Service
                {
                    Header = model.Header,
                    Describing = model.Describing,
                    Location = model.Location,
                    Price = model.Price,
                    UserId = user.Id,
                    PhotoFileId = FileId
                };
                _db.Services.Add(service);
                _db.SaveChanges();
                return Created("", service);
            }
            return BadRequest("Error data!");
        }

        [HttpGet("{id}")]
        public IActionResult Show(int? id)
        {
            if(id == null)
                return BadRequest("Not declared id!");
            Service service = _db.Services.Include(p => p.User)
                .Include(p => p.PhotoFile).FirstOrDefault(p => p.Id == id);
            if(service == null)
                return NotFound("Not found service!");
            var obj = new { Id = service.Id, Location = service.Location, 
                Price = service.Price, Header = service.Header, 
                user = new { userName = service.User.UserName, email = service.User.Email } };
            return Ok(obj);
        }

        [HttpPut("{id}")]
        public IActionResult Edit(int? id, [FromForm]ServiceModelEditSerializer model)
        {
            if(id == null)
                return BadRequest();
            Service serviceDb = _db.Services.Find(id);
            if(serviceDb == null)
                return NotFound();
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            if(model.Describing != null)
                serviceDb.Describing = model.Describing;            
            if(model.Header != null)
                serviceDb.Header = model.Header;
            if(model.Location != null)
                serviceDb.Location = model.Location;
            if(model.Price != 0)
                serviceDb.Price = model.Price;
            if(model.PhotoFile != null)
            {
                //delete old photo
                string path = Path.Combine(_environment.WebRootPath, "images");
                PhotoFile photo = _db.PhotoFiles.Find(serviceDb.PhotoFileId);
                string filePath = Path.Combine(path, photo.Path);
                System.IO.File.Delete(filePath);
                //upload new and update 
                serviceDb.PhotoFileId = UploadFile(model.PhotoFile);
                _db.PhotoFiles.Remove(photo);
            }
            _db.Services.Update(serviceDb);
            _db.SaveChanges();
            return Ok(serviceDb);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int? id)
        {
            if(id == null)
                return BadRequest();
            Service service = _db.Services
                .Include(p => p.PhotoFile).FirstOrDefault(p => p.Id == id);
            if(service == null)
                return NotFound();

            string path = Path.Combine(_environment.WebRootPath, "images");
            string filePath = Path.Combine(path, service.PhotoFile.Path);
            System.IO.File.Delete(filePath);
            _db.Services.Remove(service);
            _db.PhotoFiles.Remove(service.PhotoFile);
            _db.SaveChanges();
            return Ok(service);
        }

        private int UploadFile(IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                string uploadsFolder = Path.Combine(_environment.WebRootPath, "images");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + uploadedFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    uploadedFile.CopyTo(fileStream);
                }
                PhotoFile file = new PhotoFile { Path = filePath };
                _db.PhotoFiles.Add(file);
                _db.SaveChanges();
                return file.Id;
            }
            return 0;
        }

    }
}