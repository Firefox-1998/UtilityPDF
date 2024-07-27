using System;
using System.IO;
using System.Windows.Forms;
using UtilityPDF.Properties;

namespace UtilityPDF
{
    internal class DisplayError
    {
        private static readonly string GenericMessageError = Resources.GenericMessageError;
        private static readonly string SpecificMessageErrorIO = Resources.SpecificMessageErrorIO;

        public static void ErrorGeneric(Exception ex)
        {
            // Display the exception message
            MessageBox.Show(GenericMessageError + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void ErrorIO(IOException ex)
        {
            // Display a more specific error message for IO exceptions
            MessageBox.Show(SpecificMessageErrorIO + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
