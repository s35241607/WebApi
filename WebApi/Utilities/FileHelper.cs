using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WebApi.Utilities
{
    public class FileHelper
    {
        private readonly string _filePath;

        public FileHelper(IConfiguration config)
        {
            _filePath = config.GetValue<string>("FileServerSettings:Path");
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("No file uploaded.");
            }

            // 確保目標資料夾存在
            Directory.CreateDirectory(_filePath);

            var filePath = Path.Combine(_filePath, file.FileName);

            using (var stream = File.Create(filePath))
            {
                await file.CopyToAsync(stream);
            }

            return filePath;
        }

        public async Task<byte[]> DownloadFileAsync(string fileName)
        {
            var memory = new MemoryStream();

            var filePath = Path.Combine(_filePath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return memory.ToArray();
        }
    }
}
