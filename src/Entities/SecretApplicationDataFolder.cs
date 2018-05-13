using System;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Entities {
    public class SecretApplicationDataFolder : ISecret<ApplicationDataFolder> {
        private ApplicationDataFolder vDefaultValue;
        public ApplicationDataFolder DefaultValue {
            get { return vDefaultValue ?? (vDefaultValue = new ApplicationDataFolder { FullName = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) }); }
        }

        public string Guid { get { return "67B90C8D-832C-413B-848C-B9EEC0FC6E6A"; } }
    }
}
