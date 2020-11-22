using System;

namespace PAD
{
    /// <summary>
    /// Class describing Zodiak entity (Overall exists 12 of these entities)
    /// </summary>
    public class Zodiak
    {
        public int Id { get; set; }
        public string Code { get; set; }

        /// <summary>
        /// Returns <see cref = "Zodiak"/> object parsed from one row of a file with zodiak names
        /// </summary>
        /// <param name="s">>Row from a file</param>
        /// <returns>
        /// <see cref = "Zodiak"/> object parsed from one row of a file with zodiak names
        /// </returns>
        public Zodiak ParseFile(string s)
        {
            var row = s.Split(new char[] { '|' });
            return new Zodiak() { Id = Convert.ToInt32(row[0]), Code = row[1] };
        }
    }
}
