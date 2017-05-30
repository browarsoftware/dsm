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
using System.Collections;
using System.Threading;

namespace Server
{
    class TCPServer
    {
        // delegate declaration
        public delegate void NewConnection(object sender, HandleClinet client);

        // event declaration
        public event NewConnection NewClient;

        // delegate declaration
        public delegate void ConnectionBroken(object sender, HandleClinet client);

        // event declaration
        public event ConnectionBroken Disconnection;

        private int port = 0;
        private bool stopServer = false;
        ArrayList allClients = new ArrayList();

        public void RemoveClient(int id)
        {
            if (id < allClients.Count)
            {
                HandleClinet hC = (HandleClinet)allClients[id];
                RemoveClient(hC);
                hC.StopClient();
            }
        }

        public TCPServer(int port)
        {
            this.port = port;
        }

        public void BroadcastMessage(byte[] message, HandleClinet except)
        {
            HandleClinet helpClient = null;
            for (int a = 0; a < allClients.Count; a++)
            {
                helpClient = (HandleClinet)allClients[a];
                if (helpClient != except)
                    try
                    {
                        helpClient.SendData(message);
                    }
                    catch
                    { }
            }
        }

        public void RemoveClient(HandleClinet client)
        {
            Disconnection(this, client);
            allClients.Remove(client);
        }
        TcpListener serverSocket = null;
        public void StartServer()
        {
            serverSocket = new TcpListener(port);
            TcpClient clientSocket = null;
            int counter = 0;

            serverSocket.Start();
            counter = 0;
            int count = 0;
            try
            {
                while (!stopServer)
                {
                    counter += 1;
                    clientSocket = serverSocket.AcceptTcpClient();
                    Console.WriteLine(" >> " + "Client No:" + Convert.ToString(counter) + " started!");
                    HandleClinet client = new HandleClinet(count);
                    count++;
                    allClients.Add(client);
                    client.startClient(clientSocket, Convert.ToString(counter), this);
                    NewClient(this, client);
                }
            }
            catch
            {
            }
        }
        public void StopServer()
        {
            stopServer = true;
            HandleClinet helpClient;
            for (int a = 0; a < allClients.Count; a++)
            {
                helpClient = (HandleClinet)allClients[a];
                helpClient.StopClient();
            }
            serverSocket.Stop();
        }
    }
}
