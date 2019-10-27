using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Entities.Data {
    public class SecretDataSources : ISecret<DataSources> {
        private DataSources vDefaultValue;

        public DataSources DefaultValue => vDefaultValue ?? (vDefaultValue = new DataSources() );

        public string Guid => "89DE3BA2-8479-432A-ACAA-62DCBF2F7A44";
    }
}
