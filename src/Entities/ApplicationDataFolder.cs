using System.Xml.Serialization;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Entities {
    [XmlRoot("ApplicationDataFolder")]
    public class ApplicationDataFolder : IGuid, ISecretResult<ApplicationDataFolder>  {
        [XmlAttribute("guid")]
        public string Guid { get; set; }

        [XmlElement("fullname")]
        public string FullName { get; set; }

        public ApplicationDataFolder() {
            Guid = System.Guid.NewGuid().ToString();
        }

        public ApplicationDataFolder Clone() {
            var clone = (ApplicationDataFolder)MemberwiseClone();
            clone.Guid = System.Guid.NewGuid().ToString();
            return clone;
        }
    }
}
