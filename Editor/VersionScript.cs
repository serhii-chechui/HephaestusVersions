using System;
using UnityEditor;

namespace HephaestusVersions.Editor
{
    public static class VersionScript
    {
        private enum VersionTypes
        {
            MAJOR = 0,
            MINOR = 1,
            PATCH = 2
        }

        #region Major

        [MenuItem("Tools/Versions/Major/Major – X.0.0.(0)")]
        public static void IncreaseMajor()
        {
            IncreaseVersion((int) VersionTypes.MAJOR);
            IncreaseSubVersion();
        }

        [MenuItem("Tools/Versions/Major/Reset Major")]
        public static void ResetMajor()
        {
            ResetVersion((int) VersionTypes.MAJOR);
        }

        #endregion

        #region Minor

        [MenuItem("Tools/Versions/Minor/Minor – 0.X.0.(0)")]
        public static void IncreaseMinor()
        {
            IncreaseVersion((int) VersionTypes.MINOR);
            IncreaseSubVersion();
        }

        [MenuItem("Tools/Versions/Minor/Reset Minor")]
        public static void ResetMinor()
        {
            ResetVersion((int) VersionTypes.MINOR);
        }

        #endregion

        #region Patch

        [MenuItem("Tools/Versions/Patch/Patch – 0.0.X.(X)")]
        public static void IncreasePatch()
        {
            IncreaseVersion((int) VersionTypes.PATCH);
            IncreaseSubVersion();
        }

        [MenuItem("Tools/Versions/Patch/Reset Patch")]
        public static void ResetPatch()
        {
            ResetVersion((int) VersionTypes.PATCH);
        }

        #endregion

        #region SubVesrion

        [MenuItem("Tools/Versions/Subversion/Subversion – 0.0.0.(x)")]
        public static void IncreaseSubversion()
        {
            IncreaseSubVersion();
        }

        [MenuItem("Tools/Versions/Subversion/Reset Subversion")]
        public static void ResetSubversion() {
            ResetSubversionToOne();
        }

        private static void IncreaseSubVersion()
        {
            var buildNumber = Convert.ToInt32(PlayerSettings.iOS.buildNumber);
            buildNumber++;
            PlayerSettings.iOS.buildNumber = buildNumber.ToString();

            var bundleVersionCode = PlayerSettings.Android.bundleVersionCode;
            bundleVersionCode++;
            PlayerSettings.Android.bundleVersionCode = bundleVersionCode;
        }

        private static void ResetSubversionToOne()
        {
            PlayerSettings.iOS.buildNumber = "1";
            PlayerSettings.Android.bundleVersionCode = 1;
        }

        #endregion

        #region Common

        private static void IncreaseVersion(int versionType)
        {
            var version = PlayerSettings.bundleVersion.Split('.');

            var target = Convert.ToInt32(version[versionType]);
            target++;
            version[versionType] = target.ToString();

            PlayerSettings.bundleVersion = string.Join(".", version);
        }

        private static void ResetVersion(int versionType)
        {
            var version = PlayerSettings.bundleVersion.Split('.');
            version[versionType] = "1";
            PlayerSettings.bundleVersion = string.Join(".", version);
        }

        #endregion
    }
}