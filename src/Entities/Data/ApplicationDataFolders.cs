using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Entities.Data {
    [XmlRoot("ApplicationDataFolders")]
    public class ApplicationDataFolders : List<ApplicationDataFolder>, ISecretResult<ApplicationDataFolders>  {
        public ApplicationDataFolders Clone() {
            var clone = new ApplicationDataFolders();
            clone.AddRange(this);
            return clone;
        }

        public ApplicationDataFolder FolderOnThisMachine() {
            var machine = System.Environment.MachineName.ToLower();
            return this.FirstOrDefault(f => f.Machine.ToLower() == machine);
        }
    }
}
