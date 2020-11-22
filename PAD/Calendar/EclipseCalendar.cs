
namespace PAD
{
    public class EclipseCalendar: Calendar
    {
        public int EclipseCode { get; set; }

        public override string GetShortName(ELanguage langCode)
        {
            return DateStart.ToString("HH:mm:ss");
        }
    }
}
