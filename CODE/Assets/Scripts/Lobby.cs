using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lobby : MonoBehaviour
{
    [SerializeField] private Transform _parentContent, _parentContentRoom;
    [SerializeField] private Text _chatText;
    [SerializeField] private InputField _inputChat;
    [SerializeField] private ScrollRect _scrollRectChat;
    [SerializeField] private ScrollRect _scrollRectRoom;
    [SerializeField] private Button _joinRoomButton;

    private Dictionary<int, GameObject> _dictIdToObjectRoom = new Dictionary<int, GameObject>();

    private void OnEnable()
    {
        SmartFoxController.OnSendMessagePublic += _ShowChatMessage;
        SmartFoxController.OnRoomAdded += _ShowListRoom;
        SmartFoxController.OnRoomRemoved += _RemoveRoom;
    }

    private void OnDisable()
    {
        SmartFoxController.OnSendMessagePublic -= _ShowChatMessage;
        SmartFoxController.OnRoomAdded -= _ShowListRoom;
        SmartFoxController.OnRoomRemoved -= _RemoveRoom;
    }

    private void Start()
    {
        _ShowListRoom();
    }

    public void SentChatPress()
    {
        SmartFoxController.Instance.PublicMessageRequest(_inputChat.text);
        _inputChat.text = "";
    }

    public void CreateGameButton()
    {
        SmartFoxController.Instance.CreateRoomRequest();
    }

    /// <summary>
    /// show room created and new room added
    /// </summary>
    /// <param name="idRoom"></param>
    /// <param name="nameroom"></param>
    private void _ShowListRoom(int idRoom, string nameroom)
    {
        if (_dictIdToObjectRoom.ContainsKey(idRoom))
        {
            Destroy(_dictIdToObjectRoom[idRoom]);
            _dictIdToObjectRoom.Remove(idRoom);
        }
        foreach (var item in SmartFoxController.Instance.SmartFox.RoomList)
        {
            if (item.IsHidden || !item.IsGame || item.IsPasswordProtected || _dictIdToObjectRoom.ContainsKey(idRoom)) continue;
            Button button = Instantiate(_joinRoomButton, _parentContentRoom);
            Text text = button.GetComponentInChildren<Text>();
            button.onClick.AddListener(() => SmartFoxController.Instance.JoinRoomRequest(item.Name));
            text.text = item.Name;
            Canvas.ForceUpdateCanvases();
            _scrollRectRoom.verticalNormalizedPosition = 0f;
            if (!_dictIdToObjectRoom.ContainsKey(idRoom))
            {
                _dictIdToObjectRoom.Add(idRoom, button.gameObject);
            }
        }
    }

    /// <summary>
    /// init list room
    /// </summary>
    private void _ShowListRoom()
    {
        foreach (var item in SmartFoxController.Instance.SmartFox.RoomList)
        {
            if (item.IsHidden || !item.IsGame || item.IsPasswordProtected) continue;
            Button button = Instantiate(_joinRoomButton, _parentContentRoom);
            Text text = button.GetComponentInChildren<Text>();
            button.onClick.AddListener(() => SmartFoxController.Instance.JoinRoomRequest(item.Name));
            text.text = item.Name;
            Canvas.ForceUpdateCanvases();
            _scrollRectRoom.verticalNormalizedPosition = 0f;
        }
    }

    private void _ShowChatMessage(string userSend, string contentMessage)
    {
        Text text = Instantiate(_chatText, _parentContent);
        text.text = userSend + ": " + contentMessage;
        Canvas.ForceUpdateCanvases();
        _scrollRectChat.verticalNormalizedPosition = 0f;
    }

    private void _RemoveRoom(int id)
    {
        if (_dictIdToObjectRoom.ContainsKey(id))
        {
            Destroy(_dictIdToObjectRoom[id]);
        }
        _dictIdToObjectRoom.Remove(id);
    }

    private void Update()
    {
        SmartFoxController.Instance.SmartFox.ProcessEvents();
    }
}
