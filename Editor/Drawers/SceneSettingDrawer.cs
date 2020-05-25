using UnityEngine;
using UnityEditor;
using OmiyaGames.Common.Editor;

namespace OmiyaGames.Builds.Editor
{
    ///-----------------------------------------------------------------------
    /// <remarkds>
    /// <copyright file="SceneSettingDrawer.cs" company="Omiya Games">
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
    /// <strong>Version:</strong> 0.1.0-preview.1<br/>
    /// <strong>Date:</strong> 5/24/2020<br/>
    /// <strong>Author:</strong> Taro Omiya
    /// </term>
    /// <description>Initial version.</description>
    /// </item>
    /// </list>
    /// </remarks>
    ///-----------------------------------------------------------------------
    /// <summary>
    /// Property drawer for <see cref="SceneSetting"/>.
    /// </summary>
    [CustomPropertyDrawer(typeof(SceneSetting))]
    public class SceneSettingDrawer : CustomSettingDrawer
    {
        private SerializedProperty property = null;
        private UnityEditorInternal.ReorderableList list = null;

        private void CreateList(SerializedProperty property)
        {
            if ((list == null) || (this.property.serializedObject != property.serializedObject))
            {
                this.property = property;
                list = new UnityEditorInternal.ReorderableList(property.serializedObject, property);
                list.headerHeight = EditorHelpers.VerticalMargin;
                list.drawElementCallback += DrawScene;
                list.elementHeight = EditorHelpers.SingleLineHeight(EditorHelpers.VerticalMargin);
            }
        }

        private void DrawScene(Rect rect, int index, bool isActive, bool isFocused)
        {
            if (property != null)
            {
                SerializedProperty element = property.GetArrayElementAtIndex(index);
                rect.y += EditorHelpers.VerticalMargin;
                rect.height = EditorGUIUtility.singleLineHeight;

                // Draw the scene field
                ScenePathDrawer.DrawSceneAssetField(rect, element);
            }
        }

        protected override float CustomValueHeight(SerializedProperty property, GUIContent label)
        {
            CreateList(property);
            return list.GetHeight();
        }

        protected override void DrawCustomValue(ref Rect position, SerializedProperty property, GUIContent label)
        {
            CreateList(property);
            Indent(ref position);
            list.DoList(position);
        }
    }
}
