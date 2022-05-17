using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.AccessControl;

namespace FroxenAntiHackingEngine
{
    public class Mitigations
    {
        public class WinRuntimeMitigations
        {
            private class BinarySignaturePolicy
            {
                [DllImport("kernel32.dll", SetLastError = true)]
                public static extern bool SetProcessMitigationPolicy(int policy, ref PROCESS_MITIGATION_BINARY_SIGNATURE_POLICY lpBuffer, int size);

                public struct PROCESS_MITIGATION_BINARY_SIGNATURE_POLICY
                {
                    public uint MicrosoftSignedOnly;
                }
            }

            private class FontsDisablePolicy
            {
                [DllImport("kernel32.dll", SetLastError = true)]
                public static extern bool SetProcessMitigationPolicy(int policy, ref PROCESS_MITIGATION_FONT_DISABLE_POLICY lpBuffer, int size);

                public struct PROCESS_MITIGATION_FONT_DISABLE_POLICY
                {
                    public uint DisableNonSystemFonts;
                }
            }

            private class DynamicCodeMitigation
            {
                [DllImport("kernel32.dll", SetLastError = true)]
                public static extern bool SetProcessMitigationPolicy(int policy, ref PROCESS_MITIGATION_DYNAMIC_CODE_POLICY lpBuffer, int size);

                public struct PROCESS_MITIGATION_DYNAMIC_CODE_POLICY
                {
                    public uint ProhibitDynamicCode;
                }
            }

            private class ChildProcessMitigation
            {
                [DllImport("kernel32.dll", SetLastError = true)]
                public static extern bool SetProcessMitigationPolicy(int policy, ref PROCESS_MITIGATION_CHILD_PROCESS_POLICY lpBuffer, int size);

                public struct PROCESS_MITIGATION_CHILD_PROCESS_POLICY
                {
                    public uint NoChildProcessCreation;
                }
            }

            private class ExtensionPointDisableMitigation
            {
                [DllImport("kernel32.dll", SetLastError = true)]
                public static extern bool SetProcessMitigationPolicy(int policy, ref PROCESS_MITIGATION_EXTENSION_POINT_DISABLE_POLICY lpBuffer, int size);

                public struct PROCESS_MITIGATION_EXTENSION_POINT_DISABLE_POLICY
                {
                    public uint DisableExtensionPoints;
                }
            }

            private class ImageLoadMitigationRemoteImages
            {
                [DllImport("kernel32.dll", SetLastError = true)]
                public static extern bool SetProcessMitigationPolicy(int policy, ref PROCESS_MITIGATION_IMAGE_LOAD_POLICY lpBuffer, int size);

                public struct PROCESS_MITIGATION_IMAGE_LOAD_POLICY
                {
                    public uint NoRemoteImages;
                }
            }

            private class Win32kCallsDisable
            {
                [DllImport("kernel32.dll", SetLastError = true)]
                public static extern bool SetProcessMitigationPolicy(int policy, ref PROCESS_MITIGATION_SYSTEM_CALL_DISABLE_POLICY lpBuffer, int size);

                public struct PROCESS_MITIGATION_SYSTEM_CALL_DISABLE_POLICY
                {
                    public uint DisallowWin32kSystemCalls;
                }
            }

            private class StrictHandleChecksMitigation
            {
                [DllImport("kernel32.dll", SetLastError = true)]
                public static extern bool SetProcessMitigationPolicy(int policy, ref PROCESS_MITIGATION_STRICT_HANDLE_CHECK_POLICY lpBuffer, uint size);

                public struct PROCESS_MITIGATION_STRICT_HANDLE_CHECK_POLICY
                {
                    public uint HandleExceptionsPermanentlyEnabled;
                    public uint RaiseExceptionOnInvalidHandleReference;
                }
            }

            [DllImport("kernel32.dll", SetLastError = true)]
            private static extern IntPtr GetProcessHeap();

            [DllImport("kernel32.dll", SetLastError = true)]
            private static extern uint GetProcessHeaps(uint NumberOfHeaps, IntPtr[] ProcessHeaps);

