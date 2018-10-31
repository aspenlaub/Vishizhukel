using System;
using System.Collections.Generic;
// ReSharper disable UnusedMember.Global

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Core {
    public class IocContainer {
        private static readonly object LockObject = new object();
        private Dictionary<string, bool> Flags { get; }
        private Dictionary<Type, object> Objects { get; }

        public IocContainer() {
            Flags = new Dictionary<string, bool>();
            Objects = new Dictionary<Type, object>();
        }

        public bool Flag(string key) {
            if (!Flags.ContainsKey(key)) {
                throw new Exception("Unknown flag " + key);
            }

            return Flags[key];
        }

        public void SetFlag(string key, bool value) {
            if (Flags.ContainsKey(key)) { return; }

            VerifyCallFromNonStaticRegisterTypesMethodAndNotFromConstructor();
            Flags[key] = value;
        }

        public T Object<T>() where T : class {
            if (!Objects.ContainsKey(typeof(T))) {
                throw new Exception("ObjectFactory does not know a " + typeof(T).Name);
            }

            return (T)Objects[typeof(T)];
        }

        public void SetObject<T>(T obj) where T : class {
            if (Objects.ContainsKey(typeof(T))) { return; }

            VerifyCallFromNonStaticRegisterTypesMethodAndNotFromConstructor();
            Objects[typeof(T)] = obj;
        }

        private static IocContainer vGlobalInstance;

        public static IocContainer GlobalInstance {
            get {
                lock(LockObject) {
                    return vGlobalInstance ?? (vGlobalInstance = new IocContainer());
                }
            }
        }

        public static void ResetGlobalInstance() {
            lock (LockObject) {
                vGlobalInstance = new IocContainer();
            }
        }

        protected void VerifyCallFromNonStaticRegisterTypesMethodAndNotFromConstructor() {
            CallStackAnalyzer.CalledBy(2, out var fullyQualifiedMethodName, out var isStatic, out var constructor);
            if (!fullyQualifiedMethodName.EndsWith(@".RegisterTypes")) {
                throw new Exception($"Ioc container must be configured in a non-static method called RegisterTypes, not in {fullyQualifiedMethodName}. Make sure the method or the calling method is decorated with MethodImpl(MethodImplOptions.NoOptimization | MethodImplOptions.NoInlining)");
            }
            if (isStatic) {
                throw new Exception($"{fullyQualifiedMethodName} must not be static");
            }
            if (constructor) {
                throw new Exception($"Ioc container configuration method {fullyQualifiedMethodName} cannot be called in a constructor");
            }
        }
    }
}