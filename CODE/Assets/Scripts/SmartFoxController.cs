using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Requests;
using Sfs2X.Util;
using UnityEngine;

public class SmartFoxController
{
    private static SmartFoxController _instance = null;
    public static SmartFoxController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new SmartFoxController();
            }
            return _instance;
        }
    }

    private SmartFox _smartFoxConnection = null;
    public SmartFox SmartFox
    {
        get
        {
            if (_smartFoxConnection == null)
            {
                _smartFoxConnection = new SmartFox();
                _smartFoxConnection.ThreadSafeMode = true;
            }
            return _smartFoxConnection;
        }
    }

    private bool _isInitialized = false;

    const string IP = "127.0.0.1";
    const int PORT = 9933;
    const string zone = "BasicExamples";

    private string name = "";

    private SmartFoxController()
    {
    }

    public delegate void LoginSucces(bool success);
    public static LoginSucces OnLoginSuccess = null;

    public delegate void SendMessagePublic(string nameUser, string contentSend);
    public static SendMessagePublic OnSendMessagePublic = null;

    public delegate void RoomAdd(int idRoom, string nameRoom);
    public static RoomAdd OnRoomAdded = null;

    public delegate void RoomRemove(int idRoom);
    public static RoomRemove OnRoomRemoved = null;

    public void LoginRequest(string userName)
    {
        name = userName;
        Initialize();
    }

    public void PublicMessageRequest(string contentSend)
    {
        SmartFox.Send(new PublicMessageRequest(contentSend));
    }

    public void CreateRoomRequest(string roomName = "")
    {
        SmartFox.Send(new CreateRoomRequest(GetRoomSettings(roomName)));
    }

    public void JoinRoomRequest(int idRoom)
    {
        Debug.Log("Press join room: " + idRoom);
        SmartFox.Send(new JoinRoomRequest(idRoom));
    }

    public void Initialize()
    {
        if (_isInitialized)
        {
            //Debug.Log("Initialized");
        }
        else
        {
            Debug.Log("Initialized " + SmartFox.Version);

            SmartFox.AddEventListener(SFSEvent.CONNECTION, OnConnection);
            SmartFox.AddEventListener(SFSEvent.CONNECTION_LOST, OnConnectionLost);
            SmartFox.AddEventListener(SFSEvent.LOGIN, OnLogin);
            SmartFox.AddEventListener(SFSEvent.LOGIN_ERROR, OnLoginError);
            SmartFox.AddEventListener(SFSEvent.PUBLIC_MESSAGE, OnPublicMessage);
            SmartFox.AddEventListener(SFSEvent.ROOM_ADD, OnRoomAdd);
            SmartFox.AddEventListener(SFSEvent.ROOM_JOIN, OnRoomJoin);
            SmartFox.AddEventListener(SFSEvent.ROOM_JOIN_ERROR, OnRoomJoinError);
            SmartFox.AddEventListener(SFSEvent.ROOM_REMOVE, OnRoomRemove);

            SmartFox.Connect(IP, PORT);

        }
        _isInitialized = true;
    }


    private void OnConnection(BaseEvent evt)
    {
        if ((bool)evt.Params["success"])
        {
            Debug.Log("Connection sucessfully");
            SmartFox.Send(new LoginRequest(name, "", zone));
        }
        else
        {
            Debug.Log("Connection failed");
        }
    }

    private void OnConnectionLost(BaseEvent evt)
    {
        string reason = (string)evt.Params["reason"];

        if (reason != ClientDisconnectionReason.MANUAL)
        {
            // Show error message
            Debug.Log("Connection was lost; reason is: " + reason);
            _Reset();
        }
    }

    private void OnLogin(BaseEvent evt)
    {
        OnLoginSuccess(true);
        User user = (User)(evt.Params["user"]);

        name = user.Name;

        if (SmartFox.RoomList.Count > 0)
        {
            SmartFox.Send(new JoinRoomRequest(SmartFox.RoomList[0]));
        }
    }

    private void OnLoginError(BaseEvent evt)
    {
        OnLoginSuccess(false);
    }

    private void OnPublicMessage(BaseEvent evt)
    {
        User user = (User)(evt.Params["sender"]);
        string contentMessage = (string)(evt.Params["message"]);
        OnSendMessagePublic(user.Name, contentMessage);
    }

    private void OnRoomAdd(BaseEvent evt)
    {
        Room room = (Room)(evt.Params["room"]);
        OnRoomAdded(room.Id, room.Name);
    }

    private void OnRoomJoin(BaseEvent evt)
    {

    }

    private void OnRoomJoinError(BaseEvent evt)
    {

    }

    private void OnRoomRemove(BaseEvent evt)
    {
        Room room = (Room)(evt.Params["room"]);
        OnRoomRemoved(room.Id);
    }

    public RoomSettings GetRoomSettings(string roomName)
    {
        if ("" == roomName)
        {
            roomName = name + "'s room";
        }
        RoomSettings room = new RoomSettings(roomName);
        room.IsGame = true;
        room.GroupId = "games";
        room.MaxUsers = 2;
        room.Extension = new RoomExtension("TicTacToeExtension", "TicTacToe.TicTacToeExtension");
        return room;
    }

    public void _Reset()
    {
        _isInitialized = false;

        SmartFox.RemoveEventListener(SFSEvent.CONNECTION, OnConnection);
        SmartFox.RemoveEventListener(SFSEvent.CONNECTION_LOST, OnConnectionLost);
        SmartFox.RemoveEventListener(SFSEvent.LOGIN, OnLogin);
        SmartFox.RemoveEventListener(SFSEvent.LOGIN_ERROR, OnLoginError);

        SmartFox.Disconnect();
        UnityEngine.SceneManagement.SceneManager.LoadScene(SceneName.LOGIN);
    }
}