            [DllImport("kernel32.dll", SetLastError = true)]
            private static extern bool HeapSetInformation(IntPtr hHeap, uint HeapInformationClass, IntPtr HeapInformation, int HeapInformationLength);

            public static bool BinarySignatureMitigationPolicy_MSOnly()
            {
                BinarySignaturePolicy.PROCESS_MITIGATION_BINARY_SIGNATURE_POLICY Config = new BinarySignaturePolicy.PROCESS_MITIGATION_BINARY_SIGNATURE_POLICY();
                Config.MicrosoftSignedOnly = 1;
                return BinarySignaturePolicy.SetProcessMitigationPolicy(8, ref Config, Marshal.SizeOf(typeof(BinarySignaturePolicy.PROCESS_MITIGATION_BINARY_SIGNATURE_POLICY)));
            }

            public static bool DisableNonSystemFontsMitigationPolicy()
            {
                FontsDisablePolicy.PROCESS_MITIGATION_FONT_DISABLE_POLICY Config = new FontsDisablePolicy.PROCESS_MITIGATION_FONT_DISABLE_POLICY();
                Config.DisableNonSystemFonts = 1;
                return FontsDisablePolicy.SetProcessMitigationPolicy(9, ref Config, Marshal.SizeOf(typeof(FontsDisablePolicy.PROCESS_MITIGATION_FONT_DISABLE_POLICY)));
            }

            public static bool DynamicCodeMitigationPolicy()
            {
                DynamicCodeMitigation.PROCESS_MITIGATION_DYNAMIC_CODE_POLICY Config = new DynamicCodeMitigation.PROCESS_MITIGATION_DYNAMIC_CODE_POLICY();
                Config.ProhibitDynamicCode = 1;
                return DynamicCodeMitigation.SetProcessMitigationPolicy(2, ref Config, Marshal.SizeOf(typeof(DynamicCodeMitigation.PROCESS_MITIGATION_DYNAMIC_CODE_POLICY)));
            }

            public static bool NoChildProcessCreationMitigationPolicy()
            {
                ChildProcessMitigation.PROCESS_MITIGATION_CHILD_PROCESS_POLICY Config = new ChildProcessMitigation.PROCESS_MITIGATION_CHILD_PROCESS_POLICY();
                Config.NoChildProcessCreation = 1;
                return ChildProcessMitigation.SetProcessMitigationPolicy(13, ref Config, Marshal.SizeOf(typeof(ChildProcessMitigation.PROCESS_MITIGATION_CHILD_PROCESS_POLICY)));
            }

            public static bool ExtensionPointDisableMitigationPolicy()
            {
                ExtensionPointDisableMitigation.PROCESS_MITIGATION_EXTENSION_POINT_DISABLE_POLICY Config = new ExtensionPointDisableMitigation.PROCESS_MITIGATION_EXTENSION_POINT_DISABLE_POLICY();
                Config.DisableExtensionPoints = 1;
                return ExtensionPointDisableMitigation.SetProcessMitigationPolicy(6, ref Config, Marshal.SizeOf(typeof(ExtensionPointDisableMitigation.PROCESS_MITIGATION_EXTENSION_POINT_DISABLE_POLICY)));
            }

            public static bool ImageLoadMitigationPolicy_DisableLoadingRemoteImages()
            {
                ImageLoadMitigationRemoteImages.PROCESS_MITIGATION_IMAGE_LOAD_POLICY Config = new ImageLoadMitigationRemoteImages.PROCESS_MITIGATION_IMAGE_LOAD_POLICY();
                Config.NoRemoteImages = 1;
                return ImageLoadMitigationRemoteImages.SetProcessMitigationPolicy(10, ref Config, Marshal.SizeOf(typeof(ImageLoadMitigationRemoteImages.PROCESS_MITIGATION_IMAGE_LOAD_POLICY)));
            }

