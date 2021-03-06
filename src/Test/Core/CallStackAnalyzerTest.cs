﻿using System.Runtime.CompilerServices;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Test.Core {
    [TestClass]
    public class CallStackAnalyzerTest {
        [TestMethod]
        public void CanDetermineCallContext() {
            CallStackAnalyzer.CalledBy(0, out var fullyQualifiedClassName, out var isStatic, out var constructor);
            Assert.IsFalse(isStatic);
            Assert.IsFalse(constructor);
            Assert.AreEqual(typeof(CallStackAnalyzerTest).Namespace + '.' + nameof(CanDetermineCallContext), fullyQualifiedClassName);

            var testObject = new CallStackAnalyzerTestObject();
            Assert.IsNotNull(testObject);
            Assert.IsTrue(CallStackAnalyzerTestObject.Constructor);
            Assert.IsFalse(CallStackAnalyzerTestObject.IsStatic);
            Assert.AreEqual(typeof(CallStackAnalyzerTest).Namespace + "..ctor", CallStackAnalyzerTestObject.FullyQualifiedMethodName);

            CallStackAnalyzerTestObject.RegisterTypes();
            Assert.IsFalse(CallStackAnalyzerTestObject.Constructor);
            Assert.IsTrue(CallStackAnalyzerTestObject.IsStatic);
            Assert.AreEqual(typeof(CallStackAnalyzerTest).Namespace + '.' + nameof(CallStackAnalyzerTestObject.RegisterTypes), CallStackAnalyzerTestObject.FullyQualifiedMethodName);
        }
    }

    internal class CallStackAnalyzerTestObject {
        internal static string FullyQualifiedMethodName;
        internal static bool IsStatic, Constructor;

        [MethodImpl(MethodImplOptions.NoOptimization | MethodImplOptions.NoInlining)]
        internal CallStackAnalyzerTestObject() {
            CallStackAnalyzer.CalledBy(0, out FullyQualifiedMethodName, out IsStatic, out Constructor);
        }

        [MethodImpl(MethodImplOptions.NoOptimization | MethodImplOptions.NoInlining)]
        internal static void RegisterTypes() {
            CallStackAnalyzer.CalledBy(0, out FullyQualifiedMethodName, out IsStatic, out Constructor);
        }
    }
}
