using System;
using System.Diagnostics;
using DocumentFormat.OpenXml;
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
            // pull file from view model and format into input stream
            var uploadedFile = viewModel.Document;
            var inputStream = uploadedFile.OpenReadStream();

            var returnJson = "";
            
            try
            {
                // pass input stream into new word processing object
                var wordDocument = WordprocessingDocument.Open(inputStream, false);
                var body = wordDocument.MainDocumentPart.Document.Body;

                
                // if it wasn't an empty file, we'll do validation
                if (wordDocument.DocumentType != WordprocessingDocumentType.Document)
                {
                    //Return some sort of error message to screen
                }

                if (body is null)
                {
                    // Return some sort of error?
                }

                if (body is not null)
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
            }
            
            //TODO: json string should be passed as object, rather than just a view bag string
            ViewBag.xmlVal = returnJson;
            
            // return document results view
            return View();
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
