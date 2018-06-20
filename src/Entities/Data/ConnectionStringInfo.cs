using System.Xml.Serialization;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Data;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Entities.Data {
    public class ConnectionStringInfo : IConnectionStringInfo {
        [XmlAttribute("namespace")]
        public string Namespace { get; set; }

        [XmlAttribute("environment")]
        public EnvironmentType EnvironmentType { get; set; }

        [XmlAttribute("connectionstring")]
        public string ConnectionString { get; set; }
    }
}
