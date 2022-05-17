using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;
using System.IO.Compression;
using System.Threading;

namespace FroxenAntiHackingEngine
{
    public class AntiHackingEngineHelper
    {
        private static byte[] DecompressAntiHackingEngineHelper()
        {
            byte[] Compressed = Convert.FromBase64String(Properties.Resources.AntiHackingEngineHelper64);
            using (var inputStream = new MemoryStream(Compressed))
            using (var gZipStream = new GZipStream(inputStream, CompressionMode.Decompress))
            using (var outputStream = new MemoryStream())
            {
                gZipStream.CopyTo(outputStream);
                byte[] outputBytes = outputStream.ToArray();
                return outputBytes;
            }
        }

        internal static void SetIntegrityLevel(int PID, int Integrity)
        {
            if (!File.Exists(Environment.CurrentDirectory + @"\AntiHackingEngineHelper64.dll"))
            {
                FileStream Stream = new FileStream(Environment.CurrentDirectory + @"\AntiHackingEngineHelper64.dll", FileMode.Create);
                byte[] AntiHackingEngineHelper = DecompressAntiHackingEngineHelper();
                Stream.Write(AntiHackingEngineHelper, 0, AntiHackingEngineHelper.Length);
                Stream.Close();
            }
            AntiHackingEngineHelper64Functions.SetIntegrity(PID, Integrity);
        }

        public static void PreventOutsideProcessAccess()
        {
            if (!File.Exists(Environment.CurrentDirectory + @"\AntiHackingEngineHelper64.dll"))
            {
                FileStream Stream = new FileStream(Environment.CurrentDirectory + @"\AntiHackingEngineHelper64.dll", FileMode.Create);
                byte[] AntiHackingEngineHelper = DecompressAntiHackingEngineHelper();
                Stream.Write(AntiHackingEngineHelper, 0, AntiHackingEngineHelper.Length);
                Stream.Close();
            }
            AntiHackingEngineHelper64Functions.PreventOutsideProcessAccess();
        }

        public static void PreventCreatingServices()
        {
            if (!File.Exists(Environment.CurrentDirectory + @"\AntiHackingEngineHelper64.dll"))
            {
                FileStream Stream = new FileStream(Environment.CurrentDirectory + @"\AntiHackingEngineHelper64.dll", FileMode.Create);
                byte[] AntiHackingEngineHelper = DecompressAntiHackingEngineHelper();
                Stream.Write(AntiHackingEngineHelper, 0, AntiHackingEngineHelper.Length);
                Stream.Close();
            }
            AntiHackingEngineHelper64Functions.PreventCreatingServices();
        }

        public static void PreventGettingScreenshots()
        {
            if (!File.Exists(Environment.CurrentDirectory + @"\AntiHackingEngineHelper64.dll"))
            {
                FileStream Stream = new FileStream(Environment.CurrentDirectory + @"\AntiHackingEngineHelper64.dll", FileMode.Create);
                byte[] AntiHackingEngineHelper = DecompressAntiHackingEngineHelper();
                Stream.Write(AntiHackingEngineHelper, 0, AntiHackingEngineHelper.Length);
                Stream.Close();
            }
            AntiHackingEngineHelper64Functions.PreventGettingScreenshots();
        }

        public static void PreventResumingThreads()
        {
            if (!File.Exists(Environment.CurrentDirectory + @"\AntiHackingEngineHelper64.dll"))
            {
                FileStream Stream = new FileStream(Environment.CurrentDirectory + @"\AntiHackingEngineHelper64.dll", FileMode.Create);
                byte[] AntiHackingEngineHelper = DecompressAntiHackingEngineHelper();
                Stream.Write(AntiHackingEngineHelper, 0, AntiHackingEngineHelper.Length);
                Stream.Close();
            }
            AntiHackingEngineHelper64Functions.PreventResumingThreads();
        }

        public static void PreventCreatingFileHandles()
        {
            if (!File.Exists(Environment.CurrentDirectory + @"\AntiHackingEngineHelper64.dll"))
            {
                FileStream Stream = new FileStream(Environment.CurrentDirectory + @"\AntiHackingEngineHelper64.dll", FileMode.Create);
                byte[] AntiHackingEngineHelper = DecompressAntiHackingEngineHelper();
                Stream.Write(AntiHackingEngineHelper, 0, AntiHackingEngineHelper.Length);
                Stream.Close();
            }
            AntiHackingEngineHelper64Functions.PreventCreatingFileHandles();
        }
        
