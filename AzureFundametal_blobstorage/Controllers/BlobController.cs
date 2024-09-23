using AzureFundametal_blobstorage.Models;
using AzureFundametal_blobstorage.Services;
using Microsoft.AspNetCore.Mvc;

namespace AzureFundametal_blobstorage.Controllers
{
    public class BlobController : Controller
    {
        public readonly IBlobService _blobservice;
        public BlobController(IBlobService blobService) {
            _blobservice = blobService;
        }

        [HttpGet]
        public async Task< IActionResult> Manage(string containerName)
        {
            var blobObjects = await _blobservice.GetAllBlobs(containerName);
            return View(blobObjects);
        }
        [HttpGet]
        public IActionResult AddFile(string containerName)
        {           
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddFile(string containerName,IFormFile file, Blob blob)
        {
            var filename = Path.GetFileNameWithoutExtension(file.FileName)+
                "_"+Guid.NewGuid()+Path.GetExtension(file.FileName);
            var result = await _blobservice.UploadBlob(filename, file, containerName, blob);
            if (result)
                return RedirectToAction("Index","Container");
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ViewFile(string name, string containerName)
        {
           return Redirect( await _blobservice.GetBlob(name, containerName));
           
        }

        [HttpGet]
        public async Task<IActionResult> DeleteFile(string name, string containerName)
        {
            var result = await _blobservice.DeleteBlob(name, containerName);
            if (result)
                return RedirectToAction("Index", "Home");
            return View();
        }
    }
}
