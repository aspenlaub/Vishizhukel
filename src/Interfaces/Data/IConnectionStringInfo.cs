using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
// ReSharper disable UnusedMember.Global

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Data {
    public interface IConnectionStringInfo {
        string Namespace { get; set; }
        EnvironmentType EnvironmentType { get; set; }
        string ConnectionString { get; set; }
    }
}
