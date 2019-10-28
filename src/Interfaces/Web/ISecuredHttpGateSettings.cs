using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Entities.Web;
// ReSharper disable UnusedMember.Global

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Web {
    public interface ISecuredHttpGateSettings {
        string ApiUrl { get; set; }
        string LocalhostTempPath { get; set; }
        string LocalhostTempPathUrl { get; set; }
        SecuredHttpGateSettings Clone();
    }
}