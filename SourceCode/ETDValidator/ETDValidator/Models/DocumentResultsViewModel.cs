using System.Collections.Generic;
using System.Linq;

namespace ETDVAlidator.Models
{
    public class DocumentResultsViewModel
    {
        public string  ValidatedDocName { get; set; }
        public List<ComponentError> AllErrors;
        public List<ComponentWarning> AllWarnings;

        public DocumentResultsViewModel(string docName)
        {
            ValidatedDocName = docName;
            
            AllErrors = new List<ComponentError>();
            AllWarnings = new List<ComponentWarning>();
        }

        // since we don't want to show errors/warnings with the same message twice, let's remove them
        public void FilterDuplicatesByDescription()
        {
            AllErrors = AllErrors.GroupBy(x => x.ErrorDescription).Select(x => x.First()).ToList();
            AllWarnings = AllWarnings.GroupBy(x => x.WarningDescription).Select(x => x.First()).ToList();
        }
    }
}