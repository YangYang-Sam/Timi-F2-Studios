using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_Grass : Card_Base
{
    public override void CardEffect(CardManager user, HexCell cell)
    {
        base.CardEffect(user,cell);
        user.ActionPoint += 1;
        user.TempResourceAmount += 1;
    }
}
