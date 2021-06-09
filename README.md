# [Omiya Games](https://www.omiyagames.com/) - Multiplatform Build Settings

[![openupm](https://img.shields.io/npm/v/com.omiyagames.builds?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.omiyagames.builds/) [![Builds documentation](https://github.com/OmiyaGames/omiya-games-builds/workflows/Host%20DocFX%20Documentation/badge.svg)](https://omiyagames.github.io/omiya-games-builds/) [![Mirroring](https://github.com/OmiyaGames/omiya-games-builds/workflows/Mirroring/badge.svg)](https://bitbucket.org/OmiyaGames/omiya-games-builds) [![ko-fi](https://www.ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/I3I51KS8F)

**Multiplatform Build Settings** (or Build Settings for short) is a Unity package by [Omiya Games](https://www.omiyagames.com/) that allows one to setup build settings for multiple platforms, then with one-click, prompt Unity to build to all of them.  This package supports building to:

- Windows, both 32- and 64-bit.
- Mac OSX, 64-bit-only.
- Linux, 64-bit-only.
- WebGL.
- Android.
- iOS (XCode project export).

Other features this package supports:

- Grouping:
    - Grouped settings will be built under a nested folder.
    - In addition, one can choose to build to a single group of settings.
    - Groups can be nested as well.
- Dynamic build naming.
- Support building with debugging turned on.
- Option to zip the build after being built.
- For WebGL, supports [WebLocationChecker](https://openupm.com/packages/com.omiyagames.web.security/) integration:
    - WebGL settings can take multiple lists of valid domains; the build setting will then generate a [DomainList](https://openupm.com/packages/com.omiyagames.cryptography/) per list before zipping the build.
- For Android build, if the user forgets to enter a keystore password, the build settings will prompt the user for it before building.

A quick-start guide on how to use Build Settings is available here: [Documentation](https://omiyagames.github.io/omiya-games-builds/)

## Install

Installing via [OpenUPM's command line tool](https://openupm.com/) is *strongly* recommended over Unity's Package Manager because the former supports dependency resolution...and this package has a *lot* of dependencies!  If you haven't already [installed OpenUPM](https://openupm.com/docs/getting-started.html#installing-openupm-cli), you can do so through Node.js's `npm` (obviously have Node.js installed in your system first):
```
npm install -g openupm-cli
```
Then, to install this package, just run the following command at the root of your Unity project:
```
openupm add com.omiyagames.builds
```

## [Known Issues](https://github.com/OmiyaGames/omiya-games-web-security/issues)

As this package is in early stages of development, there are some known issues.

- Dynamic build naming sometimes doesn't update properly.  For a work-around, add, then remove a literal field.
- Build Settings does not wait until a zipping operation is completed or not.  While for most use cases, this is fine since building is such a lengthy process compared to zipping, behavior on what happens when two zipping operations runs at the same time has not been tested.
- Build Settings does *not* check if libraries for building to a specific platform has been installed or not.
- Similarly, Build Settings does *not* check if IL2CPP build libraries are installed or not.
    - Note, Build Settings intentionally does *not* build with IL2CPP if building from the wrong OS (e.g. building a Mac OSX build from Windows) even if the settings is on, because Unity does not support cross-platform building with IL2CPP.  Rather, the issue is that Build Settings simply can't check if IL2CPP build support is installed on Unity at all.
- General workflow improvements needed.
    - Build naming in particular.
 
 If you find a new one, please file them under [Github project's Issues](https://github.com/OmiyaGames/omiya-games-web-security/issues).

## Resources

- [Documentation](https://omiyagames.github.io/omiya-games-builds/)
- [Change Log](/CHANGELOG.md)

## LICENSE

Overall package is licensed under [MIT](/LICENSE.md), unless otherwise noted in the [3rd party licenses](/THIRD%20PARTY%20NOTICES.md) file and/or source code.

Copyright (c) 2019-2021 Omiya Games
