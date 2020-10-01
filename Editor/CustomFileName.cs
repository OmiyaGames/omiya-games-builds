using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEditor;
using OmiyaGames.Web;

namespace OmiyaGames.Builds.Editor
{
    ///-----------------------------------------------------------------------
    /// <remarks>
    /// <copyright file="BuildPlayersResult.cs" company="Omiya Games">
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
    /// <strong>Date:</strong> 10/31/2018<br/>
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
    /// Property used to define a flexible file name
    /// </summary>
    [System.Serializable]
    public struct CustomFileName
    {
        public enum PrefillType
        {
            Literal,
            AppName,
            BuildSettingName,
            DateTime,
            Version,
            BuildSettingNumber
        }

        [System.Serializable]
        public struct Prefill
        {
            public static readonly Dictionary<PrefillType, string> DefaultTextMapper = new Dictionary<PrefillType, string>()
            {
                {
                    PrefillType.DateTime,
                    IBuildSetting.DefaultDateTimeText
                }, {
                    PrefillType.Literal,
                    " - "
                }
            };

            [SerializeField]
            PrefillType type;
            [SerializeField]
            string text;

            public Prefill(PrefillType type, string text = null)
            {
                this.type = type;
                this.text = text;
            }

            public PrefillType Type => type;
            public string Text => text;
        }

        public delegate string GetText(string text, IBuildSetting setting);

        /// <summary>
        /// Map from PrefillType to method
        /// </summary>
        public static readonly Dictionary<PrefillType, GetText> TextMapper = new Dictionary<PrefillType, GetText>()
        {
            {
                PrefillType.Literal,
                (string text, IBuildSetting setting) =>
                {
                    return text;
                }
            }, {
                PrefillType.AppName,
                (string text, IBuildSetting setting) =>
                {
                    return PlayerSettings.productName;
                }
            }, {
                PrefillType.BuildSettingName,
                (string text, IBuildSetting setting) =>
                {
                    return setting.name;
                }
            }, {
                PrefillType.DateTime,
                (string text, IBuildSetting setting) =>
                {
                    return System.DateTime.Now.ToString(text);
                }
            }, {
                PrefillType.Version,
                (string text, IBuildSetting setting) =>
                {
                    return Application.version;
                }
            }, {
                PrefillType.BuildSettingNumber,
                (string text, IBuildSetting setting) =>
                {
                    string returnString = null;
                    if(setting.BuildNumber >= 0)
                    {
                        returnString = setting.BuildNumber.ToString();
                    }
                    return returnString;
                }
            }
        };

        [SerializeField]
        private Prefill[] names;
        [SerializeField]
        bool asSlug;

        public static bool CanEditText(PrefillType type)
        {
            switch (type)
            {
                case PrefillType.Literal:
                case PrefillType.DateTime:
                    return true;
                default:
                    return false;
            }
        }

        public static bool CanEditText(int type)
        {
            return CanEditText((PrefillType)type);
        }

        public CustomFileName(bool asSlug = false, params Prefill[] names)
        {
            this.names = names;
            this.asSlug = asSlug;
        }

        public string ToString(IBuildSetting setting)
        {
            // Append all the text into one
            StringBuilder builder = new StringBuilder();
            GetText method;
            foreach (Prefill name in names)
            {
                if (TextMapper.TryGetValue(name.Type, out method) == true)
                {
                    builder.Append(method(name.Text, setting));
                }
            }

            // Remove invalid characters
            string returnString = builder.ToString();
            returnString = Helpers.RemoveDiacritics(returnString, builder);

            // Check if this needs to be a slug
            if (asSlug == true)
            {
                returnString = UrlHelpers.GenerateSlug(returnString);
            }
            return returnString;
        }


    }
}
