using System.Collections.ObjectModel;
using System.Linq;

namespace TechnikaViceViewProJedenViewModel
{
    public class ProgramEditorViewModel
    {
        public ProgramEditorViewModel()
        {
            ComboItemsSource = new ObservableCollection<string> {"CzProgramEditorView", "BgProgramEditorView"};
            ViewByEnvironment = ComboItemsSource.First();
        }

        public string ViewByEnvironment { get; set; }

        public ObservableCollection<string> ComboItemsSource { get; set; }
    }
}