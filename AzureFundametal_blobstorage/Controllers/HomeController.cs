using AzureFundametal_blobstorage.Models;
using AzureFundametal_blobstorage.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AzureFundametal_blobstorage.Controllers
{
    public class HomeController : Controller
    {
        private readonly IContainerService _containerService;
        private readonly IBlobService _blobService;
        public HomeController(IContainerService containerService, IBlobService lobService)
        {
            _containerService = containerService;
            _blobService = lobService;
        }

        public async Task<IActionResult> Index()
        {
           
            return View(await _containerService.GetAllContainerAndBlobs());
        }

        public async Task<IActionResult>Images()
        {
            return View(await _blobService.GetAllBlobswithUri("ravi-private-container"));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}