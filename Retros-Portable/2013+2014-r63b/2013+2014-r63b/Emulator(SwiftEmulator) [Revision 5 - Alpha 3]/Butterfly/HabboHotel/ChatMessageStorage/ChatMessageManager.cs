namespace Butterfly.HabboHotel.ChatMessageStorage
{
    using Butterfly.Messages;
    using System;
    using System.Collections.Generic;

    public class ChatMessageManager
    {
        private int amountOfMessagesInside;
        private LinkedList<ChatMessage> listOfMessages = new LinkedList<ChatMessage>();

        internal void AddMessage(ChatMessage message)
        {
            this.listOfMessages.AddFirst(message);
            if (this.amountOfMessagesInside == 100)
            {
                this.listOfMessages.RemoveLast();
            }
            else
            {
                this.amountOfMessagesInside++;
            }
        }

        internal Dictionary<int, List<ChatMessage>> GetSortedMessages()
        {
            Dictionary<int, List<ChatMessage>> dictionary = new Dictionary<int, List<ChatMessage>>();
            List<ChatMessage> list = new List<ChatMessage>();
            foreach (ChatMessage message in this.listOfMessages)
            {
                dictionary.Clear();
                list.Add(message);
                dictionary.Add(list.Count, list);
            }
            return dictionary;
        }

        internal void Serialize(ref ServerMessage message)
        {
            foreach (ChatMessage message2 in this.listOfMessages)
            {
                message2.Serialize(ref message);
            }
        }

        internal int messageCount
        {
            get
            {
                return this.amountOfMessagesInside;
            }
        }
    }
}

