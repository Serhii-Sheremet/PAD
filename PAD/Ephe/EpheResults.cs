namespace PAD
{
    public class EpheResults
    {
        public double[] CalcResults { get; set; }
        public int DateInSeconds { get; set; }
        public int Znak { get; set; }
        public bool RetrogradeStatus { get; set; } = false;
        public bool DateNotFound { get; set; } = false;
    }
}
