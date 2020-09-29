using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NetTest;
using UnityEngine.SceneManagement;

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

    private bool updateLoginResult;
    private int LoginResult;
    private bool updateRegistResult;
    private int RegistResult;

    NetManager netManager;
    private void Start()
    {
        animator = GetComponent<Animator>();
        CsResManager.LoginResult += OnLoginResult;
        CsResManager.RegistResult += OnRegistResult;
        netManager = new NetManager();
        netManager.InitState();
    }
    private void Update()
    {
        if (updateLoginResult)
        {
            updateLoginResult = false;
            switch (LoginResult)
            {
                case 0:
                    ShowUserMessage("登陆成功",Color.green);
                    UserData.instance.UID = UID;
                    SceneManager.LoadScene("MainScene");
                    break;
                case 1:
                    ShowUserMessage("密码错误", Color.red);
                    break;
                case 2:
                    ShowUserMessage("用户名不存在", Color.red);
                    break;
            }
        }
        if (updateRegistResult)
        {
            updateRegistResult = false;
            switch (RegistResult)
            {
                case 0:
                    ShowUserMessage("注册成功", Color.green);
                    break;
                case 1:
                    ShowUserMessage("用户名已存在", Color.red);
                    break;
            }
        }
    }
    private void OnRegistResult(int obj)
    {
        //ShowUserMessage("注册结果："+ obj);
        print("Regist: " + obj);
        RegistResult = obj;
        updateRegistResult = true;
    }

    private void OnLoginResult(int obj)
    {
        //ShowUserMessage("登陆结果：" + obj);
        print("Login: " + obj);
        LoginResult = obj;
        updateLoginResult = true;
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
    public void ShowUserMessage(string text,Color color)
    {
        if (MessageInstance != null)
        {
            Destroy(MessageInstance);
        }
        MessageInstance = Instantiate(MessagePrefab, transform);
        MessageInstance.GetComponent<UI_LoginResultText>().ShowText(text,color);        
    }
}
