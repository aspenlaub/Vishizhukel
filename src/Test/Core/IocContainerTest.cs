using System.Runtime.CompilerServices;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Core;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Entities.Data;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Test.Core {
    [TestClass]
    public class IocContainerTest {
        protected IocContainer IocContainer;

        [TestMethod, MethodImpl(MethodImplOptions.NoOptimization | MethodImplOptions.NoInlining)]
        public void CanRegisterTypes() {
            IocContainer = new IocContainer();
            RegisterTypes();
        }

        [MethodImpl(MethodImplOptions.NoOptimization | MethodImplOptions.NoInlining)]
        protected void RegisterTypes() {
            IocContainer.SetObject<IConnectionStringInfo>(new ConnectionStringInfo());
        }
    }
}
