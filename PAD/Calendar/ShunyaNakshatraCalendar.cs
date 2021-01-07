
namespace PAD
{
    public class ShunyaNakshatraCalendar : MasaCalendar
    {
        public ENakshatra NakshatraCode { get; set; }
        public EShunya ShunyaCode { get; set; }

        public override EShunya GetShunyaCode()
        {
            return ShunyaCode;
        }
    }
}
