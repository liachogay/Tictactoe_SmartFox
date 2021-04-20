using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
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
