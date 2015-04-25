using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UnityNetwork
{
    class NetworkManager
    {
        Thread MyThread;

        public delegate void OnReceive(NetBitStream packet);

        public Dictionary<string, OnReceive> Handlers;

        private Queue<NetBitStream> Packets = new Queue<NetBitStream>();

        public NetworkManager()
        {
            Handlers = new Dictionary<string, OnReceive>();

            AddHnadler("OnAccepted", OnAccepted);

            AddHnadler("OnConnected", OnConnected);

            AddHnadler("OnConnectFailed", OnConnectFailed);

            AddHnadler("OnLost", OnLost); //利用Dic Delegate 构建方法字典。
        }

        private void AddHnadler(string msgid, OnReceive handler)
        {
            Handlers.Add(msgid, handler);
        }

        public virtual void OnAccepted(NetBitStream packet) { }
   
        public virtual void OnConnected(NetBitStream packet) { }

        public virtual void OnConnectFailed(NetBitStream packet){ }

        public virtual void OnLost(NetBitStream packet) { }

        /************************************************/
       
        public void AddPacket(NetBitStream packet)
        {
            lock (Packets)
            {
                Packets.Enqueue(packet);
            }
        }

        public NetBitStream GetPacket()
        {
            lock (Packets)
            {
                if (Packets.Count == 0) return null;

                return Packets.Dequeue();
            }
        }

        /************************************************/

        
        public void StartThreadUpdate()
        {
            MyThread = new Thread(new ThreadStart(ThreadUpdate)); MyThread.Start();
        }

        public void ThreadUpdate()
        {
            while (true)
            {
                Thread.Sleep(30);

                Update();
            }
        }

        public void Update()
        {
            NetBitStream packet = null;

            for (packet = GetPacket(); packet != null; )
            {
                string msg = "";

                packet.BeginRead(out msg);

                OnReceive handler = null;

                if(Handlers.TryGetValue(msg,out handler))
                {
                    if(handler != null) handler(packet);
                }
            }
        }

        /************************************************/
    }
}
