using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;
    public static bool IsInBattle;
    private void Awake()
    {
        instance = this;
    }

    //
    // #Add For Search Route Begin
    //
    private static void InsertAndSortByFValue(List<HexCell> list, HexCell item)
    {
        int insertIndex = -1;
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].GetFValue() > item.GetFValue())
            {
                insertIndex = i;
                break;
            }
        }

        if (insertIndex >= 0)
        {
            list.Insert(insertIndex, item);
        }
        else
        {
            list.Add(item);
        }
    }

    public static List<HexCell> SearchRoute(HexCell thisHexCell, HexCell targetHexCell, CardManager owner)
    {
        List<HexCell> openList = new List<HexCell>();
        List<HexCell> closeList = new List<HexCell>();

        HexCell nowHexCell = thisHexCell;

        openList.Add(nowHexCell);
        bool bIsFind = false;
        while (!bIsFind)
        {
            openList.Remove(nowHexCell);//将当前节点从openList中移除
            closeList.Add(nowHexCell);//将当前节点添加到closeList中
            List<HexCell> neighbors = nowHexCell.NearbyCells;
            foreach (HexCell neighbor in neighbors)
            {
                if (neighbor == null) continue;

                bool cellBelongToOwner = neighbor.OwnerManager == owner;
                bool canPassCell = cellBelongToOwner || neighbor == targetHexCell;
                //无法通过或者已在closeList
                if (closeList.Contains(neighbor) || !neighbor.CanPass || !canPassCell)
                {
                    continue;
                }
                //该节点已经在openList里 
                if (openList.Contains(neighbor))
                {
                    //计算假设从当前节点进入，该节点的g估值 
                    int assueGValue = neighbor.ComputeNearbyGValue(nowHexCell) + nowHexCell.GetGValue();
                    //假设的g估值小于于原来的g估值 
                    if (assueGValue < neighbor.GetGValue())
                    {
                        openList.Remove(neighbor);//移除neighbor
                        neighbor.SetGValue(assueGValue);//从新设置g估值 
                        neighbor.SetFatherHexCell(nowHexCell);//将当前节点设置为该节点的父节点  
                        InsertAndSortByFValue(openList, neighbor);//重新排序该neighbor节点在openList的位置 
                    }
                }
                //没有在openList里 
                else
                {
                    neighbor.SetHValue(neighbor.ComputeHValue(targetHexCell));//计算好它的h估值  
                    neighbor.SetGValue(neighbor.ComputeNearbyGValue(nowHexCell) + nowHexCell.GetGValue());//计算该节点的g估值（到当前节点的g估值加上当前节点的g估值）  
                    neighbor.SetFatherHexCell(nowHexCell);//将当前节点设置为该节点的父节点  
                    InsertAndSortByFValue(openList, neighbor);//添加到openList里 
                }

                //找到目标节点（即targetHexCell已经在openList里）
                if (neighbor == targetHexCell)
                {
                    bIsFind = true;
                    break;
                }
            }

            //找到或无法到达目标  
            if (bIsFind == true || openList.Count <= 0)
            {
                break;
            }
            else
            {
                nowHexCell = openList[0];//得到f估值最低的节点设置为当前节点  
            }
        }

        List<HexCell> route = new List<HexCell>();

        if (bIsFind)
        {
            HexCell hexCell = targetHexCell;
            while (hexCell != thisHexCell)
            {
                route.Add(hexCell);//将节点添加到路径列表里  
                hexCell = hexCell.GetFatherHexCell();//从目标节点开始搜寻父节点就是所要的路线  
            }
            route.Add(hexCell);
        }

        route.Reverse();

        return route;
    }
    //
    // #Add For Search Route End
    //

    public void MoveAllUnitsToCell(CardManager owner, HexCell targetCell, float distance)
    {
        // Hexcell 是格子，Hexcell.NearbyCells里存了跟它挨着的所有格子
        // Unit 是单位，这个函数的目标就是找到最短路径，并将上面的格子按顺序填到 unit.PathCells里面。Unit.Cell是Unit当前所在的格子
        // HexGrid 是格子管理器单例，HexGrid.instance.cells存了所有的格子
        // 这个函数是要把一个玩家的所有单位Unit遍历，每一个Unit从其所在格子Unit.cell寻到TargetCell，然后把该Unit的Unit.PathCells填好。

        foreach (Unit_Base unit in owner.Units)
        {
            List<HexCell> queue = SearchRoute(unit.Cell, targetCell,owner);
            unit.PathCells.Clear();
            for (int i = 1; i <= distance; i++)
            {
                if (queue.Count> i)
                {
                    unit.PathCells.Add(queue[i]);
                }
            }
            unit.UpdateDestinyCell();
        }
    }
}
