using System;

namespace Lib
{
    public class TaskManagerWithInvocationBlock
    {
        private Action _task;
        private readonly object _locker = new object();

        public void AddTask(Action task)
        {
            lock (_locker)
                _task += task;
        }

        public void InvokeActions()
        {
            lock (_locker)
                _task.Invoke();
        }
    }
}