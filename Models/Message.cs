using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aspcorespa.Models
{
    public class Message
    {
        public virtual int ID { get; set; }

        public virtual string Content { get; set; }

        public virtual string IPAddress { get; set; }

        public virtual string Reply { get; set; }

        public virtual DateTime SubmissionDate { get; set; }

        public virtual bool IsVisible { get; set; }

        public virtual string UserId { get; set; }

        public virtual UserEntity User { get; set; }

    }
}
