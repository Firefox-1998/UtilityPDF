using System;

namespace UtilityPDF.Configuration
{
    /// <summary>
    /// Represents the result of an update check operation
    /// </summary>
    internal class UpdateCheckResult
    {
        /// <summary>
        /// Gets or sets whether an update is available
        /// </summary>
        public bool IsUpdateAvailable { get; set; }

        /// <summary>
        /// Gets or sets the current local version
        /// </summary>
        public Version LocalVersion { get; set; }

        /// <summary>
        /// Gets or sets the latest remote version from GitHub
        /// </summary>
        public Version RemoteVersion { get; set; }

        /// <summary>
        /// Gets or sets the HTML page URL for the release
        /// </summary>
        public string DownloadUrl { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the direct download URL for the update archive (.7z or .zip)
        /// </summary>
        public string ArchiveDownloadUrl { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the download URL for the SHA-256 hash file
        /// </summary>
        public string HashDownloadUrl { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the download URL for the RSA signature file
        /// </summary>
        public string SignatureDownloadUrl { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the release notes (body) from GitHub
        /// </summary>
        public string ReleaseNotes { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the error message if the check failed
        /// </summary>
        public string ErrorMessage { get; set; } = string.Empty;

        /// <summary>
        /// Gets whether the update check completed successfully
        /// </summary>
        public bool IsSuccess => string.IsNullOrEmpty(ErrorMessage);

        /// <summary>
        /// Gets whether all required download URLs are available for a secure update
        /// </summary>
        public bool HasCompleteDownloadInfo =>
            !string.IsNullOrEmpty(ArchiveDownloadUrl) &&
            !string.IsNullOrEmpty(HashDownloadUrl) &&
            !string.IsNullOrEmpty(SignatureDownloadUrl);
    }
}
