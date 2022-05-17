using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace FroxenAntiHackingEngine
{
    public class JobObjects
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr CreateJobObjectA(IntPtr lpJobAttributes, string lpName);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool AssignProcessToJobObject(IntPtr hObject, IntPtr hProcess);

        public static IntPtr JobObject(string JobName)
        {
            return CreateJobObjectA(IntPtr.Zero, JobName);
        }

        public class JobObjectUIRestrictionsClass
        {
            [DllImport("kernel32.dll", SetLastError = true)]
            private static extern bool SetInformationJobObject(IntPtr hJobObject, uint JobObjectInformationClass, ref JOBOBJECT_BASIC_UI_RESTRICTIONS JobObjectInformation, int JobObjectInformationLength);

            [StructLayout(LayoutKind.Sequential)]
            private struct JOBOBJECT_BASIC_UI_RESTRICTIONS
            {
                public uint UIRestrictionsClass;
            }

            public enum UIRestrictions
            {
                PreventReadingClipboard = 0x00000002,
                PreventWritingClipboard = 0x00000004,
                PreventAccessingDesktop = 0x00000040,
                PreventShuttingDownSystem = 0x00000080,
                PreventChangingDisplaySettings = 0x00000010,
                PreventAccessingSystemParameters = 0x00000008,
                PreventAccessingGlobalAtoms = 0x00000020,
                LimitHandlesToProcessesInJob = 0x00000001,
            };

            public static bool SetUIRestrictionOnJobObject(IntPtr JobObject, UIRestrictions Restrictions)
            {
                JOBOBJECT_BASIC_UI_RESTRICTIONS UIRestriction = new JOBOBJECT_BASIC_UI_RESTRICTIONS();
                UIRestriction.UIRestrictionsClass = (uint)Restrictions;
                return SetInformationJobObject(JobObject, 4, ref UIRestriction, Marshal.SizeOf(typeof(JOBOBJECT_BASIC_UI_RESTRICTIONS)));
            }
        }

        public class JobObjectExtendedLimitedRestrictionsClass
        {
            [DllImport("kernel32.dll", SetLastError = true)]
            private static extern bool SetInformationJobObject(IntPtr hJobObject, uint JobObjectInformationClass, ref JOBOBJECT_EXTENDED_LIMIT_INFORMATION JobObjectInformation, int JobObjectInformationLength);

            [StructLayout(LayoutKind.Sequential)]
            private struct JOBOBJECT_BASIC_LIMIT_INFORMATION
            {
                public Int64 PerProcessUserTimeLimit;
                public Int64 PerJobUserTimeLimit;
                public UInt32 LimitFlags;
                public UIntPtr MinimumWorkingSetSize;
                public UIntPtr MaximumWorkingSetSize;
                public UInt32 ActiveProcessLimit;
                public UIntPtr Affinity;
                public UInt32 PriorityClass;
                public UInt32 SchedulingClass;
            }

            [StructLayout(LayoutKind.Sequential)]
            private struct IO_COUNTERS
            {
                public UInt64 ReadOperationCount;
                public UInt64 WriteOperationCount;
                public UInt64 OtherOperationCount;
                public UInt64 ReadTransferCount;
                public UInt64 WriteTransferCount;
                public UInt64 OtherTransferCount;
            }

            [StructLayout(LayoutKind.Sequential)]
            private struct JOBOBJECT_EXTENDED_LIMIT_INFORMATION
            {
                public JOBOBJECT_BASIC_LIMIT_INFORMATION BasicLimitInformation;
                public IO_COUNTERS IoInfo;
                public UIntPtr ProcessMemoryLimit;
                public UIntPtr JobMemoryLimit;
                public UIntPtr PeakProcessMemoryUsed;
                public UIntPtr PeakJobMemoryUsed;
            }

            public enum ExtendedLimitedRestrictions
            {
                DieOnUnhandledException = 0x00000400,
                ExitOnJobClose = 0x00002000,
                LimitBreakaway = 0x00000800,
                NoChildCreation = 0x00000008,
            };

            public static bool SetExtendedRestrictionOnJobObject(IntPtr JobObject, ExtendedLimitedRestrictions Restrictions)
            {
                JOBOBJECT_EXTENDED_LIMIT_INFORMATION ExtendedRestrictions = new JOBOBJECT_EXTENDED_LIMIT_INFORMATION();
                ExtendedRestrictions.BasicLimitInformation.LimitFlags = (uint)Restrictions;
                if (Restrictions.HasFlag(ExtendedLimitedRestrictions.NoChildCreation))
                    ExtendedRestrictions.BasicLimitInformation.ActiveProcessLimit = 1;
                return SetInformationJobObject(JobObject, 9, ref ExtendedRestrictions, Marshal.SizeOf(typeof(JOBOBJECT_EXTENDED_LIMIT_INFORMATION)));
            }
        }

        public class JobObjectCPURateControl
        {
            [DllImport("kernel32.dll", SetLastError = true)]
            private static extern bool SetInformationJobObject(IntPtr hJobObject, uint JobObjectInformationClass, ref JOBOBJECT_CPU_RATE_CONTROL_INFORMATION JobObjectInformation, int JobObjectInformationLength);

            [StructLayout(LayoutKind.Explicit)]
            public struct JOBOBJECT_CPU_RATE_CONTROL_INFORMATION
            {
                [FieldOffset(0)]
                public UInt32 ControlFlags;
                [FieldOffset(4)]
                public UInt32 MinRate;
                [FieldOffset(4)]
                public UInt32 MaxRate;
                [FieldOffset(4)]
                public UInt32 Weight;
            }

            public static bool SetCPURateControlOnJobObject(IntPtr JobObject, int MaximumCPUUsageAllowed)
            {
                JOBOBJECT_CPU_RATE_CONTROL_INFORMATION CPU_RATE_CONTROL = new JOBOBJECT_CPU_RATE_CONTROL_INFORMATION();
                CPU_RATE_CONTROL.ControlFlags = 0x00000001 | 0x00000004;
                CPU_RATE_CONTROL.MinRate = 0;
                CPU_RATE_CONTROL.MaxRate = (uint)MaximumCPUUsageAllowed;
                return SetInformationJobObject(JobObject, 15, ref CPU_RATE_CONTROL, Marshal.SizeOf(typeof(JOBOBJECT_CPU_RATE_CONTROL_INFORMATION)));
            }
        }

        public static bool AssignProcessToJob(IntPtr JobObject, int ProcessId)
        {
            return AssignProcessToJobObject(JobObject, Process.GetProcessById(ProcessId).Handle);
        }
    }
}