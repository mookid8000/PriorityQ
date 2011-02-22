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
        
        [DataType(DataType.DateTime)]
        public string ExpirationDate { get; set; }

        [DataType(DataType.Time)]
        public string ExpirationTime { get; set; }

        DateTime LocalTimeParsed
        {
            get { return DateTime.Parse(LocalTime); }
        }

        DateTime ExpirationDateParsed
        {
            get { return DateTime.Parse(ExpirationDate); }
        }

        TimeSpan ExpirationTimeParsed
        {
            get { return ParseTimeSpan(); }
        }

        TimeSpan ParseTimeSpan()
        {
            try
            {
                var time = ExpirationTime;
                var am = time.ToLower().EndsWith("am");
                var pm = time.ToLower().EndsWith("pm");
                if (!(am || pm)) throw new FormatException(string.Format("Time must be either AM or PM!: {0}", time));
                var prefix = time.Substring(0, time.Length - 2);
                var tokens = prefix.Split(':');
                if (tokens.Length != 2) throw new FormatException(string.Format("Time must consist of HH:mm: {0}", time));
                return new TimeSpan(int.Parse(tokens.First()), int.Parse(tokens.Last()), 0);
            }
            catch (Exception e)
            {
                return new TimeSpan();
            }
        }

        public bool HasExpirationTime   
        {
            get { return ExpirationDateParsed + ExpirationTimeParsed > LocalTimeParsed; }
        }

        public TimeSpan TimeToExpire
        {
            get { return ExpirationDateParsed + ExpirationTimeParsed - LocalTimeParsed; }
        }
    }
}