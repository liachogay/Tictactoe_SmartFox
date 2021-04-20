using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
        if (value)
        {
            SceneManager.LoadScene(SceneName.LOBBY);
        }
    }

    private void OnDestroy()
    {
        SmartFoxController.OnLoginSuccess -= LoginState;
    }
}
