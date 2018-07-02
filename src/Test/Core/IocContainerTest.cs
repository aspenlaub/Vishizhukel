using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Core;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Entities.Data;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Test.Core {
    [TestClass]
    public class IocContainerTest {
        protected IocContainer IocContainer;

        [TestMethod]
        public void CanRegisterTypes() {
            IocContainer = new IocContainer();
        }

        protected void RegisterTypes() {
            IocContainer.SetObject<IConnectionStringInfo>(new ConnectionStringInfo());
        }
    }
}
