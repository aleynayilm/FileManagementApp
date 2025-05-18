using Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IFileService
    {
        Task<List<FileRecord>> GetAllFilesByUserAsync(int userId);
        Task<FileRecord> UploadFileAsync(IFormFile file, int userId);
        Task<bool> DeleteFileAsync(int fileId);
    }
}
