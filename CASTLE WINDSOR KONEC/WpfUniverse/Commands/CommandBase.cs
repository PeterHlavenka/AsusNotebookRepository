using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfUniverse
{
    public class CommandBase : ICommand
    {
        private Func<bool> m_canExecute;                            //  delegat nema vstup a vraci bool
        private Action m_execute;                                   //  delegat  Zapouzdřuje metodu, která má jeden parametr a nevrací hodnotu.

        public CommandBase(Func<bool> canExecute, Action execute)   //1. konstruktor
        {
            m_canExecute = canExecute;
            m_execute = execute;
        }

        public CommandBase(Action execute)                          //2. pretizeny konstruktor
            :this(() => true, execute)                              //  zavola konstruktor teto tridy, parametr metoda ktera vraci true, neresi prvni metodu ta je true
        {
        }

        public event EventHandler CanExecuteChanged;                //  event

        public bool CanExecute(object parameter)                    // za jakych podminek je mozne vykonat command
        {
            return m_canExecute();
        }

        public void Execute(object parameter)                       // co se ma vykonat
        {
            m_execute();
        }

        public void FireCanExecute()                                 // metoda na vyvolani eventu
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
