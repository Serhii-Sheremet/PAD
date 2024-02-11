using System;

namespace PAD
{
    public class Profile
    {
        public int Id { get; set; }
        public string ProfileName { get; set; }
        public string PersonName { get; set; }
        public string PersonSurname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int PlaceOfBirthId { get; set; }
        public int PlaceOfLivingId { get; set; }
        public double Ascendent { get; set; }
        
        public double SunLongitude { get; set; }
	    public double SunLatitude { get; set; }
        public double SunSpeedInLongitude { get; set; }
	    public double SunSpeedInLatitude { get; set; }

        public double MoonLongitude { get; set; }
        public double MoonLatitude { get; set; }
        public double MoonSpeedInLongitude { get; set; }
        public double MoonSpeedInLatitude { get; set; }

        public double MercuryLongitude { get; set; }
        public double MercuryLatitude { get; set; }
        public double MercurySpeedInLongitude { get; set; }
        public double MercurySpeedInLatitude { get; set; }

        public double VenusLongitude { get; set; }
        public double VenusLatitude { get; set; }
        public double VenusSpeedInLongitude { get; set; }
        public double VenusSpeedInLatitude { get; set; }

        public double MarsLongitude { get; set; }
        public double MarsLatitude { get; set; }
        public double MarsSpeedInLongitude { get; set; }
        public double MarsSpeedInLatitude { get; set; }

        public double JupiterLongitude { get; set; }
        public double JupiterLatitude { get; set; }
        public double JupiterSpeedInLongitude { get; set; }
        public double JupiterSpeedInLatitude { get; set; }

        public double SaturnLongitude { get; set; }
        public double SaturnLatitude { get; set; }
        public double SaturnSpeedInLongitude { get; set; }
        public double SaturnSpeedInLatitude { get; set; }

        public double RahuMeanLongitude { get; set; }
        public double RahuMeanLatitude { get; set; }
        public double RahuMeanSpeedInLongitude { get; set; }
        public double RahuMeanSpeedInLatitude { get; set; }

        public double RahuTrueLongitude { get; set; }
        public double RahuTrueLatitude { get; set; }
        public double RahuTrueSpeedInLongitude { get; set; }
        public double RahuTrueSpeedInLatitude { get; set; }

        public double KetuMeanLongitude { get; set; }
        public double KetuMeanLatitude { get; set; }
        public double KetuMeanSpeedInLongitude { get; set; }
        public double KetuMeanSpeedInLatitude { get; set; }
                      
        public double KetuTrueLongitude { get; set; }
        public double KetuTrueLatitude { get; set; }
        public double KetuTrueSpeedInLongitude { get; set; }
        public double KetuTrueSpeedInLatitude { get; set; }

        public int Checked {  get; set; }


    }
}
