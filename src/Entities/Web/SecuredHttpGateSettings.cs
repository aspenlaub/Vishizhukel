using System.Xml.Serialization;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Web;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Entities.Web {
    [XmlRoot("SecuredHttpGateSettings")]
    public class SecuredHttpGateSettings : ISecretResult<SecuredHttpGateSettings>, ISecuredHttpGateSettings {
        [XmlElement("apiurl")]
        public string ApiUrl { get; set; }

        [XmlElement("localhosttemppath")]
        public string LocalhostTempPath { get; set; }

        [XmlElement("localhosttemppathurl")]
        public string LocalhostTempPathUrl { get; set; }

        public SecuredHttpGateSettings Clone() {
            var clone = (SecuredHttpGateSettings)MemberwiseClone();
            return clone;
        }
    }
}
