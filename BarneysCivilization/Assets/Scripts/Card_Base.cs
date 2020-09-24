using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_Base : MonoBehaviour
{
    public HexCell CardSource;
    public bool isActive=true;

    public bool NoneTargetCard()
    {
        return cardEffect.NonTargetCard;
    }
    public int ActionPointCost()
    {
        return cardEffect.ActionPointCost;
    }

    public Sprite CardImage;
    public string CardName;
    public string CardDescription;

    [SerializeField]
    private CardAppearence cardAppearence;
    private CardEffect cardEffect;

    private void Awake()
    {
        cardAppearence.RefreshAppearence(this);
        cardEffect = GetComponent<CardEffect>();
    }
    public void MouseDown()
    {
        UIManager.instance.MouseDownOnCard(this);
    }
    public void MouseUp()
    {
        
    }
    public virtual List<HexCell> GetCanUseCells(CardManager user)
    {
        return cardEffect.GetCanUseCells(user);
    }
    public virtual bool CanUseCard(CardManager user, HexCell cell)
    {
        return cardEffect.CanUseCard(user,cell);
    }
    public virtual void CardEffect(CardManager user, HexCell cell)
    {
        cardEffect.Effect(user, cell);
    }
}
