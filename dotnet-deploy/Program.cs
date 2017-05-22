using System;
using System.Diagnostics;

namespace dotnet_deploy
{
    class Program
    {
        static void Main(string[] args)
        {
            var arguments = $"msbuild /t:Deployer /p:RuntimeIdentifier={rid} /p:UploadAddress={addr}";
            var ps = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = arguments,
            };
            var process = new Process
            {
                StartInfo = ps,
            };

            process.Start();
        }
    }
}
