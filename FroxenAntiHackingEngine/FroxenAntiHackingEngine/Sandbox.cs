using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace FroxenAntiHackingEngine
{
    public class Sandbox
    {
        [Flags]
        public enum SandboxConfig
        {
            EnableDisallowCodeDownload = 1,
            SandboxInterop = 2,
            LockEvidence = 3
        };

        protected static void RunSandbox(string AssemblyFile, PermissionSet Permissions, Evidence Evidence, [Optional] string[] Args, [Optional] SandboxConfig Config)
        {
            AppDomainSetup Domain = new AppDomainSetup();
            Domain.ApplicationBase = AssemblyFile;
            if(Config.HasFlag(SandboxConfig.EnableDisallowCodeDownload))
                Domain.DisallowCodeDownload = true;

            if (Config.HasFlag(SandboxConfig.SandboxInterop))
                Domain.SandboxInterop = true;

            if (Config.HasFlag(SandboxConfig.LockEvidence))
                Evidence.Locked = true;
            AppDomain Sandbox = AppDomain.CreateDomain(new Random().Next(0, 99999).ToString(), Evidence, Domain, Permissions);
            Sandbox.ExecuteAssembly(AssemblyFile, Args);
        }

        public static void RunCLRCodeSandboxed(string AssemblyFile, PermissionSet Permissions, Evidence Evidence,[Optional] string[] Args, [Optional] SandboxConfig Config)
        {
            RunSandbox(AssemblyFile, Permissions, Evidence, Args, Config);
        }
    }
}
