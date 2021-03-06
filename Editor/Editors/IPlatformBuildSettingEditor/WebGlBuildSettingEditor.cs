﻿using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;
using OmiyaGames.Common.Editor;

namespace OmiyaGames.Builds.Editor
{
    ///-----------------------------------------------------------------------
    /// <remarks>
    /// <copyright file="WebGlBuildSettingEditor.cs" company="Omiya Games">
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
    /// </item><item>
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
    /// Editor script for <see cref="WebGlBuildSetting"/>
    /// </summary>
    [CustomEditor(typeof(WebGlBuildSetting))]
    public class WebGlBuildSettingEditor : IPlatformBuildSettingEditor
    {
        private SerializedProperty templatePath;
        private SerializedProperty hostSpecificArchiveSettings;

        // FIXME: do more research on the Facebook builds
        //private SerializedProperty forFacebook;

        AnimBool allArchivesAnimation;

        HostArchiveSettingReorderableList allArchiveList;

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
            templatePath = serializedObject.FindProperty("templatePath");
            hostSpecificArchiveSettings = serializedObject.FindProperty("hostSpecificArchiveSettings");
            //forFacebook = serializedObject.FindProperty("forFacebook");

            allArchivesAnimation = new AnimBool(true, Repaint);

            allArchiveList = new HostArchiveSettingReorderableList(target, hostSpecificArchiveSettings, new GUIContent("Web Hosts"));
        }

        protected override void DrawPlatformSpecificSettings()
        {
            EditorGUILayout.HelpBox("Current path -- " + PlayerSettings.WebGL.template, MessageType.None);
            EditorGUILayout.PropertyField(templatePath);
            // FIXME: to draw
            //EditorGUILayout.PropertyField(forFacebook);
        }

        protected override void DrawExtraSettings()
        {
            EditorGUILayout.Space();

            // Draw foldout
            EditorHelpers.DrawBoldFoldout(allArchivesAnimation, "Host Specific Archive Settings");

            // Draw the list
            using (EditorGUILayout.FadeGroupScope scope = new EditorGUILayout.FadeGroupScope(allArchivesAnimation.faded))
            {
                if (scope.visible == true)
                {
                    allArchiveList.List.DoLayoutList();
                }
            }
        }
    }
}
