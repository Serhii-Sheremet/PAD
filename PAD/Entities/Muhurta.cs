using System;

namespace PAD
{
    /// <summary>
    /// Class describes Muhurta entity (overall there are 5 entities)
    /// </summary>
    public class Muhurta
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int ColorId { get; set; }

        /// <summary>
        /// Returns <see cref = "Muhurta"/> object parsed from one row of a file with Muhurta
        /// </summary>
        /// <param name="s">>Row from a file</param>
        /// <returns>
        /// <see cref = "Muhurta"/> object parsed from one row of a file with Muhurta
        /// </returns>
        public Muhurta ParseFile(string s)
        {
            var row = s.Split(new char[] { '|' });
            return new Muhurta() { Id = Convert.ToInt32(row[0]), ColorId = Convert.ToInt32(row[1]), Code = row[2] };
        }
    }
}
