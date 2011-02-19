using System.Collections.Generic;

namespace Web.Models
{
    public class Question
    {
        public Question(string text)
            : this()
        {
            Text = text;
        }

        protected Question()
        {
            Voters = new List<string>();
        }

        public virtual string Text { get; private set; }
        public virtual int Votes { get; private set; }
        public virtual List<string> Voters { get; set; }
    }
}