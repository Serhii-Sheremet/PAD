using System;

namespace PAD
{
    /// <summary>
    /// Class describing Colors entity (For now used 3 of these entities)
    /// </summary>
    public class Colors : ICloneable
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int ARGBValue { get; set; }

        public object Clone()
        {
            return new Colors
            {
                Id = this.Id,
                Code = this.Code,
                ARGBValue = this.ARGBValue
            };
        }

        /// <summary>
        /// Returns <see cref = "ATColor"/> object parsed from one row of a file with colors
        /// </summary>
        /// <param name="s">>Row from a file</param>
        /// <returns>
        /// <see cref = "ATColor"/> object parsed from one row of a file with colors
        /// </returns>
        public Colors ParseFile(string s)
        {
            var row = s.Split(new char[] { '|' });
            return new Colors() { Id = Convert.ToInt32(row[0]), Code = row[1], ARGBValue = Convert.ToInt32(row[2]) };
        }
    }
}
