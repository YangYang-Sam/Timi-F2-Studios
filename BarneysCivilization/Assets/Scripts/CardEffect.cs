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
    public virtual UseCardFailReason GetFailReason(CardManager user, HexCell cell)
    {
        UseCardFailReason reason = UseCardFailReason.None;
        if(user.ActionPoint < ActionPointCost)
        {
            reason = UseCardFailReason.NoActionPoint;
        }
        else if(!NonTargetCard && !GetCanUseCells(user).Contains(cell))
        {
            reason = UseCardFailReason.NotValidCell;         
        }
        return reason;
    }
    public static string GetWarningTextByFailReason(UseCardFailReason reason)
    {
        string t = "";
        switch (reason)
        {
            case UseCardFailReason.InvalidUnitLevel:
                return "单位等级不符";
            case UseCardFailReason.NoActionPoint:
                return "行动力不足";
            case UseCardFailReason.NoHealthPoint:
                return "单位战斗力不足";
            case UseCardFailReason.None:
                return "";
            case UseCardFailReason.NoResource:
                return "资源不足";
            case UseCardFailReason.NotValidCell:
                return "无法在该格子释放";
            case UseCardFailReason.NotValidCellType:
                return "格子类型不符";
            case UseCardFailReason.CantUseOnCore:
                return "不能在核心格子上释放";
            case UseCardFailReason.NoEmptyDesert:
                return "没有空沙漠地块";
        }
        return t;
    }
}

public enum UseCardFailReason
{
    None,
    NoActionPoint,
    NoHealthPoint,
    NoResource,
    NotValidCell,
    NotValidCellType,
    InvalidUnitLevel,
    CantUseOnCore,
    NoEmptyDesert
}