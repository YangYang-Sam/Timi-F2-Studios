using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_EndButton : MonoBehaviour
{
    public Button button;
    private void Update()
    {
        button.interactable = PlayerController.canControl;
    }
}
