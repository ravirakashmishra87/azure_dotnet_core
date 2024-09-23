using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using RedisCache.Data;
using RedisCache.Models;
using System.Diagnostics;

namespace RedisCache.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IDistributedCache _distributedCache;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext applicationDbContext, IDistributedCache cache)
        {
            _distributedCache = cache;
            _context = applicationDbContext;
            _logger = logger;
        }

        public IActionResult Index()
        {
            List<Category> Categorylist = new List<Category>();

            var cachedCategory = _distributedCache.GetString("CategoryList");

            if(!string.IsNullOrEmpty(cachedCategory))
            {
                Categorylist = JsonConvert.DeserializeObject<List<Category>>(cachedCategory);   
            }
            else
            {
                Categorylist = _context.Category.ToList();
                _distributedCache.SetString("CategoryList",JsonConvert.SerializeObject(cachedCategory));
            }
            return View(Categorylist);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}