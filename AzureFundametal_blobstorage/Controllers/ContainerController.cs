using AzureFundametal_blobstorage.Models;
using AzureFundametal_blobstorage.Services;
using Microsoft.AspNetCore.Mvc;

namespace AzureFundametal_blobstorage.Controllers
{
    
    
    public class ContainerController : Controller
    {
        private readonly IContainerService _containerService;
        public ContainerController(IContainerService containerService)
        {
            _containerService = containerService;

        }
        public async Task<IActionResult> Index()
        {
            var containers = await _containerService.GetAllContainers();
            return View(containers);
        }

        public async Task<IActionResult> Delete(String containerName)
        { 
            await _containerService.DeleteContainer(containerName);
           return RedirectToAction(nameof(Index));
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Container container)
        {
            await _containerService.CreateContainer(container.Name);
            return RedirectToAction(nameof(Index));
        }
    }
}
