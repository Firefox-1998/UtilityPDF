namespace UtilityPDF.Security
{
    /// <summary>
    /// Represents the result of a binary integrity verification
    /// </summary>
    internal class IntegrityResult
    {
        /// <summary>
        /// Whether the SHA-256 hash matched
        /// </summary>
        public bool HashValid { get; set; }

        /// <summary>
        /// Whether the RSA signature was verified successfully
        /// </summary>
        public bool SignatureValid { get; set; }

        /// <summary>
        /// Error message if verification failed
        /// </summary>
        public string ErrorMessage { get; set; } = string.Empty;

        /// <summary>
        /// Overall verification passed (both hash and signature are valid)
        /// </summary>
        public bool IsValid => HashValid && SignatureValid && string.IsNullOrEmpty(ErrorMessage);
    }
}
