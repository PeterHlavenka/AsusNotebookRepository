using System.Linq;

namespace Matika.Gui
{
    public class EnumeratedWordsViewModel
    {
        public EnumeratedWordsViewModel()
        {
            var dc = new EnumeratedWordsDBDataContext();
            var test = dc.Words.Select(d => d).ToList();
        }      
    }
}