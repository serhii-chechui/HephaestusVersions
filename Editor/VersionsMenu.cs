using UnityEditor;
using UnityEngine;

namespace WTFGames.Hephaestus.VersionsSystem.Editor
{
    /// <summary>
    /// Hephaestus → Versions menu: bumps and resets the project version and the store build numbers in PlayerSettings.
    /// </summary>
    public static class VersionsMenu
    {
        private const string Root = "Hephaestus/Versions/";
        private const string LogPrefix = "[Hephaestus Versions] ";
        private const string DialogTitle = "Hephaestus Versions";

        #region Bump

        [MenuItem(Root + "Bump Major (X.0.0)", priority = 0)]
        public static void BumpMajor()
        {
            BumpVersion(VersionPart.Major);
        }

        [MenuItem(Root + "Bump Minor (x.X.0)", priority = 1)]
        public static void BumpMinor()
        {
            BumpVersion(VersionPart.Minor);
        }

        [MenuItem(Root + "Bump Patch (x.x.X)", priority = 2)]
        public static void BumpPatch()
        {
            BumpVersion(VersionPart.Patch);
        }

        [MenuItem(Root + "Bump Build Number", priority = 20)]
        public static void BumpBuildNumber()
        {
            if (!TryGetNextBuildNumbers(out var ios, out var tvos, out var macos, out var android))
            {
                return;
            }

            SetBuildNumbers(ios, tvos, macos, android);
            Save();
        }

        #endregion

        #region Reset

        [MenuItem(Root + "Reset/Major", priority = 40)]
        public static void ResetMajor()
        {
            ResetVersion(VersionPart.Major);
        }

        [MenuItem(Root + "Reset/Minor", priority = 41)]
        public static void ResetMinor()
        {
            ResetVersion(VersionPart.Minor);
        }

        [MenuItem(Root + "Reset/Patch", priority = 42)]
        public static void ResetPatch()
        {
            ResetVersion(VersionPart.Patch);
        }

        [MenuItem(Root + "Reset/Build Number", priority = 60)]
        public static void ResetBuildNumber()
        {
            SetBuildNumbers(BuildNumber.Initial, BuildNumber.Initial, BuildNumber.Initial, 1);
            Save();
        }

        #endregion

        #region Common

        private static void BumpVersion(VersionPart part)
        {
            // Everything is validated before anything is written, so a bad value never leaves a half-applied bump.
            if (!TryReadVersion(out var version) ||
                !TryGetNextBuildNumbers(out var ios, out var tvos, out var macos, out var android))
            {
                return;
            }

            PlayerSettings.bundleVersion = version.Bump(part).ToString();

            // Stores reject an upload whose build number is not higher than the previous one,
            // so every release bump also bumps the build number.
            SetBuildNumbers(ios, tvos, macos, android);
            Save();
        }

        private static void ResetVersion(VersionPart part)
        {
            if (!TryReadVersion(out var version))
            {
                return;
            }

            PlayerSettings.bundleVersion = version.Reset(part).ToString();
            Save();
        }

        private static bool TryReadVersion(out SemanticVersion version)
        {
            if (SemanticVersion.TryParse(PlayerSettings.bundleVersion, out version))
            {
                return true;
            }

            ShowError($"Version '{PlayerSettings.bundleVersion}' is not in the MAJOR.MINOR.PATCH format. " +
                      "Fix it in Project Settings → Player → Version.");
            return false;
        }

        private static bool TryGetNextBuildNumbers(out string ios, out string tvos, out string macos, out int android)
        {
            tvos = null;
            macos = null;
            android = 0;

            if (!TryIncrement("iOS", PlayerSettings.iOS.buildNumber, out ios) ||
                !TryIncrement("tvOS", PlayerSettings.tvOS.buildNumber, out tvos) ||
                !TryIncrement("macOS", PlayerSettings.macOS.buildNumber, out macos))
            {
                return false;
            }

            if (PlayerSettings.Android.bundleVersionCode == int.MaxValue)
            {
                ShowError("Android bundle version code has reached its maximum value.");
                return false;
            }

            android = PlayerSettings.Android.bundleVersionCode + 1;
            return true;
        }

        private static bool TryIncrement(string platform, string value, out string result)
        {
            if (BuildNumber.TryIncrement(value, out result))
            {
                return true;
            }

            ShowError($"{platform} build number '{value}' must be one to three period-separated integers.");
            return false;
        }

        private static void SetBuildNumbers(string ios, string tvos, string macos, int android)
        {
            PlayerSettings.iOS.buildNumber = ios;
            PlayerSettings.tvOS.buildNumber = tvos;
            PlayerSettings.macOS.buildNumber = macos;
            PlayerSettings.Android.bundleVersionCode = android;
        }

        private static void Save()
        {
            AssetDatabase.SaveAssets();

            Debug.Log(LogPrefix +
                      $"Version {PlayerSettings.bundleVersion}; build number " +
                      $"iOS {PlayerSettings.iOS.buildNumber}, tvOS {PlayerSettings.tvOS.buildNumber}, " +
                      $"macOS {PlayerSettings.macOS.buildNumber}, Android {PlayerSettings.Android.bundleVersionCode}.");
        }

        private static void ShowError(string message)
        {
            Debug.LogError(LogPrefix + message);
            EditorUtility.DisplayDialog(DialogTitle, message, "OK");
        }

        #endregion
    }
}
