using System;

namespace PAD
{
    /// <summary>
    /// Class describing Karana entity (Overall exists 60 of these entities)
    /// </summary>
    public class Karana
    {
        public int Id { get; set; }
        public int TithiId { get; set; }
        public int Position { get; set; }
        public int ColorId { get; set; }

        /// <summary>
        /// Returns <see cref = "Karana"/> object parsed from one row of a file with Karana
        /// </summary>
        /// <param name="s">Row from a file</param>
        /// <returns>
        /// <see cref = "Karana"/> object parsed from one row of a file with Karana
        /// </returns>
        public Karana ParseFile(string s)
        {
            var row = s.Split(new char[] { '|' });
            return new Karana { Id = Convert.ToInt32(row[0]), TithiId = Convert.ToInt32(row[1]), Position = Convert.ToInt32(row[2]), ColorId = Convert.ToInt32(row[3]) };//, Name = row[4], Upravitel = row[5], GoodFor = row[6], BadFor = row[7] };
        }
    }
}
