namespace ETDVAlidator.Models
{
    public class ComponentWarning
    {
        public string WarningName { get; set; }
        public string WarningDescription { get; set; }
        
        public ComponentWarning(string warningName, string warningDescription)
        {
            WarningName = warningName;
            WarningDescription = warningDescription;
        }
    }
}