using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_Forest : Card_Base
{
    public override void CardEffect(CardManager user, HexCell cell)
    {
        base.CardEffect(user,cell);
        user.TempResourceAmount += 2;
    }
}
