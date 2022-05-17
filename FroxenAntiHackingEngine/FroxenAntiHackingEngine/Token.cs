using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Principal;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace FroxenAntiHackingEngine
{
    public class Token
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr OpenProcess(long dwDesiredAccess, bool bInherit, int PID);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern bool OpenProcessToken(IntPtr hProcess, long DesiredAccess, out IntPtr TokenHandle);

        [DllImport("advapi32.dll", ExactSpelling = true, SetLastError = true)]
        private static extern bool AdjustTokenPrivileges(IntPtr htok, bool DisableAllPrivileges, ref TOKEN_PRIVILEGES NewState, int Length, IntPtr PreviousState, IntPtr ReturnLength);

        [DllImport("advapi32.dll", SetLastError = true)]
        internal static extern bool LookupPrivilegeValue(string SystemName, string Name, ref long lpLuid);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool CloseHandle(IntPtr Handle);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SaferCreateLevel(uint dwScopeId, uint dwLevelId, uint OpenFlags, out IntPtr pLevelHandle, IntPtr lpReserved);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SaferComputeTokenFromLevel(IntPtr LevelHandle, IntPtr InAccessToken, out IntPtr OutAccessToken, uint dwFlags, IntPtr lpReserved);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SaferCloseLevel(IntPtr hLevelHandle);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool CreateRestrictedToken(IntPtr ExistingTokenHandle, uint Flags, uint DisableSidCount, IntPtr SidsToDisable, uint DeletePrivilegeCount, IntPtr PrivilegesToDelete, uint RestrictedSidCount, IntPtr SidsToRestrict, out IntPtr NewTokenHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool CreateProcess(string lpApplicationName, string lpCommandLine, ref SECURITY_ATTRIBUTES lpProcessAttributes, ref SECURITY_ATTRIBUTES lpThreadAttributes, bool bInheritHandles, uint dwCreationFlags, IntPtr lpEnvironment, string lpCurrentDirectory, [In] ref STARTUPINFO lpStartupInfo, out PROCESS_INFORMATION lpProcessInformation);

        [DllImport("ntdll.dll", SetLastError = true)]
        private static extern int NtSetInformationProcess(IntPtr hProcess, int InformationClass, ref PROCESS_ACCESS_TOKEN Information, int InformationLength);

        [DllImport("kernel32.dll",SetLastError = true)]
        private static extern uint ResumeThread(IntPtr hThread);

        /*/

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetTokenInformation(IntPtr hToken, uint TokenInformationClass, TOKEN_MANDATORY_LABEL TokenInformation, int TokenInformationLength);

        [DllImport("advapi32.dll")]
        private static extern int GetLengthSid(ref SID Sid);

        /*/

        public static bool IsAdmin()
        {
            return new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct TOKEN_PRIVILEGES
        {
            public int Count;
            public long Luid;
            public long Attr;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct PROCESS_ACCESS_TOKEN
        {
            public IntPtr Token;
            public IntPtr Thread;
        }

        public enum Operation
        {
            DisablePrivilege = 0x00000000,
            EnablePrivilege = 0x00000002,
            RemovePrivilege = 0X00000004
        };

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct TOKEN_USER
        {
            IntPtr User;
        }

        public enum IntegrityLevel
        {
            Untrusted = 1,
            Low = 2
        };

        public static bool SetTokenPrivilege(int ProcessId, string PrivilegeName, Operation WhatToDo)
        {
            TOKEN_PRIVILEGES tp;
            IntPtr TokenHandle = IntPtr.Zero;
            IntPtr RemoteProcess = Process.GetProcessById(ProcessId).Handle;
            OpenProcessToken(RemoteProcess, 0x00000020 | 0x00000008, out TokenHandle);
            tp.Count = 1;
            tp.Luid = 0;
            tp.Attr = (long)WhatToDo;
            LookupPrivilegeValue(null, PrivilegeName, ref tp.Luid);
            bool IsSuccess = AdjustTokenPrivileges(TokenHandle, false, ref tp, 0, IntPtr.Zero, IntPtr.Zero);
            CloseHandle(RemoteProcess);
            CloseHandle(TokenHandle);
            return IsSuccess;
        }
        /*/

        [StructLayout(LayoutKind.Sequential)]
        private struct SID_IDENTIFIER_AUTHORITY
        {
            public byte[] Value;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct SID
        {
            public byte Revision;
            public byte SubAuthorityCount;
            public SID_IDENTIFIER_AUTHORITY IdentifierAuthority;
            public uint[] SubAuthority;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct SID_AND_ATTRIBUTES
        {
            public SID Sid;
            public long Attributes;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct TOKEN_MANDATORY_LABEL
        {
            public SID_AND_ATTRIBUTES Label;
        }
        /*/

        public static void SetTokenIntegrity(int ProcessId, IntegrityLevel Integrity)
        {
            /*/
            try
            {
                SID IntegrityLevelSID = new SID();
                IntegrityLevelSID.Revision = 1;
                IntegrityLevelSID.SubAuthorityCount = 1;
                IntegrityLevelSID.IdentifierAuthority.Value[5] = 16;
                if (Integrity == IntegrityLevel.Untrusted)
                {
                    IntegrityLevelSID.SubAuthority[0] = 0x00000000;
                }
                else if (Integrity == IntegrityLevel.Low)
                {
                    IntegrityLevelSID.SubAuthority[0] = 0x00001000;
                }
                TOKEN_MANDATORY_LABEL TokenIntegrityLevel = new TOKEN_MANDATORY_LABEL();
                TokenIntegrityLevel.Label.Attributes = 0x00000020L;
                TokenIntegrityLevel.Label.Sid = IntegrityLevelSID;
                IntPtr hToken = IntPtr.Zero;
                OpenProcessToken(Process.GetProcessById(ProcessId).Handle, 0x0040 | 0x0020 | 0x0080, ref hToken);
                return SetTokenInformation(hToken, 25, TokenIntegrityLevel, Marshal.SizeOf(typeof(TOKEN_MANDATORY_LABEL)) + GetLengthSid(ref IntegrityLevelSID));
            }
            catch
            {
                return false;
            }
            /*/
            AntiHackingEngineHelper.SetIntegrityLevel(ProcessId, (int)Integrity);
        }

        private struct PROCESS_INFORMATION
        {
            public IntPtr hProcess;
            public IntPtr hThread;
            public uint dwProcessId;
            public uint dwThreadId;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct SECURITY_ATTRIBUTES
        {
            public int nLength;
            public IntPtr lpSecurityDescriptor;
            public int bInheritHandle;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct STARTUPINFO
        {
            public uint cb;
            public string lpReserved;
            public string lpDesktop;
            public string lpTitle;
            public uint dwX;
            public uint dwY;
            public uint dwXSize;
            public uint dwYSize;
            public uint dwXCountChars;
            public uint dwYCountChars;
            public uint dwFillAttribute;
            public uint dwFlags;
            public short wShowWindow;
            public short cbReserved2;
            public IntPtr lpReserved2;
            public IntPtr hStdInput;
            public IntPtr hStdOutput;
            public IntPtr hStdError;
        }

        public static bool CreateProcessWithPrivilegesRemoved(string FileName, string Args)
        {
            STARTUPINFO SInfo = new STARTUPINFO();
            PROCESS_INFORMATION ProcessInfo = new PROCESS_INFORMATION();
            SECURITY_ATTRIBUTES ProcessSecurity = new SECURITY_ATTRIBUTES();
            SECURITY_ATTRIBUTES ThreadSecurity = new SECURITY_ATTRIBUTES();
            ProcessSecurity.nLength = Marshal.SizeOf(ProcessSecurity);
            ThreadSecurity.nLength = Marshal.SizeOf(ThreadSecurity);
            IntPtr hToken = IntPtr.Zero;
            IntPtr RestrictedToken = IntPtr.Zero;
            OpenProcessToken(Process.GetCurrentProcess().Handle, 0x0002 | 0x0001 | 0x0008 | 0x00020000L, out hToken);
            CreateRestrictedToken(hToken, 0x1, 0, IntPtr.Zero, 0, IntPtr.Zero, 0, IntPtr.Zero, out RestrictedToken);
            if (!CreateProcess(FileName, Args, ref ProcessSecurity, ref ThreadSecurity, false, 0x00000004, IntPtr.Zero, null, ref SInfo, out ProcessInfo))
            {
                CloseHandle(RestrictedToken);
                return false;
            }
            PROCESS_ACCESS_TOKEN TokenInfo = new PROCESS_ACCESS_TOKEN();
            TokenInfo.Token = RestrictedToken;
            TokenInfo.Thread = IntPtr.Zero;
            NtSetInformationProcess(ProcessInfo.hProcess, 9, ref TokenInfo, Marshal.SizeOf(TokenInfo));
            ResumeThread(ProcessInfo.hThread);
            CloseHandle(ProcessInfo.hProcess);
            CloseHandle(ProcessInfo.hThread);
            CloseHandle(RestrictedToken);
            return true;
        }
    }
}