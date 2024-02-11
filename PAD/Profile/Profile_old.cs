using System;

namespace PAD
{
    public class Profile_old
    {
        public int Id { get; set; }
        public string ProfileName { get; set; }
        public string PersonName { get; set; }
        public string PersonSurname { get; set; }
        public int PlaceOfLivingId { get; set; }
        public int NakshatraMoonId { get; set; }
        public int PadaMoon { get; set; }
        public int NakshatraLagnaId { get; set; }
        public int PadaLagna { get; set; }
        public int NakshatraSunId { get; set; }
        public int PadaSun { get; set; }
        public int NakshatraVenusId { get; set; }
        public int PadaVenus { get; set; }
        public int NakshatraJupiterId { get; set; }
        public int PadaJupiter { get; set; }
        public int NakshatraMercuryId { get; set; }
        public int PadaMercury { get; set; }
        public int NakshatraMarsId { get; set; }
        public int PadaMars { get; set; }
        public int NakshatraSaturnId { get; set; }
        public int PadaSaturn { get; set; }
        public int NakshatraRahuId { get; set; }
        public int PadaRahu { get; set; }
        public int NakshatraKetuId { get; set; }
        public int PadaKetu { get; set; }
        public bool IsChecked { get; set; }
        public string GUID { get; set; }

        public Profile_old() { GUID = Guid.NewGuid().ToString(); }
    }
}
