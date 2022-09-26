using Business.Abstract;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class FileManager : IFileService
    {
        public async Task<string> SaveFileToLocalDirectory(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName);
            var rndFileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine("image", rndFileName);
            var path = $"wwwroot\\{filePath}";

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return filePath;
        }
    }
}
