using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using WebApi.Utilities;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly FileHelper _fileHelper;

        public FileController(FileHelper fileHelper)
        {
            _fileHelper = fileHelper;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string id)
        {
            var fileBytes = await _fileHelper.DownloadFileAsync(id);
            return File(fileBytes, "application/octet-stream", id);
        }

        [HttpPost]
        [RequestSizeLimit(2 * 1024 * 1024)]
        public async Task<IActionResult> Post(IFormFile file)
        {
            var fileName = await _fileHelper.UploadFileAsync(file);
            return Ok(fileName);
        }

    }
}
