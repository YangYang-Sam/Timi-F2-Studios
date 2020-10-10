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
        float percent = 1 - ((Time.time - InGameManager.instance.DecisionTimer) / InGameManager.instance.DecisionDuration);
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
