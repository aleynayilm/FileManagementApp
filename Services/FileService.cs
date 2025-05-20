using Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class FileService : IFileService
    {
        private readonly IRepository<FileRecord> _fileRepository;
        private readonly IWebHostEnvironment _environment;

        public FileService(IRepository<FileRecord> fileRepository,IWebHostEnvironment environment)
        {
            _fileRepository=fileRepository;
            _environment=environment;
        }

        public async Task<bool> DeleteFileAsync(int fileId)
        {
            var file= await _fileRepository.GetByIdAsync(fileId);
            if(file == null)
                return false;
            var physicalPath=Path.Combine(_environment.WebRootPath, file.FilePath);
            if (File.Exists(physicalPath))
                File.Delete(physicalPath);

            _fileRepository.Delete(file);
            await _fileRepository.SaveAsync();
            return true;
        }

        public async Task<List<FileRecord>> GetAllFilesByUserAsync(int userId)
        {
            var allFiles = await _fileRepository.GetAllAsync();
            return allFiles.Where(f=>f.UserId==userId).ToList();
        }

        public List<string> GetUploadedFiles()
        {
            var uploadPath = Path.Combine(_environment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadPath))
                return new List<string>();

            return Directory.GetFiles(uploadPath)
                .Select(Path.GetFileName)
                .ToList();
        }

        public async Task<FileRecord> UploadFileAsync(IFormFile file, int userId)
        {
            var uploadsFolder= Path.Combine(_environment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var fileRecord = new FileRecord
            {
                FileName = file.FileName,
                FilePath = Path.Combine("uploads", uniqueFileName),
                UploadedAt = DateTime.Now,
                UserId = userId
            };

            await _fileRepository.AddAsync(fileRecord);
            await _fileRepository.SaveAsync();

            return fileRecord;
    }
    }
}
