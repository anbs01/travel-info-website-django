using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Threading.Tasks;

namespace TravelPortal.Web.Services
{
    public interface IUploadService
    {
        /// <summary>
        /// 上传文件到指定子目录
        /// </summary>
        /// <param name="file">文件对象</param>
        /// <param name="subFolder">子文件夹名称（如 "recommendations", "foods"）</param>
        /// <returns>返回文件的相对路径（如 "/uploads/foods/guid_name.jpg"）</returns>
        Task<string> UploadFileAsync(IFormFile file, string subFolder);
    }

    public class UploadService : IUploadService
    {
        private readonly IWebHostEnvironment _environment;

        public UploadService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> UploadFileAsync(IFormFile file, string subFolder)
        {
            if (file == null || file.Length == 0) return string.Empty;

            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", subFolder);
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return $"/uploads/{subFolder}/{uniqueFileName}";
        }
    }
}
