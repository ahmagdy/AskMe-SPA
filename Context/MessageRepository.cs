using Aspcorespa.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aspcorespa.Context
{
    public class MessageRepository : IMessageRepository
    {
        private readonly AppDBContext _dbContext;
        

        public MessageRepository(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }


        public bool ChangeMessageVisibility(int id,string username)
        {
            var message =  _dbContext.Messages.FirstOrDefault(m => m.ID == id && m.User.UserName == username);
            if (message == null) return false;
            message.IsVisible = !message.IsVisible;
            _dbContext.Update(message);
            _dbContext.SaveChanges();
            return true;
        }

        public IList<Message> GetAllMessage(string username)
        {
            return _dbContext.Messages.Where(u => u.User.UserName == username)
                                        .Select(m => new Message { ID = m.ID, Content = m.Content, SubmissionDate = m.SubmissionDate, Reply = m.Reply, IsVisible = m.IsVisible })
                                        .ToList();
        }

        public bool AddMessage(MessageRequestModel model)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.UserName == model.Username);
            if (user == null || string.IsNullOrWhiteSpace(model.Message)) return false;
            _dbContext.Messages.Add(new Message { Content = model.Message,UserId = user.Id,IPAddress=model.Ip });
            _dbContext.SaveChanges();
            return true;
        }

        public int MessageCount(string username)
        {
            if (username == null) return 0;
            return _dbContext.Messages.Count(u => u.User.UserName == username);
        }

        public string GetSingleMessageReply(int id)
        {
            var message = _dbContext.Messages.FirstOrDefault(m => m.ID == id);
            if (message == null) return null;
            return message.Reply;
        }

        public bool AddReply(int id,string reply)
        {
            var message = _dbContext.Messages.FirstOrDefault(m => m.ID == id);
            if (message == null || string.IsNullOrWhiteSpace(reply)) return false;
            message.Reply = reply;
            message.IsVisible = true;
            _dbContext.Update(message);
            _dbContext.SaveChanges();
            return true;
        }

        public IList<Message> GetAllVisibleMessages(string username)
        {
            if (username == null || string.IsNullOrWhiteSpace(username)) return null;
            return _dbContext.Messages.Where(m => m.User.UserName == username && m.IsVisible)
                                 .ToList();
        }

        public bool DeleteMessage(int id, string username)
        {
            var message = _dbContext.Messages.FirstOrDefault(m => m.ID == id && m.User.UserName == username);
            if (message == null) return false;
            _dbContext.Remove(message);
            _dbContext.SaveChanges();
            return true;
        }
    }
}
