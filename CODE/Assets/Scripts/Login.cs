using UnityEngine;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    [SerializeField] private InputField _userName;
    [SerializeField] private InputField _pass;
    [SerializeField] private InputField _passChange;

    private void Awake()
    {
        SmartFoxController.OnLoginSuccess += LoginState;
    }

    // Start is called before the first frame update
    void Start()
    {
        SmartFoxController.Instance.Initialize();
    }

    private void Update()
    {
        if (SmartFoxController.Instance.SmartFox != null)
        {
            SmartFoxController.Instance.SmartFox.ProcessEvents();
        }
    }

    public void SignUpButton()
    {
        SmartFoxController.Instance.SignUpRequest(_userName.text, _pass.text, _passChange.text);
    }

    public void LoginButton()
    {
        SmartFoxController.Instance.LoginRequest(_userName.text, _pass.text);
    }
    /// <summary>
    /// Use for notificate user login success or not
    /// </summary>
    private void LoginState(bool value)
    {
        Debug.Log("User is login: " + value);
        if (value)
        {
            //SceneManager.LoadScene(SceneName.LOBBY);
        }
    }

    private void OnDestroy()
    {
        SmartFoxController.OnLoginSuccess -= LoginState;
    }
}
