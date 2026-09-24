using System;
using System.Globalization;

namespace WTFGames.Hephaestus.VersionsSystem.Editor
{
    /// <summary>
    /// A MAJOR.MINOR.PATCH version, as stored in <c>PlayerSettings.bundleVersion</c>.
    /// </summary>
    public readonly struct SemanticVersion : IEquatable<SemanticVersion>
    {
        private const int PartsCount = 3;

        private static readonly char[] SuffixSeparators = {'-', '+'};

        public int Major { get; }
        public int Minor { get; }
        public int Patch { get; }

        public SemanticVersion(int major, int minor, int patch)
        {
            if (major < 0) throw new ArgumentOutOfRangeException(nameof(major), major, "Must not be negative.");
            if (minor < 0) throw new ArgumentOutOfRangeException(nameof(minor), minor, "Must not be negative.");
            if (patch < 0) throw new ArgumentOutOfRangeException(nameof(patch), patch, "Must not be negative.");

            Major = major;
            Minor = minor;
            Patch = patch;
        }

        /// <summary>
        /// Parses "X", "X.Y" or "X.Y.Z"; missing parts are 0. A pre-release or build suffix
        /// ("1.2.0-beta", "1.2.0+42") is dropped, because a bump always makes a release version.
        /// </summary>
        public static bool TryParse(string value, out SemanticVersion version)
        {
            version = default;

            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            var core = value.Trim();
            var suffixIndex = core.IndexOfAny(SuffixSeparators);
            if (suffixIndex >= 0)
            {
                core = core.Substring(0, suffixIndex);
            }

            var parts = core.Split('.');
            if (parts.Length > PartsCount)
            {
                return false;
            }

            var numbers = new int[PartsCount];
            for (var i = 0; i < parts.Length; i++)
            {
                if (!int.TryParse(parts[i], NumberStyles.None, CultureInfo.InvariantCulture, out numbers[i]))
                {
                    return false;
                }
            }

            version = new SemanticVersion(numbers[0], numbers[1], numbers[2]);
            return true;
        }

        public static SemanticVersion Parse(string value)
        {
            if (TryParse(value, out var version))
            {
                return version;
            }

            throw new FormatException($"'{value}' is not a version in the MAJOR.MINOR.PATCH format.");
        }

        /// <summary>
        /// Increments the part and resets every lower part to 0: 1.2.3 → Minor → 1.3.0.
        /// </summary>
        public SemanticVersion Bump(VersionPart part)
        {
            switch (part)
            {
                case VersionPart.Major:
                    return new SemanticVersion(Major + 1, 0, 0);
                case VersionPart.Minor:
                    return new SemanticVersion(Major, Minor + 1, 0);
                case VersionPart.Patch:
                    return new SemanticVersion(Major, Minor, Patch + 1);
                default:
                    throw new ArgumentOutOfRangeException(nameof(part), part, null);
            }
        }

        /// <summary>
        /// Sets only the given part to 0 and keeps the others: 1.2.3 → Minor → 1.0.3.
        /// </summary>
        public SemanticVersion Reset(VersionPart part)
        {
            switch (part)
            {
                case VersionPart.Major:
                    return new SemanticVersion(0, Minor, Patch);
                case VersionPart.Minor:
                    return new SemanticVersion(Major, 0, Patch);
                case VersionPart.Patch:
                    return new SemanticVersion(Major, Minor, 0);
                default:
                    throw new ArgumentOutOfRangeException(nameof(part), part, null);
            }
        }

        public bool Equals(SemanticVersion other)
        {
            return Major == other.Major && Minor == other.Minor && Patch == other.Patch;
        }

        public override bool Equals(object obj)
        {
            return obj is SemanticVersion other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Major;
                hashCode = (hashCode * 397) ^ Minor;
                hashCode = (hashCode * 397) ^ Patch;
                return hashCode;
            }
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}.{1}.{2}", Major, Minor, Patch);
        }

        public static bool operator ==(SemanticVersion left, SemanticVersion right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(SemanticVersion left, SemanticVersion right)
        {
            return !left.Equals(right);
        }
    }
}
