﻿using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Extensions;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Core {
    public class ExceptionSaver {
        public static async Task SaveUnhandledExceptionAsync(IFolder exceptionLogFolder, Exception exception, string source, Func<SavedException, Task> onExceptionSaved) {
            var assemblyName = Assembly.GetExecutingAssembly().GetName();
            var title = $"Unhandled exception in {assemblyName.Name} v{assemblyName.Version}";
            var message = exception.Message;
            var stackTrace = exception.StackTrace;
            string innerMessage = "", innerStackTrace = "";
            if (exception.InnerException != null) {
                innerMessage = exception.InnerException.Message;
                innerStackTrace = exception.InnerException.StackTrace;
            }
            await SaveExceptionAsync(exceptionLogFolder, title, message, stackTrace, innerMessage, innerStackTrace, onExceptionSaved);
        }

        private static async Task SaveExceptionAsync(IFolder exceptionLogFolder, string title, string message, string stackTrace, string innerMessage, string innerStackTrace, Func<SavedException, Task> onExceptionSaved) {
            exceptionLogFolder.CreateIfNecessary();

            var fileContents = "Title:\r\n" + title + "\r\nMessage:\r\n" + message + "\r\nStack trace:\r\n" + stackTrace + "\r\nInner message:\r\n" + innerMessage + "\r\nInner stack trace:\r\n" + innerStackTrace;
            var md5 = MD5.Create();
            var inputBytes = Encoding.UTF8.GetBytes(fileContents);
            var hash = md5.ComputeHash(inputBytes);
            var sb = new StringBuilder();
            foreach (var t in hash) { sb.Append(t.ToString("X2")); }

            var exceptionName = "Exception-" + sb;
            var fileName = exceptionLogFolder.FullName + '\\' + exceptionName + ".txt";
            await File.WriteAllTextAsync(fileName, fileContents);

            var savedException = new SavedException {
                ExceptionName = exceptionName,
                FileContents = fileContents,
                FileFullName = fileName
            };
            await onExceptionSaved(savedException);
        }
    }

    public class SavedException {
        public string ExceptionName { get; set; }
        public string FileContents { get; set; }
        public string FileFullName { get; set; }
    }
}
