using System;
using System.IO;
using System.Windows.Forms;
using UtilityPDF.Resources;

namespace UtilityPDF
{
    internal class DisplayError
    {
        public static void ErrorGeneric(Exception ex)
        {
            // Display the exception message
            MessageBox.Show(Strings.GenericMessageError + ex.Message, Strings.MsgBoxErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void ErrorIO(IOException ex)
        {
            // Display a more specific error message for IO exceptions
            MessageBox.Show(Strings.SpecificMessageErrorIO + ex.Message, Strings.MsgBoxErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
