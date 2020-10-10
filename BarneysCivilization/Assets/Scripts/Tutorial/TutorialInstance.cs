﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialInstance : MonoBehaviour
{
    public GameObject Graphic;
    public Text TimerText;

    public TutorialTriggerType type;
    public float time;
    private float timer;
    public bool MoveCameraToEnemy;
    public GameObject StartCloseObj;

    private bool isStart=false;
    private float Duration=3;
    private float durationTimer;
    private void Start()
    {
        UIManager.instance.PlayerUseCardEvent += PlayerUseCard;
        PlayerController.instance.PlayerMoveEvent += PlayerMove;
        InGameManager.instance.LateDecisionEvent += OnLateDecision;
        UIManager.instance.ChooseCardFinish += ChooseCard;
        timer = time;
        durationTimer = Duration;
        if (type == TutorialTriggerType.None)
        {
            TutorialStart();
        }
    }

 
    private void ChooseCard()
    {
        if (type == TutorialTriggerType.ChooseCard)
        {
            TutorialStart();
        }
    }

    private void OnLateDecision()
    {
        if (type == TutorialTriggerType.TurnStart)
        {
            TutorialStart();
        }
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            if (type == TutorialTriggerType.Time)
            {
                TutorialStart();
            }
        }
        if (isStart)
        {
            durationTimer -= Time.deltaTime;
            int t = Mathf.FloorToInt(durationTimer);
            if (t > 0)
            {
                TimerText.text = t.ToString();
            }
            else
            {
                TimerText.text = "点击继续";
            }
        }        
    }

    private void PlayerMove(HexCell obj)
    {
        if (type == TutorialTriggerType.Move)
        {
            TutorialStart();
        }
    }

    private void PlayerUseCard(Card_Base obj)
    {
        if (type == TutorialTriggerType.UseCard)
        {
            TutorialStart();
        }
    }

    public void TutorialStart()
    {
        Graphic.SetActive(true); 
        isStart = true;
        if (MoveCameraToEnemy)
        {
            BattleCamera.instance.MoveToCore(1);
        }
        if (StartCloseObj)
        {
            StartCloseObj.SetActive(false);
        }
    }
    public void TutorialClose()
    {
        if (durationTimer <=0)
        {
            UIManager.instance.PlayerUseCardEvent -= PlayerUseCard;
            PlayerController.instance.PlayerMoveEvent -= PlayerMove;
            InGameManager.instance.LateDecisionEvent -= OnLateDecision;
            UIManager.instance.ChooseCardFinish -= ChooseCard;
            TutorialManager.instance.isInstanceGoing = false;
            Destroy(gameObject);
        }
    }
}
public enum TutorialTriggerType
{
    None,
    UseCard,
    Move,
    Time,
    TurnStart,
    ChooseCard
}