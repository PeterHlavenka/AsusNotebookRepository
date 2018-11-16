using System;

namespace Mediaresearch.Framework.Utilities.Threading.TaskQueue
{
	public class DelegateTask<TParams> : ITask
	{
		private readonly string m_name;
		private readonly Action<TParams> m_action;
		private readonly TParams m_parameters;

		public DelegateTask(string name, Action<TParams> action, TParams parameters)
		{
			m_name = name;
			m_action = action;
			m_parameters = parameters;
		}

		public void Invoke()
		{
			m_action(m_parameters);
		}

		public override string ToString()
		{
			return string.Format("{0}[TASK]", m_name);
		}
	}

	public class DelegateTask : DelegateTask<object>
	{
		public DelegateTask(string name, Action action)
			: base(name, o => action(), null)
		{
		}
	}
}