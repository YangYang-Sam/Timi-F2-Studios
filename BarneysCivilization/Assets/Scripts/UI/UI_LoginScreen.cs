using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_LoginScreen : MonoBehaviour
{
    Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void Login()
    {

    }
    public void RegistButton()
    {
        animator.Play("RegistButton");
    }
    public void Regist()
    {

    }
    public void BackButton()
    {
        animator.Play("BackButton");
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
