using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ETDVAlidator.Models
{
    public class LoadDocumentViewModel
    {
        public IFormFile Document { get; set; }
    }
}