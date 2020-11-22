using System;

namespace PAD
{
    /// <summary>
    /// Class describing Nakshatra entity (Overall exists 27 of these entities)
    /// </summary>
    public class Nakshatra
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int ColorId { get; set; }

        /// <summary>
        /// Returns <see cref = "Nakshatra"/> object parsed from one row of a file with Nakshatra
        /// </summary>
        /// <param name="s">Row from a file</param>
        /// <returns>
        /// <see cref = "Nakshatra"/> object parsed from one row of a file with Nakshatra
        /// </returns>
        public Nakshatra ParseFile(string s)
        {
            var row = s.Split(new char[] { '|' });
            return new Nakshatra() { Id = Convert.ToInt32(row[0]), Code = row[1], ColorId = Convert.ToInt32(row[2]) };
        }
    }
}
