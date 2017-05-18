using System;
using System.Diagnostics;
using Microsoft.Build.Framework;

namespace DeployerTarget
{
    public class Deployer : Microsoft.Build.Utilities.Task
    {
        public string PublishedOutput { get; set; }
        public string PackageName { get; set; }
        public string ApplicationName { get; set; }
        public string ApplicationVersion { get; set; }
        public string Rid { get; set; }

        public override bool Execute()
        {
            //Debugger.Launch();
            Log.LogMessage(MessageImportance.High, "Hello from Deployer!");
            Log.LogMessage(MessageImportance.High, $"PublishedOutput=${PublishedOutput}");
            Log.LogMessage(MessageImportance.High, $"PackageName={PackageName}");
            Log.LogMessage(MessageImportance.High, $"ApplicationName={ApplicationName}");
            Log.LogMessage(MessageImportance.High, $"ApplicationVersion={ApplicationVersion}");
            Log.LogMessage(MessageImportance.High, $"Rid={Rid}");

            return true;
        }
    }
}
