using Aspcorespa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aspcorespa.Context
{
    public interface IMessageRepository
    {
        IList<Message> GetAllMessage(string username);
        bool AddMessage(MessageRequestModel model);
        int MessageCount(string username);
        bool ChangeMessageVisibility(int id, string username);
        bool DeleteMessage(int id, string username);
        string GetSingleMessageReply(int id);
        bool AddReply(int id, string reply);
        IList<Message> GetAllVisibleMessages(string username);
    }
}
