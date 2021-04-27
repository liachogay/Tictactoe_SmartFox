using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using Sfs2X.Entities.Variables;
using Sfs2X.Requests;
using Sfs2X.Util;
using System.Collections.Generic;
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
    private int IDPlayer = -1;

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

    public delegate void EnemyMove(int x, int y, string text);
    public static EnemyMove OnEnemyMove = null;

    public delegate void SetBoardEnable(bool[] boolArray);
    public static SetBoardEnable OnSetBoardEnable = null;

    public delegate void StartGame(int turnPlayer, int idPlayer1, int idPlayer2, string namePlayer1, string namePlayer2);
    public static StartGame OnStartGame = null;

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
        RoomSettings roomSettings = GetRoomSettings(roomName);
        SmartFox.Send(new CreateRoomRequest(roomSettings, true, SmartFox.LastJoinedRoom));
    }

    public void JoinRoomRequest(string idRoom)
    {
        Debug.Log("Press join room: " + idRoom);
        SmartFox.Send(new JoinRoomRequest(idRoom, "", SmartFox.LastJoinedRoom.Id, false));
    }

    public void LeaveRoomRequest()
    {
        //SmartFox.Send(new LeaveRoomRequest());
        SmartFox.Send(new JoinRoomRequest("The Lobby"));
        UnityEngine.SceneManagement.SceneManager.LoadScene(SceneName.LOBBY);
    }

    public void SendMoveToEnemy(int x, int y)
    {
        ISFSObject sFS = new SFSObject();
        sFS.PutInt("x", x);
        sFS.PutInt("y", y);
        SmartFox.Send(new ExtensionRequest("move", sFS, SmartFox.LastJoinedRoom));
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
            SmartFox.AddEventListener(SFSEvent.USER_ENTER_ROOM, OnUserEnterRoom);
            SmartFox.AddEventListener(SFSEvent.USER_EXIT_ROOM, OnUserExitRoom);
            SmartFox.AddEventListener(SFSEvent.EXTENSION_RESPONSE, OnExtensionResponse);
            SmartFox.AddEventListener(SFSEvent.ROOM_VARIABLES_UPDATE, OnRoomVariableUpdate);

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
        if (OnRoomAdded != null)
        {
            OnRoomAdded(room.Id, room.Name);
        }
    }

    private void OnRoomJoin(BaseEvent evt)
    {
        Room room = (Room)(evt.Params["room"]);
        Debug.Log("Join room: " + room.Name);
        if (room.Name != "The Lobby")
        {
            SmartFox.Send(new ExtensionRequest("ready", new SFSObject(), SmartFox.LastJoinedRoom));
            UnityEngine.SceneManagement.SceneManager.LoadScene(SceneName.GAME);
        }
    }

    private void OnRoomJoinError(BaseEvent evt)
    {
        Debug.Log("Join room error: " + evt.Params["errorMessage"]);
    }

    private void OnRoomRemove(BaseEvent evt)
    {
        Room room = (Room)(evt.Params["room"]);
        if (OnRoomRemoved != null)
        {
            OnRoomRemoved(room.Id);
        }
    }

    private void OnUserExitRoom(BaseEvent evt)
    {
        Room room = (Room)(evt.Params["room"]);
        User user = (User)(evt.Params["user"]);
        Debug.Log(user.Name + " leave room " + room.Name);
    }

    private void OnUserEnterRoom(BaseEvent evt)
    {
        Room room = (Room)(evt.Params["room"]);
        User user = (User)(evt.Params["user"]);
        Debug.Log(user.Name + " enter room " + room.Name);
    }

    private void OnRoomVariableUpdate(BaseEvent evt)
    {
        List<string> changedVar = (List<string>)evt.Params["changedVars"];
        foreach (var item in changedVar)
        {
            Debug.Log(item);
        }
    }

    private void OnExtensionResponse(BaseEvent evt)
    {
        string cmd = (string)(evt.Params["cmd"]);

        ISFSObject sfs = (SFSObject)(evt.Params["params"]);

        switch (cmd)
        {
            case "move":
                //true is empty
                if (OnEnemyMove != null)
                {
                    OnEnemyMove(sfs.GetInt("x"), sfs.GetInt("y"), sfs.GetUtfString("text"));
                }
                if (sfs.GetInt("sender") == SmartFox.MySelf.Id)
                {
                    if (OnSetBoardEnable != null)
                    {
                        OnSetBoardEnable(null);
                    }
                }
                break;
            case "getSpotEmpty":
                if (OnSetBoardEnable != null)
                {
                    OnSetBoardEnable(sfs.GetBoolArray("boardEmpty"));
                }
                break;
            case "start":
                var turn = sfs.GetInt("turn");
                var idPlayer1 = sfs.GetInt("idPlayer1");
                var idPlayer2 = sfs.GetInt("idPlayer2");
                var namePlayer1 = sfs.GetUtfString("namePlayer1");
                var namePlayer2 = sfs.GetUtfString("namePlayer2");
                if (OnStartGame != null)
                {
                    OnStartGame(turn, idPlayer1, idPlayer2, namePlayer1, namePlayer2);
                }
                break;
            case "tie":
                Debug.Log("two player is draw");
                break;
            case "win":
                Debug.Log("winner: " + sfs.GetInt("winner"));
                break;
        }
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
        room.MaxVariables = 10;
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
        SmartFox.RemoveEventListener(SFSEvent.PUBLIC_MESSAGE, OnPublicMessage);
        SmartFox.RemoveEventListener(SFSEvent.ROOM_ADD, OnRoomAdd);
        SmartFox.RemoveEventListener(SFSEvent.ROOM_JOIN, OnRoomJoin);
        SmartFox.RemoveEventListener(SFSEvent.ROOM_JOIN_ERROR, OnRoomJoinError);
        SmartFox.RemoveEventListener(SFSEvent.ROOM_REMOVE, OnRoomRemove);
        SmartFox.RemoveEventListener(SFSEvent.USER_ENTER_ROOM, OnUserEnterRoom);
        SmartFox.RemoveEventListener(SFSEvent.USER_EXIT_ROOM, OnUserExitRoom);

        SmartFox.Disconnect();
        UnityEngine.SceneManagement.SceneManager.LoadScene(SceneName.LOGIN);
    }
}
