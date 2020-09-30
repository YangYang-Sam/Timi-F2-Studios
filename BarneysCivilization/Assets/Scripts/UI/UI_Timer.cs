using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Timer : MonoBehaviour
{
    public Image FillBar;
    float Duration;
    float StartTime;
    void Start()
    {
        InGameManager.instance.GameStateChangeEvent += OnGameStateChange;
        Duration = InGameManager.instance.DecisionDuration;
    }
    void Update()
    {
        float percent = 1 - ((Time.time - StartTime) / Duration);
        if (percent > 0)
        {
            FillBar.fillAmount = percent;
        }
        else
        {
            FillBar.fillAmount = 0;
        }
       
    }
    private void OnGameStateChange()
    {
        switch (InGameManager.CurrentGameState)
        {
            case GameStateType.Decision:
                StartTime = Time.time;
                break;
        }
    }

 

}
