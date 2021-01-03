using System;

namespace PAD
{
    /// <summary>
    /// Class describing Pada entity (Overall exists 108 of these entities)
    /// </summary>
    public class Pada
    {
        public int Id { get; set; }
        public int ZodiakId { get; set; }
        public int NakshatraId { get; set; }
        public int PadaNumber { get; set; }
        public string SpecialNavamsha { get; set; }
        public int Navamsha { get; set; }
        public int ColorId { get; set; }

        /// <summary>
        /// Returns <see cref = "Pada"/> object parsed from one row of a file with Pada
        /// </summary>
        /// <param name="s">>Row from a file</param>
        /// <returns>
        /// <see cref = "Pada"/> object parsed from one row of a file with Pada
        /// </returns>
        public Pada ParseFile(string s)
        {
            var row = s.Split(new char[] { '|' });
            return new Pada() { Id = Convert.ToInt32(row[0]), ZodiakId = Convert.ToInt32(row[1]), NakshatraId = Convert.ToInt32(row[2]), PadaNumber = Convert.ToInt32(row[3]), SpecialNavamsha = row[4], Navamsha = Convert.ToInt32(row[5]), ColorId = Convert.ToInt32(row[6]) };
        }
    }
}
