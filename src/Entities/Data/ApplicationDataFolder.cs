using System.Xml.Serialization;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Entities.Data {
    public class ApplicationDataFolder {
        [XmlAttribute("machine")]
        public string Machine { get; set; }

        [XmlAttribute("name")]
        public string FullName { get; set; }
    }
}
