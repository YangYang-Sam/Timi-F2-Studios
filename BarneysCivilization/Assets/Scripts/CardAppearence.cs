using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardAppearence : MonoBehaviour
{
    [SerializeField]
    private Card_Base card;
    [SerializeField]
    private GameObject Graphic;

    public Image CardImage;
    public Text NameText;
    public Text DescriptionText;

    public void SetVisibility(bool visible)
    {
        Graphic.SetActive(visible);
    }
    public void RefreshAppearence(Card_Base card)
    {
        CardImage.sprite = card.CardImage;
        NameText.text = card.CardName;
        DescriptionText.text = card.CardDescription;
    }
}
