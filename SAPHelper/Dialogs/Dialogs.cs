using SAPbouiCOM;

namespace SAPHelper
{
    public static class Dialogs
    {
        public static Application SBOApplication;

        public static void Info(string msg, BoMessageTime messageTime = BoMessageTime.bmt_Short)
        {
            BarMessage(msg, BoStatusBarMessageType.smt_Warning);
        }

        public static void PopupInfo(string msg)
        {
            Info(msg);
            SBOApplication.MessageBox(msg);
        }

        public static void Success(string msg, BoMessageTime messageTime = BoMessageTime.bmt_Short)
        {
            BarMessage(msg, BoStatusBarMessageType.smt_Success);
        }

        public static void PopupSuccess(string msg)
        {
            Success(msg);
            SBOApplication.MessageBox(msg);
        }

        public static void Error(string msg)
        {
            BarMessage(msg, BoStatusBarMessageType.smt_Error);
        }

        public static void PopupError(string msg, BoMessageTime messageTime = BoMessageTime.bmt_Short)
        {
            Error(msg);
            SBOApplication.MessageBox(msg);
        }

        public static bool Confirm(string msg)
        {
            return SBOApplication.MessageBox(msg, 1, "Sim", "Não") == 1;
        }

        private static void BarMessage(string message, BoStatusBarMessageType messageType, BoMessageTime messageTime = BoMessageTime.bmt_Short)
        {
            SBOApplication.StatusBar.SetText(message, messageTime, messageType);
        }

        public static void RecebeSBOApplication(Application application)
        {
            SBOApplication = application;
        }
    }
}
