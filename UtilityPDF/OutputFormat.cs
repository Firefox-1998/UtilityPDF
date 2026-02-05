namespace UtilityPDF
{
    /// <summary>
    /// Specifies the output format for PDF conversion
    /// </summary>
    internal enum OutputFormat
    {
        /// <summary>
        /// DOCX format only
        /// </summary>
        Docx = 0,

        /// <summary>
        /// RTF format only (deletes DOCX after conversion)
        /// </summary>
        RtfOnly = 1,

        /// <summary>
        /// Both DOCX and RTF formats
        /// </summary>
        DocxAndRtf = 2
    }
}
