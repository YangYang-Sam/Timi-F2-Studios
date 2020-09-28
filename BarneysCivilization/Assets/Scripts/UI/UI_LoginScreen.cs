using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NetTest;

public class UI_LoginScreen : MonoBehaviour
{
    Animator animator;
    string UID;
    string Password;
    public InputField IDField;
    public InputField PWField;
    public InputField RegIDField;
    public InputField RegPWField;
    public GameObject MessagePrefab;
    public GameObject MessageInstance;

    NetManager netManager;
    private void Start()
    {
        animator = GetComponent<Animator>();
        CsResManager.LoginResult += OnLoginResult;
        CsResManager.RegistResult += OnRegistResult;
        netManager = new NetManager();
        netManager.InitState();
    }

    private void OnRegistResult(int obj)
    {
        //ShowUserMessage("注册结果："+ obj);
        print("Regist: " + obj);
    }

    private void OnLoginResult(int obj)
    {
        //ShowUserMessage("登陆结果：" + obj);
        print("Login: " + obj);
    }

    public void Login()
    {
        UID = IDField.text;
        Password = PWField.text;
        netManager.ReqLogin(UID, Password);
        //print("ID: " + UID + " PW: " + Password);
    }
    public void RegistButton()
    {
        animator.Play("RegistButton");
    }
    public void Regist()
    {
        UID = RegIDField.text;
        Password = RegPWField.text;
        netManager.ReqRegister(UID, Password);
    }
    public void BackButton()
    {
        animator.Play("BackButton");
    }
    public void ShowUserMessage(string text)
    {
        if (MessageInstance != null)
        {
            Destroy(MessageInstance);
        }
        MessageInstance = Instantiate(MessagePrefab, transform);
        MessageInstance.GetComponent<UI_LoginResultText>().ShowText(text);
        
    }
    public void OnRecLoginResult(int result)
    {
        switch (result)
        {
            case 0:
                
                break;
            case 1:
                break;
            case 2:
                break;
            default:
                break;
        }
    }
    public void OnRecRegistResult(int result)
    {
        switch (result)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
            default:
                break;
        }
    }
}
