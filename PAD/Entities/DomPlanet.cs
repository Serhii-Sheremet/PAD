namespace PAD
{
    public class DomPlanet
    {
        public EPlanet PlanetCode {get;set;}
        public double Longitude { get; set; }
        public ETransitType TransitType { get; set; }
        public string Retro { get; set; }
        public string Exaltation { get; set; }
        public bool IsActiveAspect { get; set; }
        public EColor ColorCode { get; set; }
    }
}
