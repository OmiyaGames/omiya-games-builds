﻿using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEditorInternal;
using UnityEngine;
using System.Text;
using OmiyaGames.Cryptography;
using OmiyaGames.Common.Editor;

namespace OmiyaGames.Builds.Editor
{
    ///-----------------------------------------------------------------------
    /// <remarks>
    /// <copyright file="HostArchiveSettingEditor.cs" company="Omiya Games">
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
    /// <strong>Date:</strong> 2/12/2019<br/>
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
    /// </item><item>
    /// <term>
    /// <strong>Version:</strong> 0.1.1-preview.1<br/>
    /// <strong>Date:</strong> 10/1/2020<br/>
    /// <strong>Author:</strong> Taro Omiya
    /// </term>
    /// <description>Updating editor with new fields.</description>
    /// </item>
    /// </list>
    /// </remarks>
    ///-----------------------------------------------------------------------
    /// <summary>
    /// Editor script for <see cref="HostArchiveSetting"/>.
    /// </summary>
    [CustomEditor(typeof(HostArchiveSetting))]
    public class HostArchiveSettingEditor : IChildBuildSettingEditor
    {
        // Archive settings
        private AnimBool archiveAnimation;
        private SerializedProperty archiveEnable;
        private SerializedProperty archiveType;
        private SerializedProperty archiveFileName;

        // Domain list location
        private AnimBool domainListLocationAnimation;
        private SerializedProperty webDomainVerifier;

        private AnimBool domainListPathAnimation;
        private SerializedProperty domainListPath;

        // Archive contents
        private AnimBool contentAnimation;
        private SerializedProperty includeIndexHtml;
        private SerializedProperty acceptedDomains;
        private ReorderableList domainList;

        public override void OnEnable()
        {
            base.OnEnable();

            // archive settings
            SerializedProperty setting = serializedObject.FindProperty("archiveSettings");
            archiveEnable = setting.FindPropertyRelative("enable");
            archiveType = setting.FindPropertyRelative("type");
            archiveFileName = setting.FindPropertyRelative("fileName");
            archiveAnimation = new AnimBool(true, Repaint);

            // Archive contents
            includeIndexHtml = serializedObject.FindProperty("includeIndexHtml");
            acceptedDomains = serializedObject.FindProperty("acceptedDomains");
            contentAnimation = new AnimBool(true, Repaint);

            // Setup accepted domain list
            domainList = new ReorderableList(serializedObject, acceptedDomains, true, true, true, true);
            domainList.drawHeaderCallback = DrawDomainListHeader;
            domainList.drawElementCallback = DrawDomainListElement;

            // Setup domain list location
            webDomainVerifier = serializedObject.FindProperty("webDomainVerifier");
            domainListLocationAnimation = new AnimBool(true, Repaint);

            domainListPathAnimation = new AnimBool(false, Repaint);
            domainListPath = serializedObject.FindProperty("localDomainListPath");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // Draw enable buttons
            EditorGUILayout.Space();
            archiveEnable.boolValue = EditorGUILayout.ToggleLeft("Build with Parent WebGL Setting", archiveEnable.boolValue);

            // Draw archive settings
            EditorGUILayout.Space();
            DrawArchiveSettings();

            // Draw domain list location
            EditorGUILayout.Space();
            DrawDomainListLocation();

            // Draw archive contents
            EditorGUILayout.Space();
            DrawArchiveContents();

            // Apply changes
            serializedObject.ApplyModifiedProperties();

            // Build button
            EditorGUILayout.Space();
            DrawBuildButton("Build Domain List");
        }

        private void DrawDomainListLocation()
        {
            EditorHelpers.DrawBoldFoldout(domainListLocationAnimation, "Local Domain List Path");

            // Draw the rest of the controls
            using (EditorGUILayout.FadeGroupScope fadeScope = new EditorGUILayout.FadeGroupScope(domainListLocationAnimation.faded))
            {
                if (fadeScope.visible == true)
                {
                    // Draw the rest of the controls
                    DrawFileNamePreview(DrawDomainListLocationControls, AppendDomainListLocation);

                    // Check whether to show custom path
                    HostArchiveSetting targetSetting = (HostArchiveSetting)serializedObject.targetObject;
                    domainListPathAnimation.target = (targetSetting.IsLocalPathInWebDomainVerifierDefined == false);

                    // Animate showing this property
                    using (EditorGUILayout.FadeGroupScope innerFadeScope = new EditorGUILayout.FadeGroupScope(domainListPathAnimation.faded))
                    {
                        if (innerFadeScope.visible == true)
                        {
                            // Draw the option to fill your own path.
                            EditorGUILayout.HelpBox("Web Domain Verifier does not define a local path to create a Domain List. Specify one below:", MessageType.Info);
                            EditorGUILayout.PropertyField(domainListPath);
                        }
                    }
                }
            }
        }

