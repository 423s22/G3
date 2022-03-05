using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ETDVAlidator.Models;
using DocumentFormat.OpenXml.Packaging;

namespace ETDVAlidator.Controllers
{
    public class ValidatorController : Controller
    {
        private readonly ILogger<ValidatorController> _logger;

        public ValidatorController(ILogger<ValidatorController> logger)
        {
            _logger = logger;
        }
        
        [HttpGet]
        public IActionResult LoadDocument()
        {
            return View();
        }

        [HttpPost]
        public IActionResult DocumentResults(LoadDocumentViewModel viewModel)
        {
            var uploadedFile = viewModel.Document;
            var inputStream = uploadedFile.OpenReadStream();

            var returnJson = "";
            
            try
            {
                var wordDocument = WordprocessingDocument.Open(inputStream, false);
                var body = wordDocument.MainDocumentPart.Document.Body;

                if (null != body)
                {
                    var filename = uploadedFile.FileName;
                    ValidatorModel vm = new ValidatorModel(wordDocument, filename);
                 
                    returnJson = vm.Validate();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Something went wrong...");
                Console.WriteLine(e.StackTrace);
            }
            
            ViewBag.xmlVal = returnJson;
            return View();
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
