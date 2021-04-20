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
    }

    private void OnDisable()
    {
        SmartFoxController.OnEnemyMove -= SetEnemyMove;
    }

    // Start is called before the first frame update
    void Start()
    {
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

    private void SetEnemyMove(int x, int y)
    {
        _matrixButtonPlay[x][y].GetComponentInChildren<Text>().text = "X";
        _matrixButtonPlay[x][y].enabled = false;
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
