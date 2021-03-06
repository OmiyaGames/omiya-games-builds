﻿using UnityEditor;

namespace OmiyaGames.Builds.Editor
{
    ///-----------------------------------------------------------------------
    /// <remarks>
    /// <copyright file="IosBuildSettingEditor.cs" company="Omiya Games">
    /// The MIT License (MIT)
    /// 
    /// Copyright (c) 2014-2020 Omiya Games
    /// 
    /// Permission is hereby granted, free of charge, to any person obtaining a copy
    /// of this software and associated documentation files (the "Software"), to deal
    /// in the Software without restriction, including without limitation the rights
    /// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    /// copies of the Software, and to permit persons to whom the Software is
    /// furnished to do so, subject to the following conditions:
    /// 
    /// The above copyright notice and this permission notice shall be included in
    /// all copies or substantial portions of the Software.
    /// 
    /// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    /// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    /// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    /// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    /// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    /// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
    /// THE SOFTWARE.
    /// </copyright>
    /// <list type="table">
    /// <listheader>
    /// <term>Revision</term>
    /// <description>Description</description>
    /// </listheader>
    /// <item>
    /// <term>
    /// <strong>Date:</strong> 11/21/2015<br/>
    /// <strong>Author:</strong> Taro Omiya
    /// </term>
    /// <description>Initial verison.</description>
    /// </item>
    /// <item>
    /// <term>
    /// <strong>Version:</strong> 0.1.0-preview.1<br/>
    /// <strong>Date:</strong> 5/24/2020<br/>
    /// <strong>Author:</strong> Taro Omiya
    /// </term>
    /// <description>Converting file to a package.</description>
    /// </item>
    /// </list>
    /// </remarks>
    ///-----------------------------------------------------------------------
    /// <summary>
    /// Editor script for <see cref="IosBuildSetting"/>.
    /// </summary>
    [CustomEditor(typeof(IosBuildSetting))]
    public class IosBuildSettingEditor : IPlatformBuildSettingEditor
    {
        private SerializedProperty compression;
        private SerializedProperty symlinkLibraries;
        private SerializedProperty acceptExternalModificationsToPlayer;


        public override string FileExtension
        {
            get
            {
                return "";
            }
        }

        public override void OnEnable()
        {
            base.OnEnable();
            compression = serializedObject.FindProperty("compression");
            symlinkLibraries = serializedObject.FindProperty("symlinkLibraries");
            acceptExternalModificationsToPlayer = serializedObject.FindProperty("acceptExternalModificationsToPlayer");
        }

        protected override void DrawPlatformSpecificSettings()
        {
            EditorGUILayout.PropertyField(compression);
            EditorGUILayout.PropertyField(symlinkLibraries);
            EditorGUILayout.PropertyField(acceptExternalModificationsToPlayer);
        }
    }
}
