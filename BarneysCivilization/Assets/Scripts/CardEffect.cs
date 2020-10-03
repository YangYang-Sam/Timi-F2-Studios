using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEffect : MonoBehaviour
{
    public int ActionPointCost=1;
    public bool NonTargetCard = false;
    public virtual List<HexCell> GetCanUseCells(CardManager user)
    {
        return new List<HexCell>();
    }
    public virtual bool CanUseCard(CardManager user, HexCell cell)
    {
        return user.ActionPoint >= ActionPointCost && (NonTargetCard || GetCanUseCells(user).Contains(cell));
    }
    public virtual void Effect(CardManager user, HexCell cell)
    {

    }
}
