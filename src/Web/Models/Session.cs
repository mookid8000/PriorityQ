using System;
using System.Collections.Generic;
using Norm;

namespace Web.Models
{
    public class Session
    {
        public static Func<ObjectId> NewId = ObjectId.NewObjectId;

        public Session(string headline, string creatorUserId)
        {
            Id = NewId();
            Questions = new List<Question>();
            Headline = headline;
            CreatedBy = creatorUserId;
        }

        protected Session()
        {
        }

        public virtual ObjectId Id { get; private set; }

        public virtual string Headline { get; private set; }

        public virtual List<Question> Questions { get; private set; }

        public virtual int QuestionCount { get; set; }

        public virtual Location Location { get; set; }

        public virtual string CreatedBy { get; set; }

        public virtual DateTime ExpirationTime { get; set; }
    }
}