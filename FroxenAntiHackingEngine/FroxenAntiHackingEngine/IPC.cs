using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Messaging;

namespace FroxenAntiHackingEngine
{
    public class IPC
    {
        public class AnonymousPipeIPC
        {
            public class Child
            {
                public static AnonymousPipeClientStream CreateAnonymousPipe(string PipeHandle, PipeDirection Mode)
                {
                    return new AnonymousPipeClientStream(Mode, PipeHandle);
                }

                public static void SendMessage(AnonymousPipeClientStream Client, string Message)
                {
                    byte[] MessageInBytes = Encoding.Default.GetBytes(Message);
                    Client.Write(MessageInBytes, 0, MessageInBytes.Length);
                }

                public static string ReceiveMessage(AnonymousPipeClientStream Client)
                {
                    if (Client.CanRead)
                    {
                        byte[] Message = new byte[256];
                        Client.Read(Message, 0, 256);
                        return Encoding.Default.GetString(Message);
                    }
                    return null;
                }
            }

            public class Parent
            {
                public static AnonymousPipeServerStream CreateAnonymousPipe(PipeDirection Mode)
                {
                    return new AnonymousPipeServerStream(Mode);
                }

                public static string ReceiveMessage(AnonymousPipeServerStream Server)
                {
                    byte[] MessageInBytes = new byte[256];
                    Server.Read(MessageInBytes, 0, MessageInBytes.Length);
                    return Encoding.Default.GetString(MessageInBytes);
                }

                public static void SendMessage(AnonymousPipeServerStream Server, string Message)
                {
                    byte[] MessageInBytes = Encoding.Default.GetBytes(Message);
                    Server.Write(MessageInBytes, 0, MessageInBytes.Length);
                }
            }
        }
    }
}