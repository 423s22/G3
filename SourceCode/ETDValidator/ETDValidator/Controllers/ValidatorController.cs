using System;
using System.Diagnostics;
using System.IO;
using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ETDVAlidator.Models;
using DocumentFormat.OpenXml.Packaging;
using Microsoft.AspNetCore.Http;

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
            if (null != TempData["Error"])
            {
                ViewBag.error = TempData["Error"];
            }
            
            return View();
        }

        [HttpPost]
        public IActionResult DocumentResults(LoadDocumentViewModel viewModel)
        {
            var returnJson = "";
            TempData["Error"] = null;
            
            try
            {
                var uploadedFile = viewModel.Document;
                var inputStream = uploadedFile.OpenReadStream();

                // naive extension validation - any other file validation will be caught by exception
                if (!uploadedFile.FileName.Contains(".docx"))
                {
                    TempData["Error"] = "Please enter a valid .docx file";
                    throw new FileFormatException(ViewBag.error);
                }
                
                // pass input stream into new word processing object
                var wordDocument = WordprocessingDocument.Open(inputStream, false);
                var body = wordDocument.MainDocumentPart.Document.Body;

                // if it wasn't an empty file, we'll do validation
                if (null != body)
                {
                    var filename = uploadedFile.FileName;
                    
                    // create new validation model with passed file
                    ValidatorModel vm = new ValidatorModel(wordDocument, filename);
                 
                    // do the validation and format to json string
                    returnJson = vm.Validate();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Something went wrong...");
                Console.WriteLine(e.StackTrace);

                if (null == TempData["Error"])
                {
                    TempData["Error"] = "Something went wrong validating your file";
                }
            }
            
            // redirect back to upload form if there were issues with the uploaded file
            if (null != TempData["Error"])
            {
                return RedirectToAction("LoadDocument");
            }

            // return document results view
            return View(new DocumentResultsViewModel(returnJson));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
