using System;

namespace PAD
{
    /// <summary>
    /// Class describing Planet entity (Overall exists 9 of these entities)
    /// </summary>
    public class Planet
    {
        public int Id { get; set; }
        public string Code { get; set; }

        /// <summary>
        /// Returns <see cref = "Planet"/> object parsed from one row of a file with planet names
        /// </summary>
        /// <param name="s">>Row from a file</param>
        /// <returns>
        /// <see cref = "Planet"/> object parsed from one row of a file with planet names
        /// </returns>
        public Planet ParseFile(string s)
        {
            var row = s.Split(new char[] { '|' });
            return new Planet() { Id = Convert.ToInt32(row[0]), Code = row[1] };
        }
    }
}
