using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Requests;
using Sfs2X.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    [SerializeField] private InputField _userName;

    private void Awake()
    {
        SmartFoxController.OnLoginSuccess += LoginState;
    }

    // Start is called before the first frame update
    void Start()
    {
        //SmartFoxController.Instance.Initialize();
    }

    private void Update()
    {
        SmartFoxController.Instance.SmartFox.ProcessEvents();
    }

    public void LoginButton()
    {
        SmartFoxController.Instance.LoginRequest(_userName.text);
    }
    /// <summary>
    /// Use for notificate user login success or not
    /// </summary>
    private void LoginState(bool value)
    {
        Debug.Log("User is login: " + value);
    }

    private void OnDestroy()
    {
        SmartFoxController.OnLoginSuccess -= LoginState;
    }
}
