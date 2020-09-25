using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Assertions;
using Debug = System.Diagnostics.Debug;

public class HexCell : MonoBehaviour
{
    public HexCoordinates coordinates;
    public HexCellType CellType;
    public int HexIndex;

    public int MinUnitAmount = 1;

    public Color color;

    public CardManager OwnerManager;

    public List<Unit_Base> PlacedUnits = new List<Unit_Base>();

    public List<HexCell> NearbyCells = new List<HexCell>();

    public HexCell PathFromCell;

    private List<Unit_Base> CampUnits = new List<Unit_Base>();
    private List<int> Camps = new List<int>();

    public Building_Base PlacedBuilding;
    public List<CellBuff_Base> CellBuffs = new List<CellBuff_Base>();

    public bool CanPass = true;

    public UnityEngine.UI.Text TypeText;

    [SerializeField]
    private SpriteRenderer HightlightSprite;
    
    //
    // #Add For Search Route Begin
    //
   
    HexCell FatherCell = null;
    private int GValue = 999;
    private int HValue = 999;

    public void SetFatherHexCell(HexCell f)
    {
        FatherCell = f;
    }

    public HexCell GetFatherHexCell()
    {
        return FatherCell;
    }

    public void SetGValue(int v)
    {
        GValue = v;
    }

    public void SetHValue(int v)
    {
        HValue = v;
    }

    public int GetGValue()
    {
        return GValue;
    }

    public int GetHValue()
    {
        return HValue;
    }

    public int GetFValue()
    {
        return GValue + HValue;
    }

    // Compute G value from InHex to current hex
    public int ComputeNearbyGValue(HexCell InHex)
    {
        Debug.Assert(InHex.NearbyCells.Contains(this), "Current Hex Must In InHex's NearbyCells.");
        return 1;
    }

    // Compute H value from current hex to targetHex
    public int ComputeHValue(HexCell targetHex)
    {
        return coordinates.DistanceTo(targetHex.coordinates);
    }

    //
    // #Add For Search Route End
    //
    public void GetNearbyCells()
    {
        foreach (HexCell cell in HexGrid.instance.cells)
        {
            if (cell != this && cell.coordinates.DistanceTo(coordinates) <= 1)
            {
                NearbyCells.Add(cell);
            }
        }
    }

    public bool CanPlaceUnit(CardManager owner)
    {
        bool result = true;
        return result;
    }

    public void AddUnit(Unit_Base unit)
    {
        PlacedUnits.Add(unit);
    }
    public Unit_Base GetUnitOnCell()
    {
        if (PlacedUnits.Count > 0)
        {
            return PlacedUnits[0];
        }
        else
        {
            return null;
        }
    }
    public void RemoveUnit(Unit_Base unit)
    {
        PlacedUnits.Remove(unit);
    }
    public void ChangeOwner(Unit_Base newOwnerUnit)
    {
        CardManager newOwner = null;
        if (newOwnerUnit != null)
        {
            newOwner = newOwnerUnit.Owner;
        }
        if (newOwner != OwnerManager)
        {
            CardManager oldOwner = OwnerManager;
            OwnerManager = newOwner;

            if (oldOwner != null)
            {
                oldOwner.LostCell(this);
            }

            if (newOwnerUnit != null)
            {
                newOwnerUnit.OccupyCell(this);
            }

            if (OwnerChangeEvent != null)
            {
                OwnerChangeEvent(oldOwner);
            }
        }
    }
    public void UnitArrived(Unit_Base unit)
    {
        if (Camps.Contains(unit.Owner.camp))
        {
            unit.Merged(CampUnits[Camps.IndexOf(unit.Owner.camp)]);
        }
        else
        {
            Camps.Add(unit.Owner.camp);
            CampUnits.Add(unit);
        }
    }
    public void CheckOwner()
    {
        // 清理无效单位
        if (PlacedUnits.Count > 0)
        {
            for (int i = PlacedUnits.Count - 1; i >= 0; i--)
            {
                if (PlacedUnits[i] == null || !PlacedUnits[i].isAlive)
                {
                    PlacedUnits.RemoveAt(i);
                }
            }
        }

        // 战斗
        if (Camps.Count > 1)
        {
            int TopHealth = 0;
            int SecondHealth = 0;
            Unit_Base TopUnit=null;
            Unit_Base SecondUnit = null;
            for (int i = 0; i < Camps.Count; i++)
            {
                int health = CampUnits[i].GetTotalHealth();
                if (health > TopHealth)
                {
                    SecondHealth = TopHealth;
                    TopHealth = health;
                    SecondUnit = TopUnit;
                    TopUnit = CampUnits[i];                    
                }
                else if(health> SecondHealth)
                {
                    SecondHealth = health;
                    SecondUnit = CampUnits[i];
                }
            }
            for (int i = 0; i < Camps.Count; i++)
            {
                if (CampUnits[i] == TopUnit)
                {
                    CampUnits[i].TakeDamage(SecondHealth, SecondUnit);
                }
                else
                {
                    CampUnits[i].TakeDamage(SecondHealth, TopUnit);
                }
            }
            ArtResourceManager.instance.CreateBattleEffect(transform.position +  new Vector3(0,2,0));
        }

        // 清理无效单位
        if (PlacedUnits.Count > 0)
        {
            for (int i = PlacedUnits.Count - 1; i >= 0; i--)
            {
                if (PlacedUnits[i] == null || !PlacedUnits[i].isAlive)
                {
                    PlacedUnits.RemoveAt(i);
                }
            }
        }

        if (PlacedUnits.Count == 0)
        {
            ChangeOwner(null);            
        }
        else
        {
            if ( PlacedUnits[0].Owner != OwnerManager)
            {               
                ChangeOwner(PlacedUnits[0]);
            }
            else
            {
                OwnerManager = PlacedUnits[0].Owner;
            }
            if (PlacedUnits.Count >= 2)
            {
                //Debug.LogError("Cell unit count larger than 1, Cell:" + this + " Unit1:" + PlacedUnits[0] + " Unit2:" + PlacedUnits[1]);
            }
        }

        Camps.Clear();
        CampUnits.Clear();
    }

    public void HighLightCell(bool highLight)
    {
        HightlightSprite.enabled = highLight;
    }
    public void SetHighLightColor(Color c)
    {
        HightlightSprite.color = c;
    }

    public bool isValidCell;

    public event System.Action<CardManager> OwnerChangeEvent;
}
public enum HexCellType
{
    Grass,
    Forest,
    Hill,
    Desert,
    Water,
    Snow,
    Montain
}