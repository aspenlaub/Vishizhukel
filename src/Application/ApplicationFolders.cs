using System;
using System.IO;
using System.Linq;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Aspenlaub.Net.CSharp.Foundry.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Application {
    public class ApplicationFolders {
        protected string ApplicationName;
        protected string ApplicationDataFolder;

        public ApplicationFolders(string applicationName, string applicationDataFolder) {
            ApplicationName = applicationName;
            ApplicationDataFolder = applicationDataFolder;
        }

        public string ApplicationFolder(string subFolder) {
            var folder = ApplicationDataFolder + ApplicationName + '\\';
            foreach (var s in subFolder.Split('\\').Where(s => s.Length != 0)) {
                folder = folder + s + '\\';
                if (Directory.Exists(folder)) { continue; }

                Directory.CreateDirectory(folder);
            }

            return folder;
        }

        public string ApplicationFolder(EnvironmentType environmentType, string subFolder) {
            return ApplicationFolder(Enum.GetName(typeof(EnvironmentType), environmentType) + '\\' + subFolder);
        }
    }
}
