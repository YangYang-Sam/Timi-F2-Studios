using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : MonoBehaviour
{
    public HexCell Cell;
    public CardManager Owner;

    public void InitCore(HexCell cell,CardManager owner )
    {
        Cell = cell;
        Owner = owner;
        cell.OwnerChangeEvent += OnCellChangeOwner;
    }

    private void OnCellChangeOwner(CardManager obj)
    {
        if (Owner != Cell.OwnerManager)
        {
            Owner.CampLost(Cell.OwnerManager);
        }
    }
}
