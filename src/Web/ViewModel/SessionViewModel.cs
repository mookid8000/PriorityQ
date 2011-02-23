using System.Collections.Generic;
using System.Linq;
using Web.Models;
using Web.Services;

namespace Web.ViewModel
{
    public class SessionViewModel
    {
        public SessionViewModel(Session session, UserSession currentUserSession)
        {
            SessionId = session.Id.ToString();
            Headline = session.Headline;
            Questions = session.Questions
                .Select((q, index) => new QuestionViewModel(q, index, currentUserSession))
                .OrderByDescending(q => q.Votes)
                .ToList();
        }

        public string SessionId { get; set; }
        public string Headline { get; set; }
        public List<QuestionViewModel> Questions { get; set; }
    }

    public class QuestionViewModel
    {
        public QuestionViewModel(Question question, int index, UserSession currentUserSession)
        {
            Text = question.Text;
            Votes = question.Votes;
            Index = index;
            VoteCastByCurrentUser = question.Voters.Contains(currentUserSession.UserId);
        }

        public string Text { get; set; }
        public int Votes { get; set; }
        public int Index { get; set; }
        public bool VoteCastByCurrentUser { get; set; }
    }
}