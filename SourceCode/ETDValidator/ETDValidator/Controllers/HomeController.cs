using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ETDVAlidator.Models;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace ETDVAlidator.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult LoadDocument()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoadDocument(LoadDocumentViewModel viewModel)
        {
            var uploadedFile = viewModel.Document;
            var inputStream = uploadedFile.OpenReadStream();

            var returnString = "Had trouble parsing " + uploadedFile.FileName;
            
            try
            {
                using (var wordDocument = WordprocessingDocument.Open(inputStream, false))
                {
                    var body = wordDocument.MainDocumentPart.Document.Body;
                    //var innerXml = body.InnerXml; //Get all XML in document body
                     
                    if (null != body)
                    {
                        returnString = "Successfully parsed " + uploadedFile.FileName;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Something went wrong...");
                Console.WriteLine(e);
            }

            ViewBag.xmlVal = returnString;
            return View();
        }
        
        
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
