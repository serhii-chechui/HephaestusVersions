# Hephaestus Versions

Hephaestus Versions is a part of the **Hephaestus Core** framework. It bumps the project version and the store build
numbers from the Unity editor menu, so a release is never uploaded with a stale version or a build number the store
has already seen.

## Features

- **Semantic version bumps**: Major, Minor and Patch in `PlayerSettings.bundleVersion`, with lower parts reset
  (`1.2.3` → Minor → `1.3.0`).
- **Build numbers in step**: every version bump also increments the iOS, tvOS and macOS build number and the
  Android bundle version code, because stores reject a build number they have already seen.
- **Safe**: values are checked before anything is written. A bad version or build number shows a dialog and leaves
  the settings untouched.
- **Tolerant parsing**: `1.0` is read as `1.0.0`; a `-beta` or `+42` suffix is dropped on bump; iOS build numbers can
  have up to three parts (`1.0.7` → `1.0.8`).

## Requirements

- Unity **2019.2+**

## Installation

Add the WTFGames scoped registry and the package to `Packages/manifest.json`:

```json
{
  "scopedRegistries": [
    {
      "name": "WTFGames",
      "url": "https://upm.wtfgames.com.ua/",
      "scopes": ["com.wtfgames"]
    }
  ],
  "dependencies": {
    "com.wtfgames.hephaestus.versions": "2.0.0"
  }
}
```

## Usage

Everything is in the **Hephaestus → Versions** menu:

| Menu item                   | `bundleVersion` `1.2.3` → | Build numbers |
|-----------------------------|---------------------------|---------------|
| Bump Major (X.0.0)          | `2.0.0`                   | +1            |
| Bump Minor (x.X.0)          | `1.3.0`                   | +1            |
| Bump Patch (x.x.X)          | `1.2.4`                   | +1            |
| Bump Build Number           | unchanged                 | +1            |
| Reset → Major / Minor / Patch | `0.2.3` / `1.0.3` / `1.2.0` | unchanged |
| Reset → Build Number        | unchanged                 | `1`           |

The result is saved to `ProjectSettings/ProjectSettings.asset` and logged to the Console.

The same logic is available from code, e.g. in a CI build script:

```csharp
using UnityEditor;
using WTFGames.Hephaestus.VersionsSystem.Editor;

var next = SemanticVersion.Parse(PlayerSettings.bundleVersion).Bump(VersionPart.Patch);
PlayerSettings.bundleVersion = next.ToString();

if (BuildNumber.TryIncrement(PlayerSettings.iOS.buildNumber, out var buildNumber))
{
    PlayerSettings.iOS.buildNumber = buildNumber;
}
```

## Migrating from 1.x

- The menu moved from **Tools → Versions** to **Hephaestus → Versions**.
- Code that called `HephaestusVersions.Editor.VersionScript` should use
  `WTFGames.Hephaestus.VersionsSystem.Editor.VersionsMenu` (`IncreaseMajor` → `BumpMajor`,
  `IncreaseSubversion` → `BumpBuildNumber`, `ResetSubversion` → `ResetBuildNumber`).
- The assembly is now `com.wtfgames.hephaestus.versions.editor`. asmdefs that reference it by GUID keep working;
  those that reference it by name need the new name.

## Tests

EditMode tests live in `Tests/Editor`. To run them from a project, add the package to `testables` in
`Packages/manifest.json`:

```json
"testables": ["com.wtfgames.hephaestus.versions"]
```

## License

Copyright (C) 2021-2026 Serhii Chechui (WTFGames).

Hephaestus Versions is free software: you can redistribute it and/or modify it under
the terms of the GNU General Public License as published by the Free Software
Foundation, either version 3 of the License, or (at your option) any later
version (`GPL-3.0-or-later`). See [LICENSE.md](LICENSE.md) for the full text.
