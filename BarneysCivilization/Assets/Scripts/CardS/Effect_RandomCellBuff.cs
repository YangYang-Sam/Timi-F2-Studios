using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_RandomCellBuff : CardEffect
{
    public int Turns;
    public GameObject BuffPrefab;
    public List<HexCellType> Types;
    public override bool CanUseCard(CardManager user, HexCell cell)
    {
        return base.CanUseCard(user, cell);
    }
    public override List<HexCell> GetCanUseCells(CardManager user)
    {
        return base.GetCanUseCells(user);
    }
    public override void Effect(CardManager user, HexCell cell)
    {
        List<HexCell> PossibleCells = new List<HexCell>();
        foreach (HexCell c in HexGrid.instance.cells)
        {
            if (c.isValidCell && Types.Contains(c.CellType))
            {
                PossibleCells.Add(c);
            }
        }
        int cellCount = PossibleCells.Count;
        Random.InitState(UserData.instance.RandomSeeds[1]);
        int randomCellIndex = Random.Range(0, cellCount);
        var randomCell = PossibleCells[randomCellIndex];

        GameObject g = Instantiate(BuffPrefab, randomCell.transform.position, Quaternion.identity);
        CellBuff_Base buff = g.GetComponent<CellBuff_Base>();
        buff.Turns = Turns;
        buff.OnCreated(randomCell, user);
    }
}
