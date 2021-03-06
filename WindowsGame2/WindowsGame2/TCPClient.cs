/*
 * Copyright (c) 2013 Tomasz Hachaj
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
 * The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Threading;

namespace Series3D2
{
    public class TCPClient
    {
        // delegate declaration
        public delegate void NewDataRecived(object sender, byte[] data);

        // event declaration
        public event NewDataRecived NewData;

        bool stopClient = false;
        String ip = "127.0.0.1";
        int port = 0;
        TcpClient clientSocket = null;
        public TCPClient(String ip, int port)
        {
            this.ip = ip;
            this.port = port;
        }
        public void StartConnection()
        {
            clientSocket = new System.Net.Sockets.TcpClient();
            clientSocket.Connect(ip, port);
            Thread t = new Thread(ReceiveData);
            t.Start();
        }
        public void StopConnection()
        {
            stopClient = true;
            clientSocket.Close();
        }
        public void ReceiveData()
        {
            try
            {
                while (!stopClient)
                {
                    while (clientSocket.Available > 0)
                    {
                        NetworkStream serverStream = clientSocket.GetStream();
                        byte[] inStream = new byte[85];
                        serverStream.Read(inStream, 0, 85);
                        NewData(this, inStream);
                    }
                    Thread.Sleep(1);
                }
            }
            catch { }
        }

        public void SendData(byte[] data)
        {
            NetworkStream serverStream = clientSocket.GetStream();
            serverStream.Write(data, 0, data.Length);
            serverStream.Flush();
        }
    }
}
