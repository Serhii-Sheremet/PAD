using System;

namespace PAD
{
    /// <summary>
    /// Class describes Joga entity (overall there are 10 entities)
    /// </summary>
    public class Joga
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int ColorId { get; set; }

        /// <summary>
        /// Returns <see cref = "Joga"/> object parsed from one row of a file with Joga
        /// </summary>
        /// <param name="s">>Row from a file</param>
        /// <returns>
        /// <see cref = "Joga"/> object parsed from one row of a file with Joga
        /// </returns>

        public Joga ParseFile(string s)
        {
            var row = s.Split(new char[] { '|' });
            return new Joga() { Id = Convert.ToInt32(row[0]), ColorId = Convert.ToInt32(row[1]), Code = row[2] };
        }
    }
}
