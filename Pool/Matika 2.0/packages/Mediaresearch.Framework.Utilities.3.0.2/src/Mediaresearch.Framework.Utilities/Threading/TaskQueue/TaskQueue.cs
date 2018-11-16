using System;
using log4net;

namespace Mediaresearch.Framework.Utilities.Threading.TaskQueue
{
	public class TaskQueue : ThreadedObjectsObserverBase<ITask>, ITaskQueue
	{
		private readonly string m_name;
		private static readonly ILog m_log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		public TaskQueue()
			:this(string.Format("TaskQueue_{0}", Guid.NewGuid()))
		{
		}

		public TaskQueue(string name)
			: base(true)
		{
			m_name = name;
		}

		public int? MaximalQueueLength { get; set; }

		private volatile bool m_isWorkingOnItem;

		protected override void HandleObject(ITask task)
		{
			Exception exception = null;
			try
			{
				m_isWorkingOnItem = true;
				if (m_log.IsDebugEnabled)
					m_log.Debug(string.Format("Invoking task '{0}'...", task));
				task.Invoke();
				if (m_log.IsDebugEnabled)
					m_log.Debug(string.Format("Task '{0}' finished", task));
			}
			catch(Exception ex)
			{
				if (m_log.IsErrorEnabled)
					m_log.Error(string.Format("Exception occured in task '{0}'!", task), ex);
				exception = ex;
				throw;
			}
			finally
			{
				m_isWorkingOnItem = false;
				OnTaskCompleted(new TaskCompletedEventArgs(task){Exception = exception});
				OnIsBusyChanged();
			}
		}

		protected override string WorkingThreadName
		{
			get { return  m_name; }
		}

		protected override bool RunThreadAsBackground
		{
			get { return true; }
		}

		public void EnqueueTask(ITask task)
		{
			if(!CanEnqueueTask())
				throw new InvalidOperationException("It is impossible to add task to gueue at this moment!");

			if (m_log.IsDebugEnabled)
				m_log.Debug(string.Format("Enqueueing task '{0}'...", task));
			
			if(IsStoped)
				StartWork();

			HandleReceivedObjects(new[] {task});
			OnTaskEnqueued(task);
			OnIsBusyChanged();
		}

		public bool CanEnqueueTask()
		{
			return MaximalQueueLength == null || WaitingItemsCount < MaximalQueueLength.Value;
		}

		public override bool ThrowOnStartAlreadyStarted
		{
			get { return false; }
		}

		public bool ClearQueueOnStopWork { get; set; }

		public override void StopWork()
		{
			lock (m_lockObject)
			{
				base.StopWork();

				if(ClearQueueOnStopWork)
				{
					m_queue.Clear();
					OnQueueCleared();
					OnIsBusyChanged();
				}
			}
		}

		public bool IsBusy
		{
			get { return m_isWorkingOnItem || WaitingItemsCount > 0; }
		}

		public event IsBusyChangedDelegate IsBusyChanged;

		public void OnIsBusyChanged()
		{
			IsBusyChangedDelegate handler = IsBusyChanged;
			if (handler != null) handler(this, IsBusy);
		}

		public event EventHandler<TaskEnqueuedEventArgs> TaskEnqueued;

		public void OnTaskEnqueued(ITask task)
		{
			EventHandler<TaskEnqueuedEventArgs> handler = TaskEnqueued;
			if (handler != null) handler(this, new TaskEnqueuedEventArgs(task));
		}

		public event EventHandler<TaskCompletedEventArgs> TaskCompleted;

		public void OnTaskCompleted(TaskCompletedEventArgs e)
		{
			EventHandler<TaskCompletedEventArgs> handler = TaskCompleted;
			if (handler != null) handler(this, e);
		}

		public event EventHandler QueueCleared;

		public void OnQueueCleared()
		{
			EventHandler handler = QueueCleared;
			if (handler != null) handler(this, new EventArgs());
		}
	}
}