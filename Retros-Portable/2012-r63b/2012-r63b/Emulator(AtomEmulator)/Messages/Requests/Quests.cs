
using System;
namespace Atom.Messages
{
    partial class GameClientMessageHandler
    {
        public void OpenQuests()
        {
            AtomEnvironment.GetGame().GetQuestManager().GetList(Session, Request);
        }

        public void StartQuest()
        {
            AtomEnvironment.GetGame().GetQuestManager().ActivateQuest(Session, Request);
        }

        public void StopQuest()
        {
            AtomEnvironment.GetGame().GetQuestManager().CancelQuest(Session, Request);
        }

        public void GetCurrentQuest()
        {
            AtomEnvironment.GetGame().GetQuestManager().GetCurrentQuest(Session, Request);
        }

        //public void RegisterQuests()
        //{
        //    RequestHandlers.Add(3101, new RequestHandler(OpenQuests));
        //    RequestHandlers.Add(3102, new RequestHandler(StartQuest));
        //    RequestHandlers.Add(3106, new RequestHandler(StopQuest));
        //    RequestHandlers.Add(3107, new RequestHandler(GetCurrentQuest));
        //}
    }
}
