using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_Base : MonoBehaviour
{
    public HexCell Cell;
    public CardManager Owner;
    public bool IsPermenant;
    public int Turns;

    public int Level = 1;
    public int MaxLevel = 1;
    public Transform EffectTransform;

    public BuildingType Type;
    public virtual void OnCreated(HexCell cell,CardManager owner)
    {
        Cell = cell;
        Owner = owner;
        cell.OwnerChangeEvent += OnCellChangeOwner;
        cell.OnBuildingCreatedOnCell(this);
        InGameManager.instance.GameStateChangeEvent += OnGameStateChange;
        if (!EffectTransform)
        {
            EffectTransform = transform;
        }
    }
    public void ChangeCell(HexCell newCell)
    {
        Cell.OwnerChangeEvent -= OnCellChangeOwner;
        InGameManager.instance.GameStateChangeEvent -= OnGameStateChange;
        Cell.PlacedBuilding = null;

        Cell = newCell;
        Cell.OwnerChangeEvent += OnCellChangeOwner;
        Cell.PlacedBuilding = this;
        InGameManager.instance.GameStateChangeEvent += OnGameStateChange;

        transform.position = Cell.transform.position;
    }
    public virtual bool CanUpgrade(CardManager user)
    {
        return MaxLevel > Level;
    }
    public virtual void UpgradeBuilding(HexCell cell, CardManager owner)
    {
        Level++;
        cell.OnBuildingUpgradeOnCell(this);
    }
    protected virtual void OnGameStateChange()
    {
        if (InGameManager.isGameState(GameStateType.Decision))
        {
            if (!IsPermenant)
            {
                Turns--;
                if (Turns <= 0)
                {
                    //OnBuildingDestroy();
                    //Owner.BuildingDestroy(this);
                    //Destroy(gameObject);

                    DestroyBuilding(this);
                }
            }     
        }
    }

    public virtual void OnBuildingDestroy()
    {
        Cell.OwnerChangeEvent -= OnCellChangeOwner; 
        InGameManager.instance.GameStateChangeEvent -= OnGameStateChange;
        Cell.PlacedBuilding = null;
    }
    protected virtual void OnCellChangeOwner(CardManager newOwner)
    {
        if (Cell.OwnerManager != Owner)
        {
            //OnBuildingDestroy();                                
            //Owner.BuildingDestroy(this);
            //Destroy(gameObject);

            DestroyBuilding(this);
        }
    }

    static public void DestroyBuilding(Building_Base building)
    {
        building.OnBuildingDestroy();
        building.Owner.BuildingDestroy(building);
        Destroy(building.gameObject);
    }
}

public enum BuildingType
{
    WarTree,
    BigTree,
    Tree,
    Magnet,
    Mine,
    Nest,
    Reservoir,
    SandTower,
    StoneCastle,
    Tinder
}