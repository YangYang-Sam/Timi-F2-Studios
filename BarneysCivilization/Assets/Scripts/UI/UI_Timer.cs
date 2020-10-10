using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Timer : MonoBehaviour
{
    public Image FillBar;
    void Start()
    {
    }
    void Update()
    {
        float percent = (InGameManager.instance.DecisionTimer-Time.time) / InGameManager.instance.DecisionDuration;
        print(percent);
        if (percent > 0)
        {
            FillBar.fillAmount = percent;
        }
        else
        {
            FillBar.fillAmount = 0;
        }
       
    }
 

}