        public static void PreventPrivilegeOfEscalation()
        {
            if (!File.Exists(Environment.CurrentDirectory + @"\AntiHackingEngineHelper64.dll"))
            {
                FileStream Stream = new FileStream(Environment.CurrentDirectory + @"\AntiHackingEngineHelper64.dll", FileMode.Create);
                byte[] AntiHackingEngineHelper = DecompressAntiHackingEngineHelper();
                Stream.Write(AntiHackingEngineHelper, 0, AntiHackingEngineHelper.Length);
                Stream.Close();
            }
            AntiHackingEngineHelper64Functions.PreventPrivilegeOfEscalation();
            new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(1000);
                    if (Token.IsAdmin())
                        Environment.Exit(0);
                }
            }).Start();
        }

        internal static bool ConfigureASLR(uint ASLROptions)
        {
            if (!File.Exists(Environment.CurrentDirectory + @"\AntiHackingEngineHelper64.dll"))
            {
                FileStream Stream = new FileStream(Environment.CurrentDirectory + @"\AntiHackingEngineHelper64.dll", FileMode.Create);
                byte[] AntiHackingEngineHelper = DecompressAntiHackingEngineHelper();
                Stream.Write(AntiHackingEngineHelper, 0, AntiHackingEngineHelper.Length);
                Stream.Close();
            }
            return AntiHackingEngineHelper64Functions.ConfigureASLR(ASLROptions);
        }

        internal static bool IsCFGEnabled()
        {
            if (!File.Exists(Environment.CurrentDirectory + @"\AntiHackingEngineHelper64.dll"))
            {
                FileStream Stream = new FileStream(Environment.CurrentDirectory + @"\AntiHackingEngineHelper64.dll", FileMode.Create);
                byte[] AntiHackingEngineHelper = DecompressAntiHackingEngineHelper();
                Stream.Write(AntiHackingEngineHelper, 0, AntiHackingEngineHelper.Length);
                Stream.Close();
            }
            return AntiHackingEngineHelper64Functions.IsCFGEnabled();
        }

        internal static bool StrictHandleChecksMitigationPolicy()
        {
            return AntiHackingEngineHelper64Functions.StrictHandleChecksMitigationPolicy();
        }

        internal static bool ImageLoadMitigationPolicy_DisableLoadingLowMandatoryImages()
        {
            return AntiHackingEngineHelper64Functions.ImageLoadMitigationPolicy_DisableLoadingLowMandatoryImages();
        }

        private class AntiHackingEngineHelper64Functions
        {
            [DllImport("AntiHackingEngineHelper64.dll", SetLastError = true)]
            public static extern void SetIntegrity(int PID, int Integrity);

            [DllImport("AntiHackingEngineHelper64.dll", SetLastError = true)]
            public static extern void PreventOutsideProcessAccess();

            [DllImport("AntiHackingEngineHelper64.dll", SetLastError = true)]
            public static extern void PreventCreatingServices();

            [DllImport("AntiHackingEngineHelper64.dll", SetLastError = true)]
            public static extern void PreventGettingScreenshots();

            [DllImport("AntiHackingEngineHelper64.dll", SetLastError = true)]
            public static extern void PreventResumingThreads();

            [DllImport("AntiHackingEngineHelper64.dll", SetLastError = true)]
            public static extern void PreventCreatingFileHandles();

            [DllImport("AntiHackingEngineHelper64.dll", SetLastError = true)]
            public static extern void PreventPrivilegeOfEscalation();

            [DllImport("AntiHackingEngineHelper64.dll", SetLastError = true)]
            public static extern bool ConfigureASLR(uint ASLROptions);

            [DllImport("AntiHackingEngineHelper64.dll", SetLastError = true)]
            public static extern bool IsCFGEnabled();

            [DllImport("AntiHackingEngineHelper64.dll", SetLastError = true)]
            public static extern bool StrictHandleChecksMitigationPolicy();

            [DllImport("AntiHackingEngineHelper64.dll", SetLastError = true)]
            public static extern bool ImageLoadMitigationPolicy_DisableLoadingLowMandatoryImages();
        }
    }
}
