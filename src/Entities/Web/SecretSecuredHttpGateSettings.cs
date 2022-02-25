using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Entities.Web {
    public class SecretSecuredHttpGateSettings : ISecret<SecuredHttpGateSettings> {
        private static SecuredHttpGateSettings PrivateDefaultSecuredHttpGateSettings;

        public SecuredHttpGateSettings DefaultValue => PrivateDefaultSecuredHttpGateSettings ??= new SecuredHttpGateSettings {
            ApiUrl = "http://localhost/securedhttpgateapi.php", LocalhostTempPath = @"c:\wamp\www\temp\", LocalhostTempPathUrl = "http://localhost/temp/"
        };

        public string Guid => "AE3335BD-B54E-48F6-B9DA-15AC091AB93F";
    }
}
