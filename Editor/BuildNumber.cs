using System.Globalization;

namespace WTFGames.Hephaestus.VersionsSystem.Editor
{
    /// <summary>
    /// Apple build numbers (CFBundleVersion): one to three period-separated non-negative integers.
    /// </summary>
    public static class BuildNumber
    {
        /// <summary>
        /// The value a build number starts from after a reset.
        /// </summary>
        public const string Initial = "1";

        private const int MaxPartsCount = 3;

        /// <summary>
        /// Increments the last part: "41" → "42", "1.0.7" → "1.0.8". An empty value becomes <see cref="Initial"/>.
        /// </summary>
        public static bool TryIncrement(string value, out string result)
        {
            result = null;

            if (string.IsNullOrWhiteSpace(value))
            {
                result = Initial;
                return true;
            }

            var parts = value.Trim().Split('.');
            if (parts.Length > MaxPartsCount)
            {
                return false;
            }

            var last = 0;
            for (var i = 0; i < parts.Length; i++)
            {
                if (!int.TryParse(parts[i], NumberStyles.None, CultureInfo.InvariantCulture, out last))
                {
                    return false;
                }
            }

            if (last == int.MaxValue)
            {
                return false;
            }

            parts[parts.Length - 1] = (last + 1).ToString(CultureInfo.InvariantCulture);
            result = string.Join(".", parts);
            return true;
        }
    }
}
