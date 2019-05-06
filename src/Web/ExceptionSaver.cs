using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Components;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Entities;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Entities.Web;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Web;
// ReSharper disable UnusedMember.Global

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Web {
    public class ExceptionSaver {
        public static void SaveUnhandledException(Exception exception, string source) {
            var assemblyName = Assembly.GetExecutingAssembly().GetName();
            var title = $"Unhandled exception in {assemblyName.Name} v{assemblyName.Version}";
            var message = exception.Message;
            var stackTrace = exception.StackTrace;
            string innerMessage = "", innerStackTrace = "";
            if (exception.InnerException != null) {
                innerMessage = exception.InnerException.Message;
                innerStackTrace = exception.InnerException.StackTrace;
            }
            SaveException(title, message, stackTrace, innerMessage, innerStackTrace);
        }

        private static void SaveException(string title, string message, string stackTrace, string innerMessage, string innerStackTrace) {
            var folder = Path.GetTempPath()+ @"\AspenlaubExceptions\";
            if (!Directory.Exists(folder)) {
                Directory.CreateDirectory(folder);
            }

            var fileContents = "Title:\r\n" + title + "\r\nMessage:\r\n" + message + "\r\nStack trace:\r\n" + stackTrace + "\r\nInner message:\r\n" + innerMessage + "\r\nInner stack trace:\r\n" + innerStackTrace;
            var md5 = MD5.Create();
            var inputBytes = Encoding.UTF8.GetBytes(fileContents);
            var hash = md5.ComputeHash(inputBytes);
            var sb = new StringBuilder();
            foreach (var t in hash) { sb.Append(t.ToString("X2")); }

            var exceptionName = "Exception-" + sb;
            var fileName = folder + exceptionName + ".txt";
            File.WriteAllText(fileName, fileContents);

            var repository = new SecretRepository(new ComponentProvider());
            var securedHttpGateSettingsSecret = new SecretSecuredHttpGateSettings();
            var errorsAndInfos = new ErrorsAndInfos();
            var securedHttpGateSettings = repository.GetAsync(securedHttpGateSettingsSecret, errorsAndInfos).Result;

            ISecuredHttpGate gate = new SecuredHttpGate(new HttpGate(), securedHttpGateSettings);
            gate.RegisterDefectAsync(exceptionName, fileContents, false).Wait();
        }
    }
}
