namespace Butterfly.HabboHotel.Rooms.ThreadPooling
{
    using Butterfly.Core;
    using System;

    internal class TaskManager
    {
        private TaskHandeler[] queues;
        private int taskHandelerSleepTime;

        public TaskManager(int amounOfQueues, int taskHandelerSleepTime, int taskHandelerBusyThreshold)
        {
            Logging.LogMessage(string.Format("Initializing Task manager with {0} queues, sleep time: {1}ms, Busy trehold {2}ms ", amounOfQueues, taskHandelerSleepTime, taskHandelerBusyThreshold));
            this.taskHandelerSleepTime = taskHandelerSleepTime;
            this.queues = new TaskHandeler[amounOfQueues];
            for (int i = 0; i < amounOfQueues; i++)
            {
                this.queues[i] = new TaskHandeler(i, taskHandelerSleepTime, taskHandelerBusyThreshold);
                this.queues[i].startHandeling();
            }
            Logging.LogMessage("Done Initializing Task manager");
        }

        public void destroy()
        {
            for (int i = 0; i < this.queues.Length; i++)
            {
                if (this.queues[i] != null)
                {
                    this.queues[i].stopHandeling();
                }
            }
        }

        public void enqueueMessage(IProcessable message)
        {
            Logging.LogMessage("Enqueing message long running is " + message.isLongRunTask());
            if (message.isLongRunTask())
            {
                this.longRunningProcessing(message);
            }
            else
            {
                TaskHandeler handeler = this.queues[0];
                int procesTime = this.queues[0].procesTime;
                for (int i = 1; i < this.queues.Length; i++)
                {
                    if (this.queues[i].procesTime < procesTime)
                    {
                        handeler = this.queues[i];
                        procesTime = this.queues[i].procesTime;
                    }
                }
                handeler.enqueueMessages(message);
            }
        }

        private void longRunningProcessing(IProcessable message)
        {
        }
    }
}

