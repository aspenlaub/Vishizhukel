using System.Collections.Generic;
using System.Xml.Serialization;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Entities.Data {
    [XmlRoot("ConnectionStringInfos")]
    public class ConnectionStringInfos : List<ConnectionStringInfo>, ISecretResult<ConnectionStringInfos> {
        public ConnectionStringInfos Clone() {
            var clone = new ConnectionStringInfos();
            clone.AddRange(this);
            return clone;
        }
    }
}
