using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using UnityEngine.SceneManagement;

using System.Text;
using WebSockets;

public class SimpleServerDemo : MonoBehaviour
{

    #region Singleton
    public static SimpleServerDemo instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Debug.LogWarning("Server instance created");
        }
        else
        {
            Destroy(this.gameObject);
            Debug.LogError("Server instance already exists! Destroying this one.");
        }
    }
    #endregion

    /// <summary>
    /// A list of active connections stored in a tuple with the connection, the client's id, and the character id in that order
    /// This list is publically accessible to other scripts
    /// </summary>
    public List<Tuple<WebSocketConnection, int, int>> clientInfoList;
    public List<int> readyClients;
    int currentId = 0;
    WebsocketListener listener;

    private bool attackHeld = false;
    private bool jumpHeld = false;
    
    #region events
    public delegate void MessageReceived(string message);
    public static MessageReceived OnMessageReceived;

    public delegate void ServerBroadcastMessage(string message);
    public static ServerBroadcastMessage SendMessageToAll;

    public delegate void ServerBroadcastMessageToClient(string message, int clientId);
    public static ServerBroadcastMessageToClient SendMessageToClient;

    public delegate void CharacterSelected(int clientId, int characterId);
    public static CharacterSelected OnCharacterSelected;
    public delegate void CharacterDeselected(int clientId, int characterId);
    public static CharacterDeselected OnCharacterDeselected;

    public delegate void ChangeServerState(ServerState newState);
    public static ChangeServerState UpdateServerState;
    #endregion
    public enum ServerState { MainMenu, CharacterSelect, Game, EndGame }
    [SerializeField] private ServerState serverState;
    public int MaxPlayerCount = 4;

    void Start()
    {
        // serverState = ServerState.MainMenu;
        // Create a server that listens for connection requests:
        listener = new WebsocketListener();
        listener.Start();

        // Create a list of active connections:
        // clients = new List<WebSocketConnection>();
        clientInfoList = new List<Tuple<WebSocketConnection, int, int>>();
        readyClients = new List<int>();

        //subscribe to events
        SendMessageToClient += SendToClient;
        UpdateServerState += ChangeSelectedState;
        SendMessageToAll += Broadcast;

        DontDestroyOnLoad(this.gameObject);

    }

    void Update()
    {
        // Check for new connections:
        ProcessNewClients();

        // Process current connections (this may lead to a callback to OnPacketReceive):
        ProcessCurrentClients();

        if (clientInfoList.Count < 1)
        {
            KeyboardTesterCode();
        }
    }

    void KeyboardTesterCode()
    {
        if (Input.GetKeyDown(KeyCode.F)) InputEvents.JumpButtonPressed?.Invoke(this, 0);
        if (Input.GetKeyDown(KeyCode.G)) InputEvents.AttackButtonPressed?.Invoke(this, 0);
        Vector2 input = Vector2.zero;
        string direction = "Neutral";
        if (Input.GetKey(KeyCode.D)) { input.x += 1; direction = "Right"; }
        if (Input.GetKey(KeyCode.A)) { input.x -= 1; direction = "Left"; }
        if (Input.GetKey(KeyCode.W)) { input.y += 1; direction = "Up"; }
        if (Input.GetKey(KeyCode.S)) { input.y -= 1; direction = "Down"; }
        InputEvents.JoystickMoved?.Invoke(this, new DirectionalEventArgs(0, input, direction));

        if (Input.GetKeyDown(KeyCode.M)) InputEvents.JumpButtonPressed?.Invoke(this, 1);
        if (Input.GetKeyDown(KeyCode.N)) InputEvents.AttackButtonPressed?.Invoke(this, 1);
        input = Vector2.zero;
        direction = "Neutral";
        if (Input.GetKey(KeyCode.RightArrow)) { input.x += 1; direction = "Right"; }
        if (Input.GetKey(KeyCode.LeftArrow)) { input.x -= 1; direction = "Left"; }
        if (Input.GetKey(KeyCode.UpArrow)) { input.y += 1; direction = "Up"; }
        if (Input.GetKey(KeyCode.DownArrow)) { input.y -= 1; direction = "Down"; }
        InputEvents.JoystickMoved?.Invoke(this, new DirectionalEventArgs(1, input, direction));
    }

    void ChangeSelectedState(ServerState newState)
    {
        //switch to the correct scene
        if (serverState == ServerState.MainMenu && clientInfoList.Count == 0)
        {
            Debug.LogWarning("No clients connected! Skipping character selection");
            serverState = ServerState.Game;
        }
        else serverState = newState;
        switch (serverState)
        {
            case ServerState.MainMenu:
                SceneManager.LoadScene("MainMenu");
                Broadcast("state MainMenu");
                Debug.LogWarning("Server state changed to MainMenu");
                Debug.LogWarning(clientInfoList.Count);
                break;
            case ServerState.CharacterSelect:
                SceneManager.LoadScene("CharacterSelect");
                Broadcast("state CharacterSelect");
                Debug.LogWarning("Server state changed to CharacterSelect");
                break;
            case ServerState.Game:
                SceneManager.LoadScene("DemoScene");
                Broadcast("state Game");
                Debug.LogWarning("Server state changed to Game");
                break;
            case ServerState.EndGame:
                SceneManager.LoadScene("EndGame");
                Broadcast("state EndGame");
                Debug.LogWarning("Server state changed to EndGame");
                break;
            default:
                break;
        }
    }
    void ProcessNewClients()
    {
        listener.Update();
        while (listener.Pending())
        {
            if (serverState != ServerState.MainMenu || clientInfoList.Count >= MaxPlayerCount)
            {
                try
                {
                    Debug.LogWarning("Game has started!");
                    WebSocketConnection tempWS = listener.AcceptConnection(OnPacketReceive);
                    if(clientInfoList.Count >= MaxPlayerCount) tempWS.Send(new NetworkPacket(Encoding.UTF8.GetBytes("Game is full!")));
                    else tempWS.Send(new NetworkPacket(Encoding.UTF8.GetBytes("Game has started!")));
                }
                catch (System.Exception)
                {
                    //Debug.LogError(e);
                    continue;
                }
            }

            WebSocketConnection ws = listener.AcceptConnection(OnPacketReceive);
            clientInfoList.Add(new Tuple<WebSocketConnection, int, int>(ws, currentId, 0));
            InputEvents.ClientConnected?.Invoke(this, currentId);
            currentId++;
        }
    }

    void ProcessCurrentClients()
    {
        for (int i = 0; i < clientInfoList.Count; i++)
        {
            if (clientInfoList[i].Item1.Status == ConnectionStatus.Connected)
            {
                clientInfoList[i].Item1.Update();
            }
            else
            {
                clientInfoList.RemoveAt(i);
                InputEvents.ClientDisconnected?.Invoke(this, clientInfoList[i].Item2);
                Console.WriteLine("Removing disconnected client. #active clients: {0}", clientInfoList.Count);
                i--;
            }
        }
    }

    #region irrelevant functions
    /// <summary>
    /// This method is called by WebSocketConnections when their Update method is called and a packet comes in.
    /// From here you can implement your own server functionality 
    ///   (parse the (string) package data, and depending on contents, call other methods, implement game play rules, etc). 
    /// Currently it prints the received packet to the console and invokes the correct event.
    /// </summary>
    void OnPacketReceive(NetworkPacket packet, WebSocketConnection connection)
    {
        string text = Encoding.UTF8.GetString(packet.Data);
        Tuple<WebSocketConnection, int, int> clientInfo = clientInfoList.Find(x => x.Item1 == connection);
        Console.WriteLine("Received a packet: {0} from client ID {1}", text, clientInfo.Item2);
        InvokeInputEvent(text, clientInfo.Item2);
    }

    void Broadcast(string message)
    {
        Broadcast(new NetworkPacket(Encoding.UTF8.GetBytes(message)));
    }
    void Broadcast(NetworkPacket packet)
    {
        foreach (var cl in clientInfoList)
        {
            cl.Item1.Send(packet);
        }
    }

    void SendToClient(NetworkPacket packet, int id)
    {
        if (clientInfoList.Count < 1) return;
        clientInfoList.Find(x => x.Item2 == id).Item1.Send(packet);
    }
    void SendToClient(string message, int clientId)
    {
        //Debug.LogError("Sending message to client: " + message + " to client: " + clientId);
        SendToClient(new NetworkPacket(Encoding.UTF8.GetBytes(message)), clientId);
    }

    void SendToClient(NetworkPacket packet, WebSocketConnection client)
    {
        client.Send(packet);
    }
    #endregion
    //invoke the appropriate input events
    void InvokeInputEvent(string input, int id)
    {
        string[] splitInput = input.Split(' ');

        switch (serverState)
        {
            case ServerState.CharacterSelect:
                CharacterSelectionInputs(splitInput, id);
                break;
            case ServerState.Game:
                GameplayInputs(splitInput, id);
                break;
            case ServerState.MainMenu:
                MainMenuInputs(splitInput, id);
                break;
            default:
                break;
        }
    }

    void MainMenuInputs(string[] input, int id)
    {
        switch (input[0].ToLower())
        {
            case "play":
                UpdateServerState?.Invoke(ServerState.CharacterSelect);
                break;
            default:
                break;
        }
    }
    void CharacterSelectionInputs(string[] input, int id)
    {
        int character = int.Parse(input[0]);
        // int ready = int.Parse(input[1]);
        if (character > 0)
        {
            OnCharacterSelected?.Invoke(id, character);
            if (!readyClients.Contains(id)) readyClients.Add(id);
        }
        else if (character < 1)
        {
            OnCharacterDeselected?.Invoke(id, character);
            if (readyClients.Contains(id)) readyClients.Remove(id);
        }
        Tuple<WebSocketConnection, int, int> foundClient = clientInfoList.Find(x => x.Item2 == id);
        Tuple<WebSocketConnection, int, int> replacementClient = new Tuple<WebSocketConnection, int, int>(foundClient.Item1, foundClient.Item2, character);
        clientInfoList[clientInfoList.IndexOf(foundClient)] = replacementClient;
        Debug.LogWarning("Character selected: " + replacementClient.Item3);
        if (readyClients.Count == clientInfoList.Count)
        {
            ChangeSelectedState(ServerState.Game);
        }

        
    }
    void GameplayInputs(string[] input, int id)
    {
        float x = float.Parse(input[0]);
        float y = float.Parse(input[1]);
        int JumpButtonPressed = int.Parse(input[2]);
        int AttackButtonPressed = int.Parse(input[3]);
        string joystickDirection = input[4];
        if (JumpButtonPressed == 1 && !jumpHeld)
        {
            InputEvents.JumpButtonPressed?.Invoke(this, id);
        }
        if (AttackButtonPressed == 1 && !attackHeld)
        {
            InputEvents.AttackButtonPressed?.Invoke(this, id);
        }
        InputEvents.JoystickMoved?.Invoke(this, new DirectionalEventArgs(id, new Vector2(x, y), joystickDirection));

        jumpHeld = JumpButtonPressed == 0 ? false : true;
        attackHeld = AttackButtonPressed == 0 ? false : true;
    }
}
