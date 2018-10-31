using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Entities.Data {
    public class SecretConnectionStringInfos : ISecret<ConnectionStringInfos> {
        private ConnectionStringInfos vDefaultValue;
        public ConnectionStringInfos DefaultValue => vDefaultValue ?? (vDefaultValue = new ConnectionStringInfos());

        public string Guid => "A959B139-A74A-4EF0-927A-83339465F76C";
    }
}
