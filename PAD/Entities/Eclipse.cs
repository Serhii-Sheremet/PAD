using System;

namespace PAD
{
    /// <summary>
    /// Class describing Eclipse entity (Overall exists 2 of these entities)
    /// </summary>
    public class Eclipse
    {
        public int Id { get; set; }
        public string Code { get; set; }

        /// <summary>
        /// Returns <see cref = "Eclipse"/> object parsed from one row of a file with Eclipse
        /// </summary>
        /// <param name="s">Row from a file</param>
        /// <returns>
        /// <see cref = "Eclipse"/> object parsed from one row of a file with Eclipse
        /// </returns>
        public Eclipse ParseFile(string s)
        {
            var row = s.Split(new char[] { '|' });
            return new Eclipse() { Id = Convert.ToInt32(row[0]), Code = row[1] };
        }

    }
}
