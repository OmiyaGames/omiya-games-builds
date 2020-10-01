using System.Text;
using System.IO;
using UnityEngine;
using OmiyaGames.Web.Security;
using OmiyaGames.Cryptography;
using OmiyaGames.Cryptography.Editor;

namespace OmiyaGames.Builds.Editor
{
    ///-----------------------------------------------------------------------
    /// <remarks>
    /// <copyright file="HostArchiveSetting.cs" company="Omiya Games">
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
    /// <description>Using the new <seealso cref="WebDomainVerifier"/>.</description>
    /// </item>
    /// </list>
    /// </remarks>
    ///-----------------------------------------------------------------------
    /// <summary>
    /// Archive settings for WebGL platform.
    /// </summary>
    public class HostArchiveSetting : IChildBuildSetting
    {
        public const string DefaultLocalDomainListPath = "domains";

        [SerializeField]
        IPlatformBuildSetting.ArchiveSettings archiveSettings = new IPlatformBuildSetting.ArchiveSettings(
            IPlatformBuildSetting.ArchiveType.Zip,
            new CustomFileName(true,
                new CustomFileName.Prefill(CustomFileName.PrefillType.AppName),
                new CustomFileName.Prefill(CustomFileName.PrefillType.Literal, "-"),
                new CustomFileName.Prefill(CustomFileName.PrefillType.BuildSettingName),
                new CustomFileName.Prefill(CustomFileName.PrefillType.Literal, "-v"),
                new CustomFileName.Prefill(CustomFileName.PrefillType.BuildSettingNumber),
                new CustomFileName.Prefill(CustomFileName.PrefillType.Literal, IPlatformBuildSetting.ArchiveSettings.GetFileExtension(IPlatformBuildSetting.ArchiveType.Zip))
            )
        );

        [SerializeField]
        bool includeIndexHtml = true;
        [UnityEngine.Serialization.FormerlySerializedAs("webLocationChecker")]
        [SerializeField]
        WebDomainVerifier webDomainVerifier;
        [SerializeField]
        string localDomainListPath = DefaultLocalDomainListPath;
        [SerializeField]
        string[] acceptedDomains;

        #region Properties
        /// <summary>
        /// Indicates whether this setting will be processed for building or not.
        /// </summary>
        public bool IsEnabled
        {
            get => archiveSettings.IsEnabled;
            set => archiveSettings.IsEnabled = value;
        }

        /// <summary>
        /// Indicates whether the HTML file will be included as part of the zipping process.
        /// </summary>
        public bool IncludeIndexHtml
        {
            get => includeIndexHtml;
        }

        /// <summary>
        /// The encrypter of the list of domains. Returns <c>null</c> if domain list is not meant to be encrypted.
        /// </summary>
        public StringCryptographer DomainEncrypter => webDomainVerifier?.DomainDecrypter;

        public bool IsLocalPathInWebDomainVerifierDefined => ((webDomainVerifier != null) && (string.IsNullOrEmpty(webDomainVerifier?.RemoteDomainListUrl) == false) && (webDomainVerifier.RemoteDomainListUrl.Contains("://") == false));

        /// <summary>
        /// The path local to web build where the domain list will be created.
        /// </summary>
        public string LocalDomainListPath
        {
            get
            {
                // Set to default return value
                string returnPath = DefaultLocalDomainListPath;

                // Check if the domain verifier already has a local path
                if (IsLocalPathInWebDomainVerifierDefined == true)
                {
                    returnPath = webDomainVerifier.RemoteDomainListUrl;
                }
                else if (string.IsNullOrEmpty(localDomainListPath) == false)
                {
                    returnPath = localDomainListPath;
                }
                return returnPath;
            }
        }

        public string[] AcceptedDomains
        {
            get => acceptedDomains;
            set => acceptedDomains = value;
        }

        public IPlatformBuildSetting.ArchiveSettings ArchiveSettings
        {
            get => archiveSettings;
        }

        internal override int MaxNumberOfResults => 1;

        internal override int BuildNumber
        {
            get => Parent.BuildNumber;
            set => Parent.BuildNumber = value;
        }
        #endregion

        public override string GetPathPreview(StringBuilder builder, char pathDivider)
        {
            string parentPath = GetParentPath(builder, pathDivider);

            // Grab the archive name
            parentPath = archiveSettings.FileName.ToString(this);
            AppendFilePath(builder, pathDivider, parentPath);
            return builder.ToString();
        }

        public override bool PreBuildCheck(out string message)
        {
            bool returnFlag = true;
            StringBuilder builder = new StringBuilder();
            if (Parent == null)
            {
                builder.AppendLine("Field 'Parent' is not set in " + name);
                returnFlag = false;
            }
            else if ((Parent is WebGlBuildSetting) == false)
            {
                builder.AppendLine("Field 'Parent' does not contain a WebGL build setting in " + name);
                returnFlag = false;
            }
            if (webDomainVerifier == null)
            {
                builder.AppendLine("Field 'Web Location Checker' is not set in " + name);
                returnFlag = false;
            }
            if ((acceptedDomains == null) || (acceptedDomains.Length <= 0))
            {
                builder.AppendLine("Field 'Accepted Domains' contains no domains in " + name);
                returnFlag = false;
            }
            message = builder.ToString();
            return returnFlag;
        }

        /// <summary>
        /// Generated a domain list.  Does not actually handle archiving.
        /// </summary>
        /// <param name="results"></param>
        protected override void Build(BuildPlayersResult results)
        {
            // Append the parent path
            StringBuilder builder = new StringBuilder();
            string folderName = ((WebGlBuildSetting)Parent).GetBuildFolderName(results);
            string pathToBuildTo = LocalDomainListPath;

            // Append the file path (local to parent)
            builder.Clear();
            builder.Append(folderName);
            AppendFilePath(builder, Helpers.PathDivider, pathToBuildTo);

            // Remove the filename from string builder
            string fileName = Path.GetFileName(pathToBuildTo);
            builder.Remove((builder.Length - fileName.Length), fileName.Length);
            folderName = builder.ToString();

            try
            {
                // Save this asset
                DomainListAssetBundleGenerator.GenerateDomainList(folderName, fileName, AcceptedDomains, DomainEncrypter, false, true);

                // Indicate success
                results.AddPostBuildReport(BuildPlayersResult.Status.Success, "Creating domain list for " + name, this);
            }
            catch (System.Exception ex)
            {
                results.AddPostBuildReport(BuildPlayersResult.Status.Error, ex.Message, this);
            }
        }

        private static void AppendFilePath(StringBuilder builder, char pathDivider, string filePath)
        {
            if (string.IsNullOrEmpty(filePath) == false)
            {
                // Check if we need to add a path divider
                if ((builder.Length > 0) && (builder[builder.Length - 1] != pathDivider))
                {
                    builder.Append(pathDivider);
                }

                // Append this folder name
                builder.Append(filePath);
            }
        }

        private string GetParentPath(StringBuilder builder, char pathDivider)
        {
            // Get the parent's path
            string parentPath = null;
            if (Parent != null)
            {
                parentPath = Parent.GetPathPreview(builder, pathDivider);
            }

            // Setup builder with parent path
            builder.Clear();
            if (string.IsNullOrEmpty(parentPath) == false)
            {
                builder.Append(parentPath);
            }

            return parentPath;
        }

        public void Build(WebGlBuildSetting parent, BuildPlayersResult results)
        {
            if (parent != null)
            {
                Build(results);
            }
        }
    }
}
