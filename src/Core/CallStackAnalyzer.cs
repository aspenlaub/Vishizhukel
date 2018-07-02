using System;
using System.Diagnostics;
using System.Linq;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Core {
    public class CallStackAnalyzer {
        public static void CalledBy(int numberOfFramesToIgnore, out string fullyQualifiedMethodName, out bool isStatic, out bool constructor) {
            fullyQualifiedMethodName = @"N/A";
            constructor = false;
            isStatic = false;
            numberOfFramesToIgnore ++;
            var stackTrace = new StackTrace(true);
            var stackFramesArray = stackTrace.GetFrames();
            if (stackFramesArray == null || stackFramesArray.Length < numberOfFramesToIgnore) {
                throw new NullReferenceException("Could not get stack frames");
            }

            var stackFrames = stackFramesArray.ToList();
            stackFrames.RemoveRange(0, numberOfFramesToIgnore);
            var method = stackFrames[0].GetMethod();
            isStatic = method.IsStatic;
            if (method == null || method.DeclaringType == null) {
                throw new NullReferenceException("Could not get stack frames");
            }

            fullyQualifiedMethodName = method.DeclaringType.Namespace + '.' + method.Name;
            constructor = stackFrames.Any(s => s.GetMethod().IsConstructor);
        }
    }
}
