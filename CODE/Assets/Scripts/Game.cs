using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    [SerializeField] private Button _buttonPlay;
    [SerializeField] private Transform _contentButton;

    private List<List<Button>> _matrixButtonPlay = new List<List<Button>>();

    private void OnEnable()
    {
        SmartFoxController.OnEnemyMove += SetEnemyMove;
        SmartFoxController.OnSetBoardEnable += SetBoardeEnable;
        SmartFoxController.OnStartGame += StartGame;
    }

    private void OnDisable()
    {
        SmartFoxController.OnEnemyMove -= SetEnemyMove;
        SmartFoxController.OnSetBoardEnable -= SetBoardeEnable;
        SmartFoxController.OnStartGame -= StartGame;
    }

    // Start is called before the first frame update
    void Start()
    {
        _contentButton.gameObject.SetActive(false);
        for (int i = 0; i < 3; i++)
        {
            _matrixButtonPlay.Add(new List<Button>());
            for (int j = 0; j < 3; j++)
            {
                int x = i;
                int y = j;
                Button button = Instantiate(_buttonPlay, _contentButton);
                _matrixButtonPlay[x].Add(button);
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => SendMoveToEnemy(x, y));
            }
        }
    }

    private void StartGame(int turnPlayer, int idPlayer1, int idPlayer2, string namePlayer1, string namePlayer2)
    {
        _contentButton.gameObject.SetActive(true);
        if (SmartFoxController.Instance.SmartFox.MySelf.Id == turnPlayer)
        {
            SetBoardeEnable(new bool[9] { true, true, true, true, true, true, true, true, true });
        }
        else
        {
            SetBoardeEnable(null);
        }
    }

    private void SetEnemyMove(int x, int y, string text)
    {
        _matrixButtonPlay[x][y].GetComponentInChildren<Text>().text = text;
        _matrixButtonPlay[x][y].enabled = false;
    }

    private void SetBoardeEnable(bool[] boolArray = null)
    {
        if (boolArray == null)
        {
            for (int i = 0; i < _matrixButtonPlay.Count; i++)
            {
                for (int j = 0; j < _matrixButtonPlay[i].Count; j++)
                {
                    _matrixButtonPlay[i][j].enabled = false;
                }
            }
        }
        else
        {
            for (int i = 0; i < _matrixButtonPlay.Count; i++)
            {
                for (int j = 0; j < _matrixButtonPlay[i].Count; j++)
                {
                    _matrixButtonPlay[i][j].enabled = boolArray[i * 3 + j];
                }
            }
        }
    }


    public void SendMoveToEnemy(int x, int y)
    {
        SmartFoxController.Instance.SendMoveToEnemy(x, y);
    }

    public void LeaveRoom()
    {
        SmartFoxController.Instance.LeaveRoomRequest();
    }

    // Update is called once per frame
    void Update()
    {
        SmartFoxController.Instance.SmartFox.ProcessEvents();
    }
}
