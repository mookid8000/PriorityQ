using System;
using System.ComponentModel.DataAnnotations;

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

        public DateTime LocalTime { get; set; }
        public DateTime ExpirationTime { get; set; }

        public bool HasExpirationTime   
        {
            get { return ExpirationTime > LocalTime; }
        }
    }
}