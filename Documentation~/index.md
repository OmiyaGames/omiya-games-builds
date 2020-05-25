# [Omiya Games](https://www.omiyagames.com/) - Builds

[![ko-fi](https://www.ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/I3I51KS8F)

**Multiplatform Build Settings** is a Unity package by [Omiya Games](https://www.omiyagames.com/) that allows one to setup build settings for multiple platforms, then with one-click, prompt Unity to build to all of them.  To quickly set one up, just do the following:

## Quick Setup

### 1) Create Asset

In Unity, select "Assets -> Create -> Omiya Games -> Build Settings" from the menu bar.  Alternatively, "Tools -> Omiya Games -> Create -> Build Settings" works as well.

![Create Menu Bar](https://omiyagames.github.io/omiya-games-builds/resources/create-context-menu.png)

### 2) Name the Asset

Name the asset as you like.

![Rename Asset](https://omiyagames.github.io/omiya-games-builds/resources/change-file-name.png)

### 3) Select a Folder to Build To

In the Project window, click on the asset if it isn't already to change the Inspector.

Under the `Build Settings` group, click the `Browse...` button and select a folder to build to.  Note that this fills in the field with an absolute path to a folder.  Alternatively, the text field can be filled manually: relative paths can be entered this way.  If so, the path will be relative to the root of the project (where the Assets, Packages, and Project Settings folders are).

![Root Build Folder field](https://omiyagames.github.io/omiya-games-builds/resources/root-build-folder.png)

### 4) Add Platforms to Build for

Under the `Platforms` group, click the plus button below the `All Settings` list to add a desired platform.  One may add as many platforms as they like.

![Add Platform](https://omiyagames.github.io/omiya-games-builds/resources/add-platforms.png)

### 5) Edit Each Platform Settings

Each setting listed in the `All Settings` can be edited individually by clicking on the edit button.

![Edit Platform](https://omiyagames.github.io/omiya-games-builds/resources/edit-platform.png)

Note that the list of options will change based on platform.  Options common between each platform includes being able to change the name of the build and folder name, as well as an option to zip each one.

Finally, clicking the breadcrumb at the top of the Inspector will return the insepctor back to the original root setting.

![Breadcrumb](https://omiyagames.github.io/omiya-games-builds/resources/breadcrumb.png)

### 6) Click `Build All`

Once all the settings are setup and ready, just click `Build All` at the bottom of the Inspector!

![Build All](https://omiyagames.github.io/omiya-games-builds/resources/build-all.png)

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

## LICENSE

Overall package is licensed under [MIT](https://github.com/OmiyaGames/omiya-games-builds/blob/master/LICENSE.md), unless otherwise noted in the [3rd party licenses](https://github.com/OmiyaGames/omiya-games-builds/blob/master/THIRD%20PARTY%20NOTICES.md) file and/or source code.
