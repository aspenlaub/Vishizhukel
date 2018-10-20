using System;
using System.Linq;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Components;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Entities;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Application {
    public class ApplicationFolders {
        protected string ApplicationName;
        protected string ApplicationDataFolder;

        public ApplicationFolders(string applicationName, string applicationDataFolder) {
            ApplicationName = applicationName;
            ApplicationDataFolder = applicationDataFolder;
        }

        public IFolder ApplicationFolder(string subFolder) {
            IFolder folder = new Folder(ApplicationDataFolder);
            folder = folder.SubFolder(ApplicationName);
            foreach (var s in subFolder.Split('\\').Where(s => s.Length != 0)) {
                folder = folder.SubFolder(s);
                folder.CreateIfNecessary();
            }

            return folder;
        }

        public IFolder ApplicationFolder(EnvironmentType environmentType, string subFolder) {
            return ApplicationFolder(Enum.GetName(typeof(EnvironmentType), environmentType) + '\\' + subFolder);
        }
    }
}
