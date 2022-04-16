namespace ETDVAlidator.Models
{
    public class ComponentError
    {
        public string ErrorName { get; set; }
        public string ErrorDescription { get; set; }

        public ComponentError(string errorName, string errorDescription)
        {
            ErrorName = errorName;
            ErrorDescription = errorDescription;
        }
    }
}