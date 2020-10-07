using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Warning : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    public static UI_Warning instance;
    private void Awake()
    {
        instance = this;
    }
    public Text WarningText;

    public void ShowWarningText(string text)
    {
        WarningText.text = text;
        animator.Play("WarningText");
    }
}
