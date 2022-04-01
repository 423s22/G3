namespace ETDVAlidator.Models
{
    public class DocumentResultsViewModel
    {
        public string JsonString { get; set; }

        public DocumentResultsViewModel(string jsonString)
        {
            JsonString = jsonString;
        }
    }
}