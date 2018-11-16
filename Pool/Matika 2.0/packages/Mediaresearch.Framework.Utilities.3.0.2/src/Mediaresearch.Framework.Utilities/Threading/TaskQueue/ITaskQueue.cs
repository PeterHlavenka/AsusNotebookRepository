using System;

namespace Mediaresearch.Framework.Utilities.Threading.TaskQueue
{
	public abstract class TaskQueueEventArgsBase : EventArgs
	{
		private readonly ITask m_task;

		protected TaskQueueEventArgsBase(ITask task)
		{
			m_task = task;
		}

		public ITask Task { get { return m_task; } }
	}

	public class TaskEnqueuedEventArgs : TaskQueueEventArgsBase
	{
		public TaskEnqueuedEventArgs(ITask task) : base(task)
		{
		}
	}

	public class TaskCompletedEventArgs : TaskQueueEventArgsBase
	{
		public TaskCompletedEventArgs(ITask task) : base(task)
		{
		}

		public Exception Exception { get; set; }
	}

	public interface ITaskQueue : ICanBeBusyNotifier
	{
		void EnqueueTask(ITask task);
		bool CanEnqueueTask();
		event EventHandler<TaskEnqueuedEventArgs> TaskEnqueued;
		event EventHandler<TaskCompletedEventArgs> TaskCompleted;
		event EventHandler QueueCleared; 
		void StopWork();
		bool ClearQueueOnStopWork { get; set; }
	}
}