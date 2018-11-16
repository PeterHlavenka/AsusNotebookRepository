using System;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows;

namespace Mediaresearch.Framework.Utilities.VersionControl
{
	public class NavigateCommand : ICommand
	{
		public void Execute(object parameter)
		{
			Process.Start(Path);
			Window window = parameter as Window;
			if (window != null)
				window.Close();
		}

		public string Path { get; set; }

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public event EventHandler CanExecuteChanged;



        /// To avoid compiler warning...
	    protected virtual void OnCanExecuteChanged()
	    {
	        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
	    }
	}
}
