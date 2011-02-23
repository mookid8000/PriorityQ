using System.ComponentModel.DataAnnotations;

namespace Web.Forms
{
    public class AddQuestionForm
    {
        public string SessionId { get; set; }

        [Required]
        public string QuestionText { get; set; }
    }
}