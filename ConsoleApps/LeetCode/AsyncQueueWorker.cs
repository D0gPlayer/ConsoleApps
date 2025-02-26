﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApps.LeetCode
{

	//Usage
	//var queue = new AsyncQueueWorker(3);
	//for (int i = 1; i < 11; i++)
	//{
	//	var taskId = i;
	//	var time = Random.Shared.Next(1000, 4000);
	//	queue.AddTaskInQueue(new Task(async () =>
	//	{
	//		Console.WriteLine($"Start task {taskId}, Time: {time}");
	//		Thread.Sleep(time);
	//		Console.WriteLine($"Finish task {taskId}");
	//	}));
	//}
	//await queue.StartQueue();
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
