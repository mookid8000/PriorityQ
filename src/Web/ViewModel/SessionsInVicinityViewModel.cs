using System.Collections.Generic;
using System.Linq;
using Web.Models;

namespace Web.ViewModel
{
    public class SessionsInVicinityViewModel
    {
        public SessionsInVicinityViewModel(IEnumerable<SessionHeadline> sessionHeadlines)
        {
            SessionHeadlines = sessionHeadlines.Select(s => new SessionHeadlineViewModel(s)).ToList();
        }

        public List<SessionHeadlineViewModel> SessionHeadlines { get; set; }
    }
}