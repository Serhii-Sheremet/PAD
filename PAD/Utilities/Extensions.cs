using System;
using System.Data.SQLite;
using System.Globalization;
using System.Windows.Forms;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using NodaTime;

namespace PAD
{
    /// <summary>
    /// Class for extensions
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Returns <see cref="bool"/> that shows either a specified date belongs to a period of time between startDate and endDate.
        /// </summary>
        /// <returns>
        /// <see cref="bool"/> that shows either a specified date belongs to a period of time between startDate and endDate.
        /// </returns>
        /// <param name="date">The date to search if it belongs to a specified period.</param>
        /// <param name="startDate">The start date of a period where to search.</param>
        /// <param name="endDate">The end date of a period where to search.</param>
        public static bool Between(this DateTime date, DateTime startDate, DateTime endDate)
        {
            return date >= startDate && date <= endDate;
        }

        public static bool StrictBetween(this DateTime date, DateTime startDate, DateTime endDate)
        {
            return date > startDate && date < endDate;
        }

        /// <summary>
        /// Returns <see cref = "int"/> that represents the int value or 0 if no refference present
        /// </summary>
        /// <param name="reader"><see cref = "SQLiteDataReader"/> object</param>
        /// <param name="index">Index in reader</param>
        /// <returns>
        /// <see cref = "int"/> that represents the int value or 0 if no refference present
        /// </returns>
        public static int IntValue(this SQLiteDataReader reader, int index)
        {
            return reader.IsDBNull(index) ? 0 : reader.GetInt32(index);
        }

        /// <summary>
        /// Returns <see cref = "double"/> that represents the double value or 0 if field is empty
        /// </summary>
        /// <param name="reader"><see cref = "SQLiteDataReader"/> object</param>
        /// <param name="index">Index in reader</param>
        /// <returns>
        /// <see cref = "double"/> that represents the double value or 0 if field is empty
        /// </returns>
        public static double DoubleValue(this SQLiteDataReader reader, int index)
        {
            return reader.IsDBNull(index) ? 0 : Double.Parse(reader.GetString(index), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Returns <see cref = "string"/> that represents the string value or string.Empty if no any value
        /// </summary>
        /// <param name="reader"><see cref = "SQLiteDataReader"/> object</param>
        /// <param name="index">Index in reader</param>
        /// <returns>
        /// <see cref = "string"/> that represents the string value or string.Empty if no any value
        /// </returns>
        public static string StringValue(this SQLiteDataReader reader, int index)
        {
            return reader.IsDBNull(index) ? string.Empty : reader.GetString(index);
        }

        /// <summary>
        /// Returns <see cref = "DateTime?"/> that represents the date value or NULL if field is empty
        /// </summary>
        /// <param name="reader"><see cref = "SQLiteDataReader"/> object</param>
        /// <param name="index">Index in reader</param>
        /// <returns>
        /// <see cref = "DateTime?"/> that represents the date value or NULL if field is empty
        /// </returns>
        public static DateTime? DateValue(this SQLiteDataReader reader, int index)
        {
            return reader.IsDBNull(index) ? (DateTime?)null : DateTime.ParseExact(reader.GetString(index), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Adding double buffering to a control
        /// </summary>
        /// <param name="control">Control</param>
        /// <param name="setting">True/False switcher</param>
        public static void MakeDoubleBuffered(this Control control, bool setting)
        {
            Type controlType = control.GetType();
            PropertyInfo pi = controlType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(control, setting, null);
        }

        public static List<T> Clone<T>(this List<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }

        public static DateTime ShiftByUtcOffset(this DateTime date, TimeSpan baseUtcOffset)
        {
            return date.Add(baseUtcOffset);
        }

        public static DateTime ShiftByNodaTimeOffset(this DateTime date, Offset ntOffset)
        {
            return date.Add(ntOffset.ToTimeSpan());
        }

        public static DateTime ShiftByDaylightDelta(this DateTime date, TimeZoneInfo.AdjustmentRule[] adjustmentRules)
        {
            return Utility.ShiftDateByDaylightDelta(date, adjustmentRules);
        }

        public static LocalDateTime ToLocalDateTime(this DateTime dateTime)
        {
            return new LocalDateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);
        }

        public static int SelectedIndex(this ListView listView)
        {
            if (listView.SelectedIndices.Count > 0)
                return listView.SelectedIndices[0];
            else
                return 0;
        }

    }
}
