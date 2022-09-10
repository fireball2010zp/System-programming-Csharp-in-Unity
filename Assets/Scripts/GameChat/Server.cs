using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using System;

namespace GameChat
{
    public class Server : MonoBehaviour
    {
        private const int MAX_CONNECTION = 10;
        private int port = 57356;
        private int hostID;
        private int reliableChannel;
        private bool isStarted = false;
        private byte error;

        Dictionary<int, UserLogins> users = new Dictionary<int, UserLogins>();

        public void AddUser(int id)
        {
            users[id] = new UserLogins(id);
        }

        internal void RemoveUser(int id)
        {
            users.Remove(id);
        }

        public UserLogins this[int id]
        {
            get => users[id];
        }

        internal IEnumerable<int> AllUserId()
        {
            return users.Keys;
        }

        public void StartServer()
        {
            NetworkTransport.Init();

            ConnectionConfig cc = new ConnectionConfig();
            reliableChannel = cc.AddChannel(QosType.Reliable);

            HostTopology topology = new HostTopology(cc, MAX_CONNECTION);
            hostID = NetworkTransport.AddHost(topology, port);

            isStarted = true;
        }

        public void ShutDownServer()
        {
            if (!isStarted)
                return;

            NetworkTransport.RemoveHost(hostID);
            NetworkTransport.Shutdown();

            isStarted = false;
        }

        void Update()
        {
            if (!isStarted)
                return;

            int recHostId;
            int connectionId; 
            int channelId; 
            byte[] recBuffer = new byte[1024];
            int bufferSize = 1024;
            int dataSize; 

            NetworkEventType recData = NetworkTransport.Receive
                (
                    out recHostId,
                    out connectionId,
                    out channelId,
                    recBuffer,
                    bufferSize,
                    out dataSize,
                    out error
                );

            while (recData != NetworkEventType.Nothing)
            {
                switch (recData)
                {
                    case NetworkEventType.Nothing:
                        break;

                    case NetworkEventType.ConnectEvent:
                        AddUser(connectionId);
                        SendMessageToAll($"{connectionId} has connected.");
                        // Debug.Log($"{connectionId} has connected.");
                        break;

                    case NetworkEventType.DataEvent:
                        string message = Encoding.Unicode.GetString(recBuffer, 0, dataSize);
                        if (users[connectionId].firstMessage)
                        {
                            users[connectionId].Login = message;
                            SendMessageToAll($"{users[connectionId].Login} has connected.");
                            // Debug.Log($"{users[connectionId].Login}: {message}");
                        }
                        else
                        {
                            SendMessageToAll($"{users[connectionId].Login}: {message}");
                            // Debug.Log($"{users[connectionId].Login}: {message}");
                        }
                        break;

                    case NetworkEventType.DisconnectEvent:
                        RemoveUser(connectionId);
                        SendMessageToAll($"{users[connectionId].Login} has disconnected.");
                        // Debug.Log($"{users[connectionId].Login} has disconnected.");
                        break;

                    case NetworkEventType.BroadcastEvent:
                        break;
                }

                recData = NetworkTransport.Receive
                    (
                        out recHostId,
                        out connectionId,
                        out channelId,
                        recBuffer,
                        bufferSize,
                        out dataSize,
                        out error
                    );
            }
        }

        public void SendMessage(string message, int connectionID)
        {
            byte[] buffer = Encoding.Unicode.GetBytes(message);
            NetworkTransport.Send
                (
                    hostID,
                    connectionID,
                    reliableChannel,
                    buffer,
                    message.Length * sizeof(char),
                    out error
                );

            if ((NetworkError)error != NetworkError.Ok)
                Debug.Log((NetworkError)error);
        }

        public void SendMessageToAll(string message)
        {
            foreach (var userId in AllUserId())
            {
                SendMessage(message, userId);
            }
        }
    }

    public class UserLogins
    {
        private string login;
        public bool firstMessage { get; private set; } = true;
        public Guid UserId { get; }

        public string Login
        {
            get { return login; }
            set
            {
                login = value;
                firstMessage = false;
            }
        }

        public UserLogins(int id)
        {
            UserId = new Guid();
        }
    }
}