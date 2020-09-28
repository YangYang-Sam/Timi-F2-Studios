using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_LoginScreen : MonoBehaviour
{
    Animator animator;
    string UID;
    string Password;
    public InputField IDField;
    public InputField PWField;
    public GameObject MessagePrefab;
    public GameObject MessageInstance;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void Login()
    {
        UID = IDField.text;
        Password = PWField.text;
        print("ID: " + UID + " PW: " + Password);
        ShowUserMessage("登陆中");
    }
    public void RegistButton()
    {
        animator.Play("RegistButton");
    }
    public void Regist()
    {
        ShowUserMessage("注册成功");
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
