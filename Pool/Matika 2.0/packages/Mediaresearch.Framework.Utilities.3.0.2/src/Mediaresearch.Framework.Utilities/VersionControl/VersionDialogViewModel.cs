using System.Collections.Generic;
using System.ComponentModel;

namespace Mediaresearch.Framework.Utilities.VersionControl
{
	public class VersionDialogViewModel : INotifyPropertyChanged
	{
		private string m_DialogTitle;
		public string DialogTitle
		{
			get { return m_DialogTitle; }
			set
			{
				m_DialogTitle = value;
				InvokePropertyChanged("DialogTitle");
			}
		}

		private string m_message;
		public string Message
		{
			get { return m_message; }
			set
			{
				m_message = value;
				InvokePropertyChanged("Holders");
			}
		}

		private List<LinkHolder> m_holders;
		public List<LinkHolder> Holders
		{
			get { return m_holders; }
			set
			{
				m_holders = value;
				InvokePropertyChanged("Holders");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public void InvokePropertyChanged(string property)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null) handler(this, new PropertyChangedEventArgs(property));
		}
	}

	public class LinkHolder
	{
		public string Path { get; set; }
		public NavigateCommand Command { get; set; }
	}
}