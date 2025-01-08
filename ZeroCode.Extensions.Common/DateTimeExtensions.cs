using System;

namespace ZeroCode.Extensions
{
    /// <summary>
    ///     Extension methods for <see cref="DateTime" /> and etc.
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        ///     Return new <see cref="DateTime" /> sets time to the end of a current day
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static DateTime EndOfDay(this DateTime @this)
        {
            return new DateTime(
                @this.Year,
                @this.Month,
                @this.Day,
                DateTime.MaxValue.Hour,
                DateTime.MaxValue.Minute,
                DateTime.MaxValue.Second,
                DateTime.MaxValue.Millisecond
            );
        }

        /// <summary>
        ///     Return new <see cref="DateTimeOffset" /> sets time to the end of a current day
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static DateTimeOffset EndOfDay(this DateTimeOffset @this)
        {
            return new DateTimeOffset(
                @this.Year,
                @this.Month,
                @this.Day,
                DateTimeOffset.MaxValue.Hour,
                DateTimeOffset.MaxValue.Minute,
                DateTimeOffset.MaxValue.Second,
                DateTimeOffset.MaxValue.Millisecond,
                @this.Offset
            );
        }

        /// <summary>
        ///     Return new <see cref="DateTime" /> sets time to the start of a current day
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static DateTime StartOfDay(this DateTime @this)
        {
            return new DateTime(
                @this.Year,
                @this.Month,
                @this.Day,
                DateTime.MinValue.Hour,
                DateTime.MinValue.Minute,
                DateTime.MinValue.Second,
                DateTime.MinValue.Millisecond
            );
        }

        /// <summary>
        ///     Return new <see cref="DateTimeOffset" /> sets time to the start of a current day
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static DateTimeOffset StartOfDay(this DateTimeOffset @this)
        {
            return new DateTimeOffset(
                @this.Year,
                @this.Month,
                @this.Day,
                DateTime.MinValue.Hour,
                DateTime.MinValue.Minute,
                DateTime.MinValue.Second,
                DateTime.MinValue.Millisecond,
                @this.Offset
            );
        }

        /// <summary>
        ///     Return new <see cref="DateTime" /> sets to zero a date time part specified by <paramref name="part" />.
        ///     <br />
        ///     <example>
        ///         new DateTime(2024, 12, 20, 13, 56, 20).Truncate(DateTimePart.Hour); // => "2024-12-20T00:00:00"
        ///     </example>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="part">DateTime part that must be truncated</param>
        /// <returns></returns>
        public static DateTime Truncate(this DateTime value, DateTimePart part)
        {
            return new DateTime(
                part >= DateTimePart.Year ? value.Year : DateTime.MinValue.Year,
                part >= DateTimePart.Month ? value.Month : DateTime.MinValue.Month,
                part >= DateTimePart.Day ? value.Day : DateTime.MinValue.Day,
                part >= DateTimePart.Hour ? value.Hour : DateTime.MinValue.Hour,
                part >= DateTimePart.Minute ? value.Minute : DateTime.MinValue.Minute,
                part >= DateTimePart.Second ? value.Second : DateTime.MinValue.Second,
                part >= DateTimePart.Millisecond ? value.Millisecond : DateTime.MinValue.Millisecond
            );
        }

        /// <summary>
        ///     Return new <see cref="DateTimeOffset" /> sets to zero a date time part specified by <paramref name="part" />
        ///     (offset not changing).
        ///     <br />
        ///     <example>
        ///         new DateTimeOffset(2024, 12, 20, 13, 56, 20, TimeSpan.Zero).Truncate(DateTimePart.Hour); // =>
        ///         "2024-12-20T00:00:00"
        ///     </example>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="part">DateTime part that must be truncated</param>
        /// <returns></returns>
        public static DateTimeOffset Truncate(this DateTimeOffset value, DateTimePart part)
        {
            return new DateTimeOffset(
                part >= DateTimePart.Year ? value.Year : DateTime.MinValue.Year,
                part >= DateTimePart.Month ? value.Month : DateTime.MinValue.Month,
                part >= DateTimePart.Day ? value.Day : DateTime.MinValue.Day,
                part >= DateTimePart.Hour ? value.Hour : DateTime.MinValue.Hour,
                part >= DateTimePart.Minute ? value.Minute : DateTime.MinValue.Minute,
                part >= DateTimePart.Second ? value.Second : DateTime.MinValue.Second,
                part >= DateTimePart.Millisecond ? value.Millisecond : DateTime.MinValue.Millisecond,
                value.Offset
            );
        }

        /// <summary>
        ///     Returns the greatest of two <see cref="DateTime" />
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static DateTime Max(DateTime left, DateTime right)
        {
            return left < right ? right : left;
        }

        /// <summary>
        ///     Return the greatest of two <see cref="DateTimeOffset" />
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static DateTimeOffset Max(DateTimeOffset left, DateTimeOffset right)
        {
            return left < right ? right : left;
        }

        /// <summary>
        ///     Return the least of two <see cref="DateTime" />
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static DateTime Min(DateTime left, DateTime right)
        {
            return left < right ? left : right;
        }

        /// <summary>
        ///     Return the least of two <see cref="DateTimeOffset" />
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static DateTimeOffset Min(DateTimeOffset left, DateTimeOffset right)
        {
            return left < right ? left : right;
        }

        /// <summary>
        ///     Return the clamped <see cref="DateTime" /> in inclusive range of <paramref name="min" /> and
        ///     <paramref name="max" />
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static DateTime Clamp(DateTime value, DateTime min, DateTime max)
        {
            return value < min ? min : value > max ? max : value;
        }

        /// <summary>
        ///     Return the clamped <see cref="DateTimeOffset" /> in inclusive range of <paramref name="min" /> and
        ///     <paramref name="max" />
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static DateTimeOffset Clamp(DateTimeOffset value, DateTimeOffset min, DateTimeOffset max)
        {
            return value < min ? min : value > max ? max : value;
        }
    }
}