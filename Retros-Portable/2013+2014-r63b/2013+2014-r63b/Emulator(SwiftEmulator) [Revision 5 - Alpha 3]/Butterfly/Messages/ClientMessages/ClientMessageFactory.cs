namespace Butterfly.Messages.ClientMessages
{
    using Butterfly.Messages;
    using System;
    using System.Collections;

    internal class ClientMessageFactory
    {
        private static Queue freeObjects;

        internal static ClientMessage GetClientMessage(int messageID, byte[] body)
        {
            if (freeObjects.Count > 0)
            {
                ClientMessage message;
                lock (freeObjects.SyncRoot)
                {
                    message = (ClientMessage) freeObjects.Dequeue();
                }
                if (message == null)
                {
                    return new ClientMessage(messageID, body);
                }
                message.Init(messageID, body);
                return message;
            }
            return new ClientMessage(messageID, body);
        }

        internal static void Init()
        {
            freeObjects = new Queue();
        }

        internal static void ObjectCallback(ClientMessage message)
        {
            lock (freeObjects.SyncRoot)
            {
                freeObjects.Enqueue(message);
            }
        }
    }
}

