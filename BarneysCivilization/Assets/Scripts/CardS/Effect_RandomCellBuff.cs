using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_RandomCellBuff : CardEffect
{
    public int Turns;
    public GameObject BuffPrefab;
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
        int cellCount = HexGrid.instance.cells.Length;
        Random.InitState(UserData.instance.RandomSeeds[1]);
        int randomCellIndex = Random.Range(0, cellCount - 1);
        var randomCell = HexGrid.instance.cells[randomCellIndex];

        GameObject g = Instantiate(BuffPrefab, randomCell.transform.position, Quaternion.identity);
        CellBuff_Base buff = g.GetComponent<CellBuff_Base>();
        buff.Turns = Turns;
        buff.OnCreated(randomCell, user);
    }
}
