using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.IO.Pipelines;
using System.Text.RegularExpressions;
using TOT.ToolDownloadImage.Models;
using TOT.ToolDownloadImage.Services;

namespace TOT.ToolDownloadImage.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IDownloadRepository _downloadRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly Regex regex = new Regex(".jpg|.png|.peg");

        public HomeController(ILogger<HomeController> logger, IDownloadRepository downloadRepository, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _downloadRepository = downloadRepository;
            _webHostEnvironment = webHostEnvironment;
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

        [HttpGet]
        public IActionResult Index()
        {
            
            var model = new ImageViewModel();
            
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ImageViewModel model)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    //var myUniqueFileName = $@"{Guid.NewGuid()}.jpg"; => random filename
                    //split input string list into array
                    string[] arrayUrl = Regex.Split(model.URL.Replace("\r\n", "\r"), @"\r|\n{1,}").ToArray();
                    var host = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
                    var savedDirectory = @"images";

                    List<string> listImage = new List<string>();
                    foreach (var url in arrayUrl)
                    {
                        var extension = Path.GetExtension(url);
                        var fileName = Path.GetFileName(url);
                        if (regex.IsMatch(extension))
                        {
                            var result = _downloadRepository.GetUrlContent(url);

                            string webRootPath = _webHostEnvironment.WebRootPath;
                            string path = Path.Combine(webRootPath, savedDirectory, fileName);
                            if (result.Result is not null)
                            {
                                await result.Result.CopyToAsync(new FileStream(path, FileMode.Create));
                                listImage.Add($@"{host}/{savedDirectory}/{fileName}");
                                var imageItem = new Image
                                {
                                    URL = $@"/{savedDirectory}/{fileName}"
                                };
                                await _downloadRepository.AddAsync(imageItem);
                            }
                            else
                            {
                                listImage.Add($@"{host}/{savedDirectory}/{fileName}/EXE");
                                continue;
                            }
                        }
                        else
                        {
                            listImage.Add($@"{host}/{savedDirectory}/{fileName}/EXE");
                        }
                    }
                    return View("Result", listImage);
                }

                return View();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IActionResult Result(List<string> listImage)
        {
            
            return View(listImage);
        }
    }
}