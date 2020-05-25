using UnityEngine;
using UnityEditor;
using OmiyaGames.Common.Editor;

namespace OmiyaGames.Builds.Editor
{
    ///-----------------------------------------------------------------------
    /// <remarkds>
    /// <copyright file="CustomSettingDrawer.cs" company="Omiya Games">
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
    /// <strong>Date:</strong> 11/20/2018<br/>
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
    /// Property drawer for <see cref="CustomSetting{TYPE}"/>.
    /// </summary>
    public abstract class CustomSettingDrawer : PropertyDrawer
    {
        protected static void Indent(ref Rect position)
        {
            position.x += EditorHelpers.IndentSpace;
            position.width -= EditorHelpers.IndentSpace;
        }

        protected abstract float CustomValueHeight(SerializedProperty property, GUIContent label);

        protected abstract void DrawCustomValue(ref Rect position, SerializedProperty property, GUIContent label);

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // Get List
            float returnHeight = base.GetPropertyHeight(property, label);
            //returnHeight -= EditorGUIUtility.singleLineHeight;

            // Check if control is enabled
            SerializedProperty childProperty = property.FindPropertyRelative("enable");
            if (childProperty.boolValue == true)
            {
                returnHeight += CustomValueHeight(property.FindPropertyRelative("customValue"), label);
            }

            // Calculate Height
            return returnHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            EditorGUI.BeginProperty(position, label, property);

            // Setup position
            Rect childPosition = position;
            childPosition.height = EditorGUIUtility.singleLineHeight;

            // Draw enabled
            SerializedProperty childProperty = property.FindPropertyRelative("enable");
            EditorGUI.PropertyField(childPosition, childProperty, label);

            // Check if control is enabled
            if (childProperty.boolValue == true)
            {
                // Setup next control's position
                childProperty = property.FindPropertyRelative("customValue");
                childPosition.y += childPosition.height;
                childPosition.y += EditorHelpers.VerticalMargin;
                childPosition.height = CustomValueHeight(childProperty, label);

                // Draw custom control
                DrawCustomValue(ref childPosition, childProperty, label);
            }

            // End this property
            EditorGUI.EndProperty();
        }
    }
}
