using System;

namespace ZeroCode
{
    /// <summary>
    ///     Shortened version of <see cref="Guid" />. Useful for create generic file names.
    /// </summary>
    public readonly struct ShortGuid : IEquatable<ShortGuid>, IEquatable<Guid>
    {
        /// <summary>
        ///     Actual value of short GUID
        /// </summary>
        private readonly string? _value;

        /// <summary>
        ///     Source GUID of this <see cref="ShortGuid" />
        /// </summary>
        private readonly Guid _source;

        /// <summary>
        ///     Actual value of short guid (including Guid.Empty value)
        /// </summary>
        private string ShortGuidValue => _value ?? EmptyShortGuidValue;

        /// <summary>
        ///     Default short guid value
        /// </summary>
        internal const string EmptyShortGuidValue = "AAAAAAAAAAAAAAAAAAAAAA";

        /// <summary>
        ///     Instance of default <see cref="ShortGuidValue" />
        /// </summary>
        public static ShortGuid Empty = new ShortGuid();

        public ShortGuid(Guid guid)
        {
            _value = ToShortGuid(guid);
            _source = guid;
        }

        public ShortGuid(string shortGuid)
        {
            if (!ValidateShortGuid(shortGuid, out var source))
                throw new ArgumentException("Invalid short GUID value", nameof(shortGuid));

            _source = source;
            _value = shortGuid;
        }

        /// <summary>
        ///     Transforming <see cref="Guid" /> value to short GUID
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        private static string ToShortGuid(Guid guid)
        {
            return Convert.ToBase64String(guid.ToByteArray())
                .Replace("+", "-")
                .Replace("/", "_")[..^2];
        }

        /// <summary>
        ///     Returns source <see cref="Guid" /> value of this <see cref="ShortGuid" />
        /// </summary>
        /// <returns></returns>
        public Guid ToGuid()
        {
            return _source;
        }

        /// <summary>
        ///     Validate and return source <see cref="Guid" /> value of string value that can be short GUID
        /// </summary>
        /// <param name="shortGuid"></param>
        /// <param name="guid"></param>
        /// <returns></returns>
        private static bool ValidateShortGuid(string shortGuid, out Guid guid)
        {
            guid = Guid.Empty;
            var base64String = Convert.FromBase64String(shortGuid
                .Replace("-", "+")
                .Replace("_", "/") + "==");

            try
            {
                guid = new Guid(base64String);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return ShortGuidValue;
        }

        public static bool operator ==(ShortGuid left, ShortGuid right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ShortGuid left, ShortGuid right)
        {
            return !left.Equals(right);
        }

        public static bool operator ==(ShortGuid left, Guid right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ShortGuid left, Guid right)
        {
            return !left.Equals(right);
        }

        /// <inheritdoc />
        public bool Equals(ShortGuid other)
        {
            return _source.Equals(other._source);
        }

        /// <inheritdoc />
        public bool Equals(Guid other)
        {
            return _source.Equals(other);
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            return (obj is ShortGuid otherShortGuid && Equals(otherShortGuid))
                   || (obj is Guid otherGuid && Equals(otherGuid));
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return _source.GetHashCode();
        }
    }
}