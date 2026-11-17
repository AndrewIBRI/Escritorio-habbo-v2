namespace Butterfly.HabboHotel.Rooms.ThreadPooling
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Threading;

    internal class TaskHandeler
    {
        private Thread handeler;
        private Queue<IProcessable> messageQueue;

        private event noParameterDelegate threadStopped;

        public TaskHandeler(int id, int sleepTime, int chokeTreshHold)
        {
            this.id = id;
            this.sleepTime = sleepTime;
            this.chokeTreshHold = chokeTreshHold;
            this.messageQueue = new Queue<IProcessable>();
        }

        public void enqueueMessages(IProcessable information)
        {
            this.messageQueue.Enqueue(information);
        }

        private void handleMessages()
        {
            while (this.running)
            {
                DateTime now = DateTime.Now;
                if (this.messageQueue.Count != 0)
                {
                    TaskProcessLogic.processMessage(this.messageQueue.Dequeue());
                }
                TimeSpan span = (TimeSpan) (DateTime.Now - now);
                this.procesTime = (int) Math.Round(span.TotalMilliseconds);
                if (this.procesTime < this.sleepTime)
                {
                    Thread.Sleep((int) (this.sleepTime - this.procesTime));
                }
            }
            this.handeler = null;
            this.running = false;
            if (this.threadStopped != null)
            {
                this.threadStopped();
            }
            this.threadStopped = null;
        }

        public void restartHandeler()
        {
            this.threadStopped += new noParameterDelegate(this.startHandeling);
            this.running = false;
        }

        public void startHandeling()
        {
            if (!this.running)
            {
                this.handeler = new Thread(new ThreadStart(this.handleMessages));
                this.running = true;
                this.handeler.Start();
            }
        }

        public void stopHandeling()
        {
            if (this.handeler != null)
            {
                this.running = false;
            }
        }

        private int chokeTreshHold { get; set; }

        public int id { get; private set; }

        public bool isChoked
        {
            get
            {
                return ((this.procesTime != 0) && (this.procesTime < this.chokeTreshHold));
            }
        }

        public bool isSynchronized { get; private set; }

        public int procesTime { get; private set; }

        public bool running { get; private set; }

        private int sleepTime { get; set; }

        private delegate void noParameterDelegate();
    }
}

