using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    CardManager cardManager;
    public float[] CellValues;
    private void Start()
    {
        cardManager = GetComponent<CardManager>();
        InGameManager.instance.GameStateChangeEvent += OnGameStateChanged;
    }
    private void OnGameStateChanged()
    {
        switch (InGameManager.CurrentGameState)
        {
            case GameStateType.Decision:
                StartCoroutine(PlayCardProcess());
                break;
        }         
        
    }

    public void EvaluateGridValue()
    {
        CellValues = new float[HexGrid.instance.cells.Length];
        List<HexCell> CanMoveCells = new List<HexCell>();
        foreach (HexCell cell in cardManager.OccupiedCells)
        {
            if (!CanMoveCells.Contains(cell))
            {
                CanMoveCells.Add(cell);
            }
            foreach (HexCell neighbor in cell.NearbyCells)
            {
                if (!CanMoveCells.Contains(neighbor) && neighbor.CanPass && cell.GetUnitOnCell().Health > cell.MinUnitAmount)
                {
                    CanMoveCells.Add(neighbor);
                }
            }
        }

        foreach(HexCell cell in CanMoveCells)
        {
            if (cell.OwnerManager == null)
            {
                switch (cell.CellType)
                {
                    case HexCellType.Desert:
                        CellValues[cell.HexIndex] -= 0.3f;
                        break;
                    default: CellValues[cell.HexIndex] += 1;
                        break;
                }
                
            }
            else if (cell.OwnerManager != cardManager)
            {
                if (GetNearbyHealthTotal(cell) > cell.GetUnitOnCell().GetTotalHealth())
                {
                    CellValues[cell.HexIndex] += 2;
                }
                else
                {
                    foreach(HexCell neighbor in cell.NearbyCells)
                    {
                        CellValues[neighbor.HexIndex] += 0.3f;
                    }
                }
            }
            CellValues[cell.HexIndex] += Random.Range(-0.1f,0.1f);
        }
    }

    private int GetNearbyHealthTotal(HexCell targetCell)
    {
        int t = 0;
        foreach(HexCell cell in targetCell.NearbyCells)
        {
            if (cell.OwnerManager == cardManager)
            {
                t += Mathf.Clamp(cell.GetUnitOnCell().GetTotalHealth() - cell.MinUnitAmount, 0, int.MaxValue);
            }
        }
        return t;
    }
    public HexCell GetMVC(List<HexCell> queue)
    {
        HexCell cell=null;
        float maxValue = 0;
        foreach (HexCell c in queue)
        {
            if (CellValues[c.HexIndex] > maxValue)
            {
                maxValue = CellValues[c.HexIndex];
                cell = c;
            }
        }
        return cell;
    }
    private IEnumerator PlayCardProcess()
    {
        yield return new WaitForSeconds(0.5f);
        EvaluateGridValue();
        bool haveCardToUse = true;
        while (haveCardToUse)
        {
            yield return new WaitForSeconds(0.02f);                  

            haveCardToUse = false;
            for (int i = 0; i < cardManager.Cards.Count; i++)
            {
                Card_Base card = cardManager.Cards[i];
                HexCell cell = null;

                List<HexCell> canUseCells = card.GetCanUseCells(cardManager);
                if (canUseCells.Count > 0)
                {
                    cell = GetMVC(canUseCells);
                }
            
                if (card.CanUseCard(cardManager, cell))
                {
                    print("AI use card:" + card);
                    haveCardToUse = true;
                    cardManager.UseCard(cardManager.Cards[i],cell);
                    break;
                }
            }

            if (!haveCardToUse)
            {
                cardManager.UpdateCanMoveCells();
                if (cardManager.CanMoveCells.Count > 0)
                {
                    cardManager.SetUnitMoveTo(GetMVC(cardManager.CanMoveCells));
                }
            }
        }
    }
}
