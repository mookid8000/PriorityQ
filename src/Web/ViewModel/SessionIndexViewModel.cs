using System.Collections.Generic;
using System.Linq;
using Web.Models;

namespace Web.ViewModel
{
    public class SessionIndexViewModel
    {
        public SessionIndexViewModel(IEnumerable<SessionHeadline> sessionHeadlines, long sessionCount, int first, int sessionsPerPage)
        {
            SessionHeadlines = sessionHeadlines
                .Select(s => new SessionHeadlineViewModel(s))
                .ToList();

            SessionCount = sessionCount;
            From = first + 1;
            To = first + SessionHeadlines.Count;

            ShowNext = first + sessionsPerPage < sessionCount;
            ShowPrevious = first > 0;

            Next = first + sessionsPerPage;
            Previous = first - sessionsPerPage;
        }

        public bool ShowPrevious { get; set; }
        public bool ShowNext { get; set; }
        public List<SessionHeadlineViewModel> SessionHeadlines { get; set; }
        public long SessionCount { get; set; }
        public int From { get; set; }
        public int To { get; set; }
        public int Next { get; set; }
        public int Previous { get; set; }
    }

    public class SessionHeadlineViewModel
    {
        public SessionHeadlineViewModel(SessionHeadline sessionHeadline)
        {
            Id = sessionHeadline.Id.ToString();
            Headline = sessionHeadline.Headline;
            QuestionCount = sessionHeadline.QuestionCount;
        }

        public string Id { get; set; }
        public string Headline { get; set; }
        public int QuestionCount { get; set; }
    }
}