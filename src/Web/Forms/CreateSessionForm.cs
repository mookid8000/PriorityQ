using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Web.Forms
{
    public class CreateSessionForm
    {
        public CreateSessionForm()
        {
            Headline = "";
        }

        [Required]
        public string Headline { get; set; }
        
        public string Lat { get; set; }
        
        public string Lng { get; set; }

        public bool HasLocation
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Lat)
                       && !string.IsNullOrWhiteSpace(Lng);
            }
        }

        public string LocalTime { get; set; }

        [Range(1, 48)]
        public int ExpirationHours { get; set; }
    }
}