            public static bool DisableWin32kCallsMitigationPolicy()
            {
                Win32kCallsDisable.PROCESS_MITIGATION_SYSTEM_CALL_DISABLE_POLICY Config = new Win32kCallsDisable.PROCESS_MITIGATION_SYSTEM_CALL_DISABLE_POLICY();
                Config.DisallowWin32kSystemCalls = 1;
                return Win32kCallsDisable.SetProcessMitigationPolicy(4, ref Config, Marshal.SizeOf(typeof(Win32kCallsDisable.PROCESS_MITIGATION_SYSTEM_CALL_DISABLE_POLICY)));
            }

            public static bool StrictHandleChecksMitigationPolicy()
            {
                return AntiHackingEngineHelper.StrictHandleChecksMitigationPolicy();
            }

            public static bool ImageLoadMitigationPolicy_DisableLoadingLowMandatoryImages()
            {
                return AntiHackingEngineHelper.ImageLoadMitigationPolicy_DisableLoadingLowMandatoryImages();
            }

            public enum ASLROptions
            {
                EnableForceRelocateImages = 1,
                DisallowStrippedImages = 2,
                EnableBottomUpRandomization = 3,
                EnableHighEntropy = 4
            };

            public static void ConfigureASLR(ASLROptions Options)
            {
                AntiHackingEngineHelper.ConfigureASLR((uint)Options);
            }

            public static bool TerminateOnHeapCorruption()
            {
                return HeapSetInformation(GetProcessHeap(), 1, IntPtr.Zero, 0);
            }

            public static bool TerminateOnHeapCorruption(IntPtr HeapHandle)
            {
                return HeapSetInformation(HeapHandle, 1, IntPtr.Zero, 0);
            }

            public class WinMitigations_NewProcess
            {
                [DllImport("shell32.dll", SetLastError = true)]
                private static extern IntPtr ShellExecuteA(IntPtr hWnd, string lpOperation, string lpFile, string lpParameters, string lpDirectory, int nShowCmd);

                public static bool IsCFGEnabled()
                {
                    return AntiHackingEngineHelper.IsCFGEnabled();
                }

                public static void StartWithCFGEnabled(string FilePath, string[] Args)
                {
                    if (!Token.IsAdmin())
                        ShellExecuteA(IntPtr.Zero, "runas", "reg.exe", "add \"HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Image File Execution Options\\" + Path.GetFileName(FilePath) + "\" /v MitigationOptions /t REG_QWORD /d 0x10000000000", null, 0);
                    else
                    {
                        RegistryKey Reg = Registry.LocalMachine.CreateSubKey("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Image File Execution Options\\" + Path.GetFileName(FilePath));
                        Reg.SetValue("MitigationOptions", 0x10000000000, RegistryValueKind.QWord);
                    }
                    Task.Delay(1000).Wait();
                    string Arg = null;
                    if(Args != null)
                    {
                        for (int i = 0; i < Args.Length; i++)
                        {
                            if (Arg != null)
                            {
                                Arg = Arg + " " + Args[i];
                            }
                            else
                            {
                                Arg = Args[i];
                            }
                        }
                    }
                    Process.Start(FilePath, Arg);
                }
            }
        }

        /// <summary>
        /// Custom Mitigations which are non-microsoft implemented, none of the mitigations in these classes are implemented yet.
        /// </summary>
        public class CustomMitigations
        {
            /// <summary>
            /// Mitigations that Mitigates memory-issues.
            /// </summary>
            public class MemoryMitigations
            {
                /// <summary>
                /// Mitigates against Use-After-Free vulnerabilities by clearing the pointer of the memory block after it's freed, mitigates calls that uses the VirtualAlloc, malloc, HeapAlloc Functions Only.
                /// </summary>
                public static void UseAfterFreeMitigation()
                {

                }

                /// <summary>
                /// Mitigates against overwriting SEH by hooking UnhandledExceptionFilter to prevent it's usage.
                /// </summary>
                public static void PreventOverwritingSEH()
                {

                }
            }
        }
    }
}