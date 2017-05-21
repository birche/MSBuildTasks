using System;
using System.Diagnostics;
using Microsoft.Build.Framework;
using System.IO;
using System.IO.Compression;
using System.Net.Http;

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

        private bool PostZippedFileToHttp(string path, string uriString)
        {
            HttpContent streamContent = new StreamContent(File.OpenRead(path));
            using (var client = new HttpClient())
            {
                using (var formData = new MultipartFormDataContent())
                {
                    formData.Add(streamContent, "files", Path.GetFileName(path));
                    HttpResponseMessage response = client.PostAsync(new Uri(uriString), formData).Result;
                    if (!response.IsSuccessStatusCode)
                        Debug.WriteLine("Failed to send");

                    Debug.WriteLine(response.Content.ReadAsStringAsync().Result);
                }
            }
        }

        //[HttpPost("upload-files")]
        //[AllowAnonymous]
        //public Task<string> FileUploadAction(IFormFileCollection files)
        //{
        //    var sb = new StringBuilder().AppendLine("** File upload **").AppendLine($"File count: {files.Count}");
        //    foreach (var file in files)
        //    {
        //        if (file.Length > 0 && file.FileName.EndsWith(".zip", StringComparison.CurrentCultureIgnoreCase))
        //        {
        //            using (Stream zipStream = file.OpenReadStream())
        //            {
        //                m_ProjectPackage.LoadProjectZipToRepository(zipStream);
        //            }
        //            sb.AppendLine($"Did unzip file {file.FileName}");
        //        }
        //        else if (file.Length > 0 && file.FileName.EndsWith(".override", StringComparison.CurrentCultureIgnoreCase))
        //        {
        //            using (Stream s = file.OpenReadStream())
        //            {
        //                m_ProjectPackage.StoreGlobalTagsOverrideAsync(s).ConfigureAwait(false);
        //            }
        //        }
        //    }
        //    return Task.FromResult(sb.ToString());
        //}

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
