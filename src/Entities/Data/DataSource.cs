using System.Xml.Serialization;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Entities.Data {
    public class DataSource : IGuid {
        [XmlAttribute("guid")]
        public string Guid { get; set; }

        [XmlAttribute("machineid")]
        public string MachineId { get; set; }

        [XmlAttribute("datasource")]
        public string TheDataSource { get; set; }
    }
}
