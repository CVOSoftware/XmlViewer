using System;
using XmlViewer.Editor.Const;
using XmlViewer.Logger;

namespace XmlViewer.Editor
{
    public class Program
    {
        [STAThread]
        public static void Main()
        {
            SystemLogger.Configure(ConfigConst.LOG_FILE_NAME);
            SystemLogger.Info(LoggerConst.APPLICATION_START);

            try
            {
                var app = new App();
                app.Run();
            }
            catch (Exception e)
            {
                SystemLogger.Fatal(e, LoggerConst.APPLICATION_EXECUTING_ERROR);
            }
            finally
            {
                SystemLogger.Info(LoggerConst.APPLICATION_CLOSE);
            }
        }
    }
}
