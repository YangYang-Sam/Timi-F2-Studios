using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialInstance : MonoBehaviour
{
    public Text contentText;
    public GameObject Graphic;

    public TutorialTriggerType type;
    public string content;
    public float time;
    private float timer;
    private void Start()
    {
        UIManager.instance.PlayerUseCardEvent += PlayerUseCard;
        PlayerController.instance.PlayerMoveEvent += PlayerMove;
        timer = time;
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
    }
    public void TutorialClose()
    {
        TutorialManager.instance.isInstanceGoing = false;
        Destroy(gameObject);
    }
}
public enum TutorialTriggerType
{
    None,
    UseCard,
    Move,
    Time
}