# Changelog

All notable changes to this project will be documented in this file. The format is based on [Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/) and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### BREAKING CHANGES
- The menu moved from **Tools → Versions** to **Hephaestus → Versions**; "Subversion" items are now "Build Number".
- Namespace `HephaestusVersions.Editor` → `WTFGames.Hephaestus.VersionsSystem.Editor`; the class `VersionScript` → `VersionsMenu`.
- Assembly `WTFGames.HephaestusVersions.Editor` → `com.wtfgames.hephaestus.versions.editor` (the asmdef GUID is kept).

### fix
- Bumping a two-part version such as `1.0` no longer throws `IndexOutOfRangeException`; missing parts are read as 0.
- Bumping Major or Minor resets the lower parts (`1.2.3` → Major → `2.0.0`, was `2.2.3`).
- Resetting Major, Minor or Patch sets it to 0 (was 1).
- Non-numeric versions and build numbers show an error dialog instead of throwing `FormatException`;
  nothing is written until every value is valid.
- Pre-release and build suffixes (`1.2.0-beta`) are accepted and dropped on bump.
- iOS build numbers with several parts (`1.0.7` → `1.0.8`) are supported.
- PlayerSettings are saved to disk right after each change.

### feat
- tvOS and macOS build numbers are bumped and reset together with iOS and Android.
- Every change logs the resulting version and build numbers to the Console.

### test
- EditMode tests for `SemanticVersion` and `BuildNumber`.

### docs
- README with setup and usage.

### build
- `package.json` follows the Hephaestus standard: author object, `publishConfig.registry` set to
  `https://upm.wtfgames.com.ua/`, no `license` field conflicting with LICENSE.md. Removed `package-lock.json`.
- `.npmignore` keeps local dev files out of the published package.

### ci
- GitHub Actions workflow that publishes the package to the WTFGames UPM registry when a GitHub release is created.

## [1.0.3] - 2022-12-07

### feat
- Bumping Major, Minor or Patch also bumps the iOS and Android build numbers.

### fix
- Build numbers reset to 1 instead of 0.

## [1.0.2] - 2022-02-07

### build
- Updated the package author and added `publishConfig`.

## [1.0.0] - 2021-10-08

### feat
- Created HephaestusVersions as a Unity package.
