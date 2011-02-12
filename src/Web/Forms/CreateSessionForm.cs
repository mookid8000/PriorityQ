namespace Web.Forms
{
    public class CreateSessionForm
    {
        public CreateSessionForm()
        {
            Headline = "";
        }

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
    }
}