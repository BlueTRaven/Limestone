using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using Microsoft.Xna.Framework;

namespace Limestone
{
    public class ClientSocket
    { 
        System.Net.Sockets.TcpClient clientSocket = new System.Net.Sockets.TcpClient();
        NetworkStream serverStream;
        public void ConnectToServer()
        {
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Loopback, 26155);

            clientSocket.Connect(remoteEP);
        }

        public void SendData(string dataTosend)
        {
            if (string.IsNullOrEmpty(dataTosend))
                return;
            NetworkStream serverStream = clientSocket.GetStream();
            byte[] outStream = System.Text.Encoding.ASCII.GetBytes(dataTosend);
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();
        }

        public void CloseConnection()
        {
            clientSocket.Close();
        }

        public void ReceiveData()
        {
            StringBuilder message = new StringBuilder();
            NetworkStream serverStream = clientSocket.GetStream();
            serverStream.ReadTimeout = 100;
            //the loop should continue until no dataavailable to read and message string is filled.
            //if data is not available and message is empty then the loop should continue, until
            //data is available and message is filled.
            while (true)
            {
                if (serverStream.DataAvailable)
                {
                    int read = serverStream.ReadByte();
                    if (read > 0)
                        message.Append((char)read);
                    else
                        break;
                }
                else if (message.ToString().Length > 0)
                    break;
            }
            Console.WriteLine(message);
        }
    }
}
