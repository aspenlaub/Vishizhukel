using System;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Entities.Data {
    public class SecretApplicationDataFolders : ISecret<ApplicationDataFolders> {
        public static readonly string DefaultFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

        private ApplicationDataFolders vDefaultValue;
        public ApplicationDataFolders DefaultValue {
            get { return vDefaultValue ?? ( vDefaultValue = new ApplicationDataFolders { new ApplicationDataFolder { FullName = DefaultFolder, Machine = Environment.MachineName } }); }
        }

        public string Guid { get { return "67B90C8D-832C-413B-848C-B9EEC0FC6E6A"; } }
    }
}
