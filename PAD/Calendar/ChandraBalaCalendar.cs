namespace PAD
{
    public class ChandraBalaCalendar: Calendar
    {
        public EZodiak ZodiakCode { get; set; }
        public int DomNumber { get; set; }
        public string Dom { get; set; }

        public override string GetShortName(ELanguage langCode)
        {
            return Dom;
        }

        public override string GetNumber(ELanguage langCode)
        {
            string domText = string.Empty;
            if (DomNumber == 6 || DomNumber == 8 || DomNumber == 12)
            {
                domText = DomNumber.ToString();
            }
            if (ZodiakCode == EZodiak.SCO && !domText.Equals(string.Empty))
            {
                domText = domText + ", Sco";
            }
            else if (ZodiakCode == EZodiak.SCO && domText.Equals(string.Empty))
            {
                domText = "Sco";
            }
            return domText;
        }

    }
}
