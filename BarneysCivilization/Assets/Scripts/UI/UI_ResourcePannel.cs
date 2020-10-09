using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_ResourcePannel : MonoBehaviour
{
    public int resource;
    public int actionPoint;
    public int deckAmount;
    public int CellAmount;

    public TextMeshProUGUI ResourceText;
    public TextMeshProUGUI ActionPointText;
    public TextMeshProUGUI CardDeckText;
    public Animator ResourceAnimator;
    public Animator ActionAnimator;
    public Animator CardDeckAnimator;

    private void Update()
    {
        if (PlayerController.instance.cardManager)
        {
            if (resource != PlayerController.instance.cardManager.GetTotalResource())
            {
                resource = PlayerController.instance.cardManager.GetTotalResource();
                ResourceAnimator.Play("ResourceIncrease");
                ResourceText.text = resource.ToString();
            }

            if (deckAmount != PlayerController.instance.cardManager.InGameCardDeck.Count)
            {   
                deckAmount = PlayerController.instance.cardManager.InGameCardDeck.Count;
                CardDeckText.text = deckAmount.ToString();
            }

            if (actionPoint != PlayerController.instance.cardManager.ActionPoint)
            {
                if (actionPoint < PlayerController.instance.cardManager.ActionPoint)
                {
                    ActionAnimator.Play("ActionPoint");
                }
                actionPoint = PlayerController.instance.cardManager.ActionPoint;
                ActionPointText.text = actionPoint.ToString();
            }

            if(CellAmount!= PlayerController.instance.cardManager.OccupiedCells.Count)
            {
                if (CellAmount > PlayerController.instance.cardManager.OccupiedCells.Count)
                {
                    CardDeckAnimator.Play("CardDeckDecrease");
                }
                else
                {
                    CardDeckAnimator.Play("CardDeck");
                }
                CellAmount = PlayerController.instance.cardManager.OccupiedCells.Count;
            }
        }
    }
}
