using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_LoginResultText : MonoBehaviour
{
    public int LifeSpan;
    public Text text;
    public void ShowText(string t)
    {
        text.text = t;
        Destroy(gameObject, LifeSpan);
    }
}
