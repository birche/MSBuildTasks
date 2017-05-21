using System;
using System.Diagnostics;
using Microsoft.Build.Framework;
using System.IO;
using System.IO.Compression;

namespace DeployerTarget
{
    public class Deployer : Microsoft.Build.Utilities.Task
    {
        public string PublishedOutput { get; set; }
        public string PackageName { get; set; }
        public string ApplicationName { get; set; }
        public string ApplicationVersion { get; set; }
        public string Rid { get; set; }

        private string ZipPublishedOutput()
        {
            var pubout = Path.Combine(Directory.GetCurrentDirectory(), PublishedOutput);
            var filename = Path.Combine(pubout, $"{ApplicationName}-{ApplicationVersion}.zip");
            Log.LogMessage(MessageImportance.High, $"Filename: {filename}");
            Log.LogMessage(MessageImportance.High, $"PubOut: {pubout}");
            if(File.Exists(filename))
                File.Delete(filename);
            
            var tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Log.LogMessage(MessageImportance.High, $"TempFile: {tempFile}");
            try {
                ZipFile.CreateFromDirectory(pubout, tempFile);
                Log.LogMessage(MessageImportance.High, "Did zip the output");
                File.Copy(tempFile, filename);
                Log.LogMessage(MessageImportance.High, $"Did copy to {filename}");
                return filename;
            } 
            catch 
            {
                throw;
            } 
            finally 
            {
                File.Delete(tempFile);
            }
        }

        public override bool Execute()
        {
            //Debugger.Launch();
            Log.LogMessage(MessageImportance.High, "Hello from Deployer!");
            Log.LogMessage(MessageImportance.High, $"PublishedOutput=${PublishedOutput}");
            Log.LogMessage(MessageImportance.High, $"PackageName={PackageName}");
            Log.LogMessage(MessageImportance.High, $"ApplicationName={ApplicationName}");
            Log.LogMessage(MessageImportance.High, $"ApplicationVersion={ApplicationVersion}");
            Log.LogMessage(MessageImportance.High, $"Rid={Rid}");

            try 
            {
                var zipFile = ZipPublishedOutput();
                Log.LogMessage(MessageImportance.High, $"Published output is now packed to the file {zipFile}");
            }
            catch
            {
                Log.LogError("Failed to zip the published output");
                return false;
            }
            return true;
        }
    }
}
