using System;
using System.IO;
using System.Windows.Forms;

namespace UtilityPDF
{
    internal class DisplayError
    {
        public static void ErrorGeneric(Exception ex)
        {
            // Display the exception message
            MessageBox.Show(SettingsString.GenericMessageError + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void ErrorIO(IOException ex)
        {
            // Display a more specific error message for IO exceptions
            MessageBox.Show(SettingsString.SpecificMessageErrorIO + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
