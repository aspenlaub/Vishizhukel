using System.Collections.Generic;
using System.Xml.Serialization;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Entities.Data {
    [XmlRoot("DataSources")]
    public class DataSources : List<DataSource>, ISecretResult<DataSources> {
        public DataSources Clone() {
            var clone = new DataSources();
            clone.AddRange(this);
            return clone;
        }
    }
}
