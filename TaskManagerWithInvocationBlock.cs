using System;

namespace Lib
{
    public class TaskManagerWithInvocationBlock
    {
        private Action _task;
        private readonly object _addLocker = new object();
        private readonly object _invokeLocker = new object();

        public void AddTask(Action task)
        {
            lock (_addLocker)
                _task += task;
        }

        public void InvokeActions()
        {
            lock (_invokeLocker)
                _task.Invoke();
        }
    }
}
