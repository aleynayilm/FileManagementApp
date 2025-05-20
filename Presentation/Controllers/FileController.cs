using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
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

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }
        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file) {
            if (file != null && file.Length > 0) {
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
    }
}
