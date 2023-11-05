using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayfabController : MonoBehaviour
{
    public static PlayfabController instance;

    [SerializeField] private InputField registerEmailInput;
    [SerializeField] private InputField registerPasswordInput;
    [SerializeField] private InputField registerConfirmPasswordInput;
    [SerializeField] private Text registerMessage;

    [SerializeField] private InputField loginEmailInput;
    [SerializeField] private InputField loginPasswordInput;
    [SerializeField] private Text loginMessage;

    [SerializeField] private GameObject loginPanel;
    [SerializeField] private GameObject registerPanel;

    public string userName;
    // Start is called before the first frame update

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject); 
        }

    }
    void Start()
    {
    }

    public void Register()
    {
        if(registerEmailInput.text == "" || registerPasswordInput.text == "" || registerConfirmPasswordInput.text == "")
        {
            registerMessage.text = "Missing information!";
            return;
        }
        if(registerPasswordInput.text.Length < 6)
        {
            registerMessage.text = "Password too short!";
            return ;
        }

        if(registerConfirmPasswordInput.text != registerPasswordInput.text)
        {
            registerMessage.text = "Password and Confirm password invalid!";
            return;
        }

        var request = new RegisterPlayFabUserRequest
        {
            Email = registerEmailInput.text,
            Password = registerPasswordInput.text,
            RequireBothUsernameAndEmail = false
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnError);
    }

    void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        registerMessage.text = "Register successfull";
        loginMessage.text = "Enter your email and password to login";

        registerPanel.SetActive(false);
        loginPanel.SetActive(true);
    }

    public void Login()
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email = loginEmailInput.text,
            Password = loginPasswordInput.text,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true
            }
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnError);
    }
    
    void OnLoginSuccess(LoginResult result)
    {
        loginMessage.text = "Login successfull";
        SceneManager.LoadScene("MainMenu");
        userName = null;
        if(result.InfoResultPayload.PlayerProfile != null)
        {
            userName = result.InfoResultPayload.PlayerProfile.DisplayName;
            PlayerPrefs.SetString("UserName", userName);
        }
    }


    public void ForgotPassword()
    {
        if(loginEmailInput.text == "")
        {
            loginMessage.text = "Please input your Email address";
            return;
        }
        var request = new SendAccountRecoveryEmailRequest
        {
            Email = loginEmailInput.text,
            TitleId = "2857F"
        };
        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnForgotPassword, OnError);
    }

    void OnForgotPassword(SendAccountRecoveryEmailResult result)
    {
        loginMessage.text = "Password reset mail sent!";
    }

    
    public void UpdateUserName(string username)
    {
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = username,
        };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdate, OnError);
    }

    void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult result)
    {
        PlayerPrefs.SetString("UserName", result.DisplayName);
    }

    public void SendLeaderboard(int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "Score",
                    Value = score
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError, null);
    }
    void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Send score successfull");
    }

    void OnError(PlayFabError error)
    {
        registerMessage.text = error.ErrorMessage;
        loginMessage.text = error.ErrorMessage;
        Debug.Log(error.GenerateErrorReport());
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
