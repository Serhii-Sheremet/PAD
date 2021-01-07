
namespace PAD
{
    public class ShunyaTithiCalendar : MasaCalendar
    {
        public int TithiId { get; set; }
        public EShunya ShunyaCode { get; set; }

        public override EShunya GetShunyaCode()
        {
            return ShunyaCode;
        }
    }
}
