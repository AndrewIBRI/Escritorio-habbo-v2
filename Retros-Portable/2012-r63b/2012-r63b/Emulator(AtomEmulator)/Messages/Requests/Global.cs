
using System;
namespace Atom.Messages
{
    partial class GameClientMessageHandler
    {
        internal void Pong()
        {
            //Session.PongOK = true;
            Session.TimePingedReceived = DateTime.Now;
        }

        //internal void RegisterGlobal()
        //{
        //    RequestHandlers.Add(196, new RequestHandler(Pong));
        //}

        //internal void UnregisterGlobals()
        //{
        //    RequestHandlers.Remove(196);
        //}
    }
}
