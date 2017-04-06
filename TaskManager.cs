using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;

namespace Lib
{
    public class TaskManager
    {
        private readonly List<TaskEntry> _list = new List<TaskEntry>();
        private readonly object _locker = new object();

        public void AddTask(Action task)
        {
            lock (_locker)
                _list.Add(new TaskEntry(task));
        }

        public void AddTasks(IEnumerable<Action> tasks)
        {
            var entries = tasks.ToList().Select(task => new TaskEntry(task));
            lock (_locker)
                _list.AddRange(entries);
        }

        public void InvokeTasks()
        {
            for (var i = 0; i < _list.Count; i++)
            {
                lock (_locker)
                {
                    if (_list[i].IsRunning)
                    {
                        continue;
                    }
                    _list[i].IsRunning = true;
                }

                _list[i].Task.Invoke();

                lock (_locker)
                {
                    _list[i].IsRunning = false;
                }
            }
        }

        private class TaskEntry
        {
            public Action Task { get; private set; }

            public bool IsRunning { get; set; }

            public TaskEntry(Action task)
            {
                Task = task;
                IsRunning = false;
            }
        }
    }

}
