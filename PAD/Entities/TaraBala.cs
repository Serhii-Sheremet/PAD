using System;

namespace PAD
{
    /// <summary>
    /// Class describing Tara Bala entity (Overall exists 11 of these entities)
    /// </summary>
    public class TaraBala
    {
        public int Id { get; set; }
        public int ColorId { get; set; }

        /// <summary>
        /// Returns <see cref = "TaraBala"/> object parsed from one row of a file with Tara Bala
        /// </summary>
        /// <param name="s">Row from a file</param>
        /// <returns>
        /// <see cref = "TaraBala"/> object parsed from one row of a file with Tara Bala
        /// </returns>
        public TaraBala ParseFile(string s)
        {
            var row = s.Split(new char[] { '|' });
            return new TaraBala() { Id = Convert.ToInt32(row[0]), ColorId = Convert.ToInt32(row[1]) };//, Short = row[2], Name = row[3], SpecialText = row[4], Description = row[5] };
        }
    }
}