        private void DrawArchiveSettings()
        {
            EditorHelpers.DrawBoldFoldout(archiveAnimation, "Archive File Name");

            // Draw the rest of the controls
            using (EditorGUILayout.FadeGroupScope fadeScope = new EditorGUILayout.FadeGroupScope(archiveAnimation.faded))
            {
                if (fadeScope.visible == true)
                {
                    // Draw the rest of the controls
                    DrawFileNamePreview(DrawArchiveControls, AppendArchiveFileName);
                }
            }
        }

        private void DrawArchiveContents()
        {
            EditorHelpers.DrawBoldFoldout(contentAnimation, "Archive Contents");

            // Draw the rest of the controls
            using (EditorGUILayout.FadeGroupScope fadeScope = new EditorGUILayout.FadeGroupScope(contentAnimation.faded))
            {
                if (fadeScope.visible == true)
                {
                    // Draw the rest of the controls
                    EditorGUILayout.PropertyField(includeIndexHtml);
                    domainList.DoLayoutList();

                    // Draw the import stuff
                    EditorGUILayout.Space();
                    UnityEngine.Object testAsset = null;
                    testAsset = EditorGUILayout.ObjectField("Import Domain List", testAsset, typeof(UnityEngine.Object), false);
                    if (testAsset != null)
                    {
                        AssetBundle bundle = null;
                        try
                        {
                            // Load the bundle, and convert it to a domain list
                            bundle = AssetBundle.LoadFromFile(AssetDatabase.GetAssetPath(testAsset));
                            DomainList domainList = DomainList.Get(bundle);

                            // Decrypt the domain list
                            HostArchiveSetting setting = ((HostArchiveSetting)target);
                            setting.AcceptedDomains = DomainList.Decrypt(domainList, setting.DomainEncrypter);
                        }
                        finally
                        {
                            if (bundle != null)
                            {
                                // Clean-up
                                bundle.Unload(true);
                            }
                        }
                    }
                }
            }
        }

        #region Archive Content Helpers
        private void DrawDomainListElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty element = domainList.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += EditorHelpers.VerticalMargin;
            rect.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(rect, element, GUIContent.none);
        }

        private void DrawDomainListHeader(Rect rect)
        {
            EditorGUI.LabelField(rect, "Domains");
        }
        #endregion

        #region Archive Settings Helpers
        private void DrawArchiveControls()
        {
            DrawName();
            EditorGUILayout.PropertyField(archiveType);
            EditorGUILayout.PropertyField(archiveFileName);
        }

        private string AppendArchiveFileName(string originalString, System.Text.StringBuilder builder)
        {
            builder.Clear();
            int endIndex = originalString.LastIndexOf(Helpers.PathDivider);
            builder.Append(originalString.Substring(0, endIndex));
            builder.Append(Helpers.PathDivider);

            CustomFileName name = CustomFileNameDrawer.GetTarget(archiveFileName);
            builder.Append(name.ToString((IBuildSetting)target));
            return builder.ToString();
        }
        #endregion

        #region Domain List Location Helpers
        private string AppendDomainListLocation(string originalString, StringBuilder builder)
        {
            builder.Clear();
            if (parentProperty.objectReferenceValue is WebGlBuildSetting)
            {
                // Get folder name
                int endIndex = originalString.LastIndexOf(Helpers.PathDivider);
                builder.Append(originalString.Substring(0, endIndex));
                builder.Append(Helpers.PathDivider);

                // Get parent name
                WebGlBuildSetting setting = (WebGlBuildSetting)parentProperty.objectReferenceValue;
                builder.Append(setting.FileName.ToString(setting));
                builder.Append(Helpers.PathDivider);

                // Get location from the host itself
                HostArchiveSetting targetSetting = (HostArchiveSetting)serializedObject.targetObject;
                builder.Append(targetSetting.LocalDomainListPath);
            }
            else
            {
                builder.Append("(Field 'Web Domain Verifier' is not set)");
            }
            return builder.ToString();
        }

        private void DrawDomainListLocationControls()
        {
            EditorGUILayout.PropertyField(webDomainVerifier);
        }
        #endregion
    }
}
