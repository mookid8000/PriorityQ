using System.Collections.Generic;

namespace Web.Models
{
    public class Question
    {
        public Question(string text)
        {
            Text = text;
            Voters = new List<string>();
        }

        protected Question()
        {
        }

        public virtual string Text { get; private set; }
        public virtual int Votes { get; private set; }
        public virtual List<string> Voters { get; set; }
    }
}