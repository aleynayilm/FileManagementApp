using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [Authorize]
    public class FileController:Controller
    {
        private readonly IFileService _fileService;
        private readonly AppDbContext _context;

        public FileController(IFileService fileService, AppDbContext context)
        {
            _fileService = fileService;
            _context = context;
        }
        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file) {
            if (file != null && file.Length > 0) {
                // 1. Dosya uzantısı kontrolü
                var allowedExtensions = new[] { ".pdf", ".png", ".jpg", ".jpeg" };
                var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    ViewBag.Message = "Yalnızca PDF, PNG veya JPG dosyaları yüklenebilir.";
                    return View();
                }

                // 2. Dosya boyutu kontrolü (5 MB)
                const long maxFileSize = 5 * 1024 * 1024; // 5 MB
                if (file.Length > maxFileSize)
                {
                    ViewBag.Message = "Dosya boyutu 5 MB'dan büyük olamaz.";
                    return View();
                }
                int userId = 0;

                if (User?.Identity?.IsAuthenticated == true)
                {
                    var claim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
                    if (claim != null && int.TryParse(claim.Value, out var parsedUserId))
                    {
                        userId = parsedUserId;
                    }
                    else
                    {
                        userId = 0;
                    }
                }

                var result = await _fileService.UploadFileAsync(file, userId);
                if (result != null)
                {
                    ViewBag.Message = $"Dosya yüklendi: {result.FileName}";
                }
                else
                {
                    ViewBag.Message = "Dosya yüklenemedi.";
                }
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> List()
        {
            int userId = 0;
            if (User?.Identity?.IsAuthenticated == true)
            {
                var claim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
                if (claim != null && int.TryParse(claim.Value, out var parsedUserId))
                {
                    userId = parsedUserId;
                }
            }
            var files=await _fileService.GetAllFilesByUserAsync(userId);
            return View(files);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var file=_context.Files.FirstOrDefault(f=>f.Id==id);
            if(file==null)
                return NotFound();
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", file.FilePath);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            _context.Files.Remove(file);
            _context.SaveChanges();
            return RedirectToAction("List");
        }
        public IActionResult Download(int id)
        {
            var file = _context.Files.FirstOrDefault(f=>f.Id==id);
            if (file == null || string.IsNullOrEmpty(file.FilePath))
                return NotFound();
            var filePath= Path.Combine(Directory.GetCurrentDirectory(),"wwwroot", file.FilePath);
            var mimeType = "application/octet-stream";
            var fileName = Path.GetFileName(file.FilePath);

            var bytes = System.IO.File.ReadAllBytes(filePath);
            return File(bytes, mimeType, fileName);
        }
    }
}
