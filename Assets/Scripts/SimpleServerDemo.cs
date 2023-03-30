using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using System.Text;
using WebSockets;

public class SimpleServerDemo : MonoBehaviour
{
    List<WebSocketConnection> clients;
    Dictionary<WebSocketConnection, int> clientNames = new Dictionary<WebSocketConnection, int>();
    int currentId = 0;
    WebsocketListener listener;
    [SerializeField] FighterManager fighterManager;


    public delegate void MessageReceived(string message);
    public static MessageReceived OnMessageReceived;

    void Start()
    {
        // Create a server that listens for connection requests:
        listener = new WebsocketListener();
        listener.Start();

        // Create a list of active connections:
        clients = new List<WebSocketConnection>();

    }

    void Update()
    {
        // Check for new connections:
        listener.Update();
        while (listener.Pending())
        {
            WebSocketConnection ws = listener.AcceptConnection(OnPacketReceive);
            clients.Add(ws);
            clientNames.Add(ws, currentId);
            InputEvents.ClientConnected?.Invoke(this, clientNames[ws]);
            currentId++;
            Console.WriteLine("A client connected from " + ws.RemoteEndPoint.Address + " with ID: " + clientNames[ws]);
        }

        // Process current connections (this may lead to a callback to OnPacketReceive):
        for (int i = 0; i < clients.Count; i++)
        {
            if (clients[i].Status == ConnectionStatus.Connected)
            {
                clients[i].Update();
            }
            else
            {
                clients.RemoveAt(i);
                clientNames.Remove(clients[i]);
                InputEvents.ClientDisconnected?.Invoke(this, clientNames[clients[i]]);
                Console.WriteLine("Removing disconnected client. #active clients: {0}", clients.Count);
                i--;
            }
        }

        //keyboard tester code
        if (Input.GetKeyDown(KeyCode.N)) InputEvents.JumpButtonPressed?.Invoke(this, 0);
        if (Input.GetKeyDown(KeyCode.M)) InputEvents.AttackButtonPressed?.Invoke(this, 0);
        Vector2 input = Vector2.zero;
        if (Input.GetKey(KeyCode.D)) input.x += 1;
        if (Input.GetKey(KeyCode.A)) input.x -= 1;
        if (Input.GetKey(KeyCode.W)) input.y += 1;
        if (Input.GetKey(KeyCode.S)) input.y -= 1;
        InputEvents.JoystickMoved?.Invoke(this, new DirectionalEventArgs(0, input));


    }

    /// <summary>
    /// This method is called by WebSocketConnections when their Update method is called and a packet comes in.
    /// From here you can implement your own server functionality 
    ///   (parse the (string) package data, and depending on contents, call other methods, implement game play rules, etc). 
    /// Currently it only does some very simple string processing, and echoes and broadcasts a message.
    /// </summary>
    void OnPacketReceive(NetworkPacket packet, WebSocketConnection connection)
    {
        string text = Encoding.UTF8.GetString(packet.Data);
        Console.WriteLine("Received a packet: {0} from client ID {1}", text, clientNames[connection]);
        // OnMessageReceived?.Invoke(text);
        InvokeEvent(text, clientNames[connection]);

        // byte[] bytes;

        // //// echo:
        // string response = "You said: " + text;
        // bytes = Encoding.UTF8.GetBytes(response);
        // connection.Send(new NetworkPacket(bytes));

        // //// broadcast:
        // string message = connection.RemoteEndPoint.ToString() + " says: " + text;
        // bytes = Encoding.UTF8.GetBytes(message);
        // Broadcast(new NetworkPacket(bytes));
    }

    void Broadcast(NetworkPacket packet)
    {
        foreach (var cl in clients)
        {
            cl.Send(packet);
        }
    }

    //invoke the appropriate events
    void InvokeEvent(string input, int id)
    {
        string[] splitInput = input.Split(' ');
        float x = float.Parse(splitInput[0]);
        float y = float.Parse(splitInput[1]);
        int JumpButtonPressed = int.Parse(splitInput[2]);
        int AttackButtonPressed = int.Parse(splitInput[3]);
        if (JumpButtonPressed == 1)
        {
            InputEvents.JumpButtonPressed?.Invoke(this, id);
        }
        if (AttackButtonPressed == 1)
        {
            InputEvents.AttackButtonPressed?.Invoke(this, id);
        }
        InputEvents.JoystickMoved?.Invoke(this, new DirectionalEventArgs(id, new Vector2(x, y)));
    }
}
