using System;

namespace PAD
{
    public static class DateTimeUtils
    {
        /// <summary>
        /// Get the intersection between testInterval and allowedInterval.
        /// </summary>
        public static DateTimeInterval GetIntervalIntersection(DateTimeInterval testInterval, DateTimeInterval allowedInterval)
        {
            #region sanity check

            const string errorMessage = "DateTimeUtils.GetIntervalIntersection - argument can not be null";

            if (testInterval == null)
            {
                throw new ArgumentNullException("testInterval", errorMessage);
            }

            if (allowedInterval == null)
            {
                throw new ArgumentNullException("allowedInterval", errorMessage);
            }

            if (testInterval.From == null || testInterval.To == null || allowedInterval.From == null || allowedInterval.To == null)
            {
                throw new ArgumentException("DateTimeUtils.GetIntervalIntersection - argument can not be null");
            }

            #endregion sanity check

            //Is testInterval totally outside the allowedInterval?
            if (testInterval.From < allowedInterval.From && testInterval.To < allowedInterval.From
                || testInterval.From > allowedInterval.To && testInterval.To > allowedInterval.To)
            {
                return new DateTimeInterval();
            }

            DateTime from = testInterval.From < allowedInterval.From
                ? allowedInterval.From
                : testInterval.From;

            DateTime to = testInterval.To > allowedInterval.To
                ? allowedInterval.To
                : testInterval.To;

            var result = new DateTimeInterval
            {
                From = from,
                To = to
            };

            return result;
        }

        public static int GetDaysInYear(DateTime date)
        {
            if (date.Equals(DateTime.MinValue))
            {
                return -1;
            }

            DateTime thisYear = new DateTime(date.Year, 1, 1);
            DateTime nextYear = new DateTime(date.Year + 1, 1, 1);

            return (nextYear - thisYear).Days;
        }

    }
}
