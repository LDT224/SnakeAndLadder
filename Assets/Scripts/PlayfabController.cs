using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;

public class PlayfabController : MonoBehaviour
{
    [SerializeField] private InputField registerEmailInput;
    [SerializeField] private InputField registerPasswordInput;
    [SerializeField] private InputField registerConfirmPasswordInput;
    [SerializeField] private Text RegisterMessage;

    [SerializeField] private InputField loginEmailInput;
    [SerializeField] private InputField loginPasswordInput;
    [SerializeField] private Text loginMessage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Register()
    {
        if(registerEmailInput.text == "" || registerPasswordInput.text == "" || registerConfirmPasswordInput.text == "")
        {
            RegisterMessage.text = "Missing information!";
            return;
        }
        if(registerPasswordInput.text.Length < 6)
        {
            RegisterMessage.text = "Password too short!";
            return ;
        }

        if(registerConfirmPasswordInput.text != registerPasswordInput.text)
        {
            RegisterMessage.text = "Password and Confirm password invalid!";
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
        RegisterMessage.text = "Register successfull";
    }

    public void Login()
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email = loginEmailInput.text,
            Password = loginPasswordInput.text,
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnError);
    }
    
    void OnLoginSuccess(LoginResult result)
    {
        loginMessage.text = "Login successfull";
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
    void OnError(PlayFabError error)
    {
        RegisterMessage.text = error.ErrorMessage;
        loginMessage.text = error.ErrorMessage;
        Debug.Log(error.GenerateErrorReport());
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
