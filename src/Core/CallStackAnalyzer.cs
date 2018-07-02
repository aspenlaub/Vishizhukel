using System;
using System.Diagnostics;
using System.Linq;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Core {
    public class CallStackAnalyzer {
        public static void CalledBy(StackTrace stackTrace, out string fullyQualifiedMethodName, out bool isStatic, out bool constructor) {
            fullyQualifiedMethodName = @"N/A";
            constructor = false;
            isStatic = false;
            var stackFrames = stackTrace.GetFrames();
            if (stackFrames == null || stackFrames.Length < 1) {
                throw new NullReferenceException("Could not get stack frames");
            }

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
