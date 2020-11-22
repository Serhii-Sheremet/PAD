using System;

namespace PAD
{
    /// <summary>
    /// Class describing Tithi entity (Overall exists 30 of these entities)
    /// </summary>
    public class Tithi
    {
        public int Id { get; set; }
        public int ColorId { get; set; }

        /// <summary>
        /// Returns <see cref = "Tithi"/> object parsed from one row of a file with Tithi
        /// </summary>
        /// <param name="s">>Row from a file</param>
        /// <returns>
        /// <see cref = "Tithi"/> object parsed from one row of a file with Tithi
        /// </returns>
        public Tithi ParseFile(string s)
        {
            var row = s.Split(new char[] { '|' });
            return new Tithi() { Id = Convert.ToInt32(row[0]), ColorId = Convert.ToInt32(row[1]) }; //, Short = row[2], Name = row[3], Upravitel = row[4], Type = row[5], GoodFor = row[6], BadFor = row[7] };
        }
    }
}
