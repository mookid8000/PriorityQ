using System;
using MongoDB.Bson;

namespace Web.Models
{
    public class SessionHeadline
    {
        public virtual ObjectId Id { get; set; }
        public virtual string Headline { get; set; }
        public virtual int QuestionCount { get; set; }
    }
}