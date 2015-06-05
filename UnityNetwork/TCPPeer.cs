using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UnityNetwork
{
    class TCPPeer
    {
        public bool isServer { get; set; }

        public Socket socket;

        NetworkManager networkMgr;

        public TCPPeer(NetworkManager netMgr)
        {
            networkMgr = netMgr;
        }

        private void AddInternalPacket(string msg, Socket sk)
        {
            NetBitStream packet = new NetBitStream();
            packet.SocketObj = sk;
            packet.BeginWrite(msg);
            networkMgr.AddPacket(packet);
        }

        public void Listen(string ip, int port)
        {
            isServer = true;

            IPEndPoint ipe = new IPEndPoint(IPAddress.Parse(ip), port);

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                socket.Bind(ipe);

                socket.Listen(10);

                socket.BeginAccept(new AsyncCallback(ListenCallBack), socket);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public void ListenCallBack(IAsyncResult ar)
        {
            Socket server = (Socket)ar.AsyncState;

            try
            {
                Socket client = server.EndAccept(ar);

                AddInternalPacket("OnAccepted", client);

                NetBitStream packet = new NetBitStream();

                packet.SocketObj = client;

                client.BeginReceive(packet.Bytes, 0, NetBitStream.HeaderLength, SocketFlags.None, new AsyncCallback(ReceiveHeader), packet);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            server.BeginAccept(new AsyncCallback(ListenCallBack), server);
        }

        public void Connect(string ip, int port)
        {
            isServer = false;

            IPEndPoint ipe = new IPEndPoint(IPAddress.Parse(ip), port);
            try
            {
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                socket.BeginConnect(ipe, new AsyncCallback(ConnectionCallBack), socket);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void ConnectionCallBack(IAsyncResult ar)
        {
            Socket client = (Socket)ar.AsyncState;

            try
            {
                client.EndConnect(ar);

                AddInternalPacket("OnConnected", client);

                NetBitStream packet = new NetBitStream();

                packet.SocketObj = client;

                client.BeginReceive(packet.Bytes, 0, NetBitStream.HeaderLength, SocketFlags.None, new AsyncCallback(ReceiveHeader), packet);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                AddInternalPacket("OnConnectFail", client);
            }
        }

        public void Send(Socket socket, NetBitStream packet)
        {
            NetworkStream nStream = new NetworkStream(socket);

            if (nStream.CanWrite)
            {
                nStream.BeginWrite(packet.Bytes, 0, packet.Length, new AsyncCallback(SendCallBack), nStream);
            }
        }

        public void SendCallBack(IAsyncResult ar)
        {
            NetworkStream nStream = (NetworkStream)ar.AsyncState;

            nStream.EndWrite(ar);

            nStream.Flush();

            nStream.Close();
        }

        public void ReceiveHeader(IAsyncResult ar)
        {
            NetBitStream packet = (NetBitStream)ar.AsyncState;

            int readNum = packet.SocketObj.EndReceive(ar);


            if (readNum < 1)
            {
                AddInternalPacket("OnLost", packet.SocketObj);

                return;
            }

            packet.ReadLength += readNum;

            if (packet.ReadLength < NetBitStream.HeaderLength)
            {
                packet.SocketObj.BeginReceive(packet.Bytes, packet.ReadLength, NetBitStream.HeaderLength - packet.ReadLength, SocketFlags.None, new AsyncCallback(ReceiveHeader), packet);
            }
            else
            {
                packet.DecodeHeader();

                packet.ReadLength = 0;

                packet.SocketObj.BeginReceive(packet.Bytes, 0, packet.BodyLength, SocketFlags.None, new AsyncCallback(ReceiveBody), packet);
            }
        }

        public void ReceiveBody(IAsyncResult ar)
        {
            NetBitStream packet = (NetBitStream)ar.AsyncState;

            int readNum = packet.SocketObj.EndReceive(ar);


            if (readNum < 1)
            {
                AddInternalPacket("OnLost", packet.SocketObj);

                return;
            }

            packet.ReadLength += readNum;

            if (packet.ReadLength < packet.BodyLength)
            {
                packet.SocketObj.BeginReceive(packet.Bytes, NetBitStream.HeaderLength + packet.ReadLength, packet.BodyLength - packet.ReadLength, SocketFlags.None, new AsyncCallback(ReceiveBody), packet);
            }
            else
            {
                networkMgr.AddPacket(packet);

                packet.Reset();

                packet.SocketObj.BeginReceive(packet.Bytes, 0, NetBitStream.HeaderLength, SocketFlags.None, new AsyncCallback(ReceiveHeader), packet);
            }
        }
    }
}
