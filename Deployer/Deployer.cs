using System;
using System.Diagnostics;
using Microsoft.Build.Framework;

namespace DeployerTarget
{
    public class Deployer : Microsoft.Build.Utilities.Task
    {
        public string ApplicationName { get; set; }

        public override bool Execute()
        {
            //Debugger.Launch();
            Log.LogMessage(MessageImportance.High, "Hello from Deployer!");

            Console.WriteLine("Hello world, from Deployer console...");

            return true;
        }
    }
}
