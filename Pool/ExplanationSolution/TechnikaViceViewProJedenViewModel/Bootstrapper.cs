using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using Caliburn.Micro;

namespace TechnikaViceViewProJedenViewModel
{
    public class Bootstrapper : BootstrapperBase
    {
        public Bootstrapper()
        {
            Initialize();
        }
        
        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            // tato metoda neni nutna kdyz tak jen pro castle a splashScreen , nesmi byt prazdna jinak se okno nezobrazi 

            var neco = new ProgramEditorViewModel();
            var basta = new WindowManager();
            basta.ShowDialog(neco);
        }
        
        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            return new[] {Assembly.GetAssembly(typeof(ProgramEditorView))};         
        }


    }
}