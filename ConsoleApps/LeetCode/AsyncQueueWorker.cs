using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApps.LeetCode
{
    public class AsyncQueueWorker
	{
		private readonly Queue<Task> _taskQueue;
		private readonly List<Task> _currentTasks;
		private readonly object _lock = new object(); 
		private readonly int _maxConcurrentTasks;

		public AsyncQueueWorker(int maxConcurrentTasks)
		{
		    _maxConcurrentTasks = maxConcurrentTasks;
		    _taskQueue = new Queue<Task>(); 
		    _currentTasks = new List<Task>();
		}

		public async Task StartQueue()
		{
		    while (_currentTasks.Count > 0)
		    {
		        var completedTask = await Task.WhenAny(_currentTasks);
		        _currentTasks.Remove(completedTask); 

				// Process the next task from the queue if there's room for it
				lock (_lock)
				{
					if (_taskQueue.Count > 0)
					{
						var task = _taskQueue.Dequeue(); 
						task.Start();
						_currentTasks.Add(task);
					}
				}
			}
		}

		public void AddTaskInQueue(Task task)
		{
			if (_currentTasks.Count >= _maxConcurrentTasks)
			{
				lock (_lock)
				{
					_taskQueue.Enqueue(task);
				}
				return;
			}
			task.Start();
			_currentTasks.Add(task);
		}
	}
}
