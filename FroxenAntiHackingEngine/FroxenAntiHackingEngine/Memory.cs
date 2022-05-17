using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace FroxenAntiHackingEngine
{
    public class Memory
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct UNICODE_STRING
        {
            public ushort Length;
            public ushort MaximumLength;
            public IntPtr Buffer;
        }

        [DllImport("ntdll.dll", CharSet = CharSet.Unicode)]
        private static extern void RtlInitUnicodeString(out UNICODE_STRING DestinationString, string SourceString);

        [DllImport("ntdll.dll")]
        private static extern void RtlZeroMemory(IntPtr Address, uint Size);

        public static void ClearStringFromMemory(string String)
        {
            UNICODE_STRING UnicodeString;
            RtlInitUnicodeString(out UnicodeString, String);
            RtlZeroMemory(UnicodeString.Buffer, (uint)Marshal.SizeOf(UnicodeString.Buffer));
            GC.Collect();
        }
    }
}