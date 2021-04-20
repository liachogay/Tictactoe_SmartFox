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

    public void LoginRequest(string userName)
    {
        name = userName;
        Initialize();
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
        }
    }

    private void OnLogin(BaseEvent evt)
    {
        OnLoginSuccess(true);
        User user = (User)(evt.Params["user"]);
        if (SmartFox.RoomList.Count > 0)
        {
            SmartFox.Send(new JoinRoomRequest(SmartFox.RoomList[0]));
        }
    }

    private void OnLoginError(BaseEvent evt)
    {
        OnLoginSuccess(false);
    }

    public void _Reset()
    {

    }
}
