using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class LineObjectPool
{
    public void FreeAllLines()
    {
        foreach (var line in ActiveLineRenderObjects)
        {
            line.SetActive(false);
            FreeLineRenderObjects.Add(line);
        }
        ActiveLineRenderObjects.Clear();
    }

    public LineRenderer NextFreeLineRenderer()
    {

        GameObject lineRendererObject = null;
        LineRenderer lineRenderer = null;

        if (FreeLineRenderObjects.Count > 0)
        {
            lineRendererObject = FreeLineRenderObjects[0];
            FreeLineRenderObjects.RemoveAt(0);
            lineRendererObject.SetActive(true);
            lineRenderer = lineRendererObject.GetComponent<LineRenderer>();
        }
        else
        {
            lineRendererObject = new GameObject();
            lineRenderer = lineRendererObject.AddComponent<LineRenderer>();
            InitLineRenderer(lineRenderer);
        }

        if (lineRendererObject)
        {
            ActiveLineRenderObjects.Add(lineRendererObject);
        }

        return lineRenderer;
    }

    private void InitLineRenderer(LineRenderer lineRenderer)
    {
        lineRenderer.positionCount = 2;
        lineRenderer.material = new Material(Shader.Find("Standard"));
    }


    private List<GameObject> ActiveLineRenderObjects = new List<GameObject>();
    private List<GameObject> FreeLineRenderObjects = new List<GameObject>();
}

public class CardManager : MonoBehaviour
{
    public int camp;

    [Header("Cards")]
    public List<Card_Base> Cards = new List<Card_Base>();
    public List<Card_Base> InGameCardDeck = new List<Card_Base>();
    private List<Card_Base> UsedCardsDeck = new List<Card_Base>();
    private List<Card_Base> VanishedCardsDeck = new List<Card_Base>();
    public GameObject[] StartDeck;

    public List<Unit_Base> Units = new List<Unit_Base>();
    public List<HexCell> OccupiedCells = new List<HexCell>();
    public List<HexCell> CanMoveCells = new List<HexCell>();
    public List<HexCell> NewOccupiedCells = new List<HexCell>();
    public List<Building_Base> Buildings = new List<Building_Base>();
    private List<HexCell> OccupiedBoundaryCells = new List<HexCell>();
    private LineObjectPool LinePool = new LineObjectPool();

    public int MaxCardAmount = 6;
    public int DrawCardAmount = 3;

    public int ResourceAmount;
    public int TempResourceAmount;
    public int BuyUnitTimes;
    public int ActionPoint;
    public int UnitMoveSpeed=1;

    public RaceInfo Race;
    public RaceTrait_Base RaceTrait;
    public Core PlayerCore;
    public bool isLost = false;

    public int StartCell;


    public GameObject GetUnitPrefab()
    {
        return Race.UnitPrefab;
    }
    public GameObject GetCorePrefab()
    {
        return Race.CorePrefab;
    }
    public int GetTotalResource()
    {
        return TempResourceAmount + ResourceAmount;
    }
    private void Start()
    {
        if (camp != PlayerController.instance.camp)
        {
            Race = ArtResourceManager.instance.RaceInfos[Random.Range(0, 2)];
        }
        InGameManager.instance.RegistCardManager(this);
    }
    public void ChooseRace(int index)
    {
        Race = ArtResourceManager.instance.RaceInfos[index];
    }
    public void GameStart()
    {
        InGameManager.instance.GameStateChangeEvent += OnGameStateChange;
        GameObject coreObj = Instantiate(GetCorePrefab());
        PlayerCore = coreObj.GetComponent<Core>();
        HexCell startCell = HexGrid.instance.cells[StartCell];
        PlayerCore.transform.position = startCell.transform.position;
        PlayerCore.InitCore(startCell, this);

        GameObject traitObj = Instantiate(Race.RaceTraitPrefab);
        RaceTrait = traitObj.GetComponent<RaceTrait_Base>();
        RaceTrait.InitRaceTrait(this);

        Unit_Base unit = CreateNewUnit(startCell, 1);
        startCell.ChangeOwner(unit);

        for (int i = 0; i < StartDeck.Length; i++)
        {
            AddNewCard(StartDeck[i]);
        }
    }
 
    public Unit_Base CreateNewUnit(HexCell cell,int health)
    {
        GameObject g = Instantiate(GetUnitPrefab());
        Unit_Base unit = g.GetComponent<Unit_Base>();
        unit.UnitCreated(cell, this, health);
        Units.Add(unit);
        unit.transform.position = cell.transform.position;
        
        return unit;
    }
   
    public void UnitRemoved(Unit_Base unit)
    {
        Units.Remove(unit);
    }
    private void OnGameStateChange()
    {
        if (isLost)
        {
            return;
        }
        switch (InGameManager.CurrentGameState)
        {
            case GameStateType.Decision:
                ActionPoint = 1;
                BuyUnitTimes = 1;
                UnitMoveSpeed = 1;
                for (int i = 0; i < DrawCardAmount; i++)
                {
                    DrawCard();
                }

                if (PlayerCore.Cell.PlacedUnits.Count == 0)
                {
                    CreateNewUnit(PlayerCore.Cell, ResourceAmount + TempResourceAmount);
                }
                else
                {
                    PlayerCore.Cell.PlacedUnits[0].ChangeHealth(ResourceAmount + TempResourceAmount);
                }

                TempResourceAmount = 0;

                // StartChooseNewCard
                if (NewOccupiedCells.Count > 0 && camp==PlayerController.instance.camp)
                {
                    ChooseCardCoroutine= StartCoroutine(ChooseCardProcess());
                }        
                break;
            case GameStateType.BeforeMove:

                if (camp == PlayerController.instance.camp)
                {
                    if (UIManager.instance.IsChoosingCard)
                    {
                        StopCoroutine(ChooseCardCoroutine);
                        ChooseRandomCard();
                    }
                }
                else if (NewOccupiedCells.Count > 0)
                {
                    ChooseRandomCard();
                }                           
                
                AbandonAllCards();
                break;
        }
    }
    Coroutine ChooseCardCoroutine;
    private IEnumerator ChooseCardProcess()
    {   
        while(NewOccupiedCells.Count>0)
        {
            while (UIManager.instance.IsChoosingCard)
            {
                yield return null;
            }
            yield return new WaitForSeconds(0.5f);
            HexCell cell = NewOccupiedCells[0];
            NewOccupiedCells.RemoveAt(0);
            UIManager.instance.ShowChooseCardPannel(Race.cards[(int)cell.CellType].cards,cell);
        }     
     
    }
    private void ChooseRandomCard()
    {
        if (camp == PlayerController.instance.camp)
        {
            UIManager.instance.ChooseRandomCard();
        } 
        while (NewOccupiedCells.Count > 0)
        { 
            HexCell cell = NewOccupiedCells[0];
            NewOccupiedCells.RemoveAt(0);
            int randomIndex = Random.Range(0, Race.cards[(int)cell.CellType].cards.Length);
            GameObject g = Race.cards[(int)cell.CellType].cards[randomIndex];
            AddNewCard(g);
            if(camp == PlayerController.instance.camp)
            {
                UIManager.instance.CardChoosed(g);
            }
        }       
    }
    public List<HexCell> GetAllNearbyCells()
    {
        List<HexCell> cells=new List<HexCell>();
        foreach (HexCell cell in OccupiedCells)
        {
            if (!cells.Contains(cell))
            {
                cells.Add(cell);
            }
            foreach (HexCell neighbor in cell.NearbyCells)
            {
                if (!cells.Contains(neighbor))
                {
                    cells.Add(neighbor);
                }
            }
        }
        return cells;
    }
    public void UpdateCanMoveCells()
    {
        CanMoveCells.Clear();
        foreach (HexCell cell in OccupiedCells)
        {
            if (!CanMoveCells.Contains(cell))
            {
                CanMoveCells.Add(cell);
            }
            foreach (HexCell neighbor in cell.NearbyCells)
            {
                if (!CanMoveCells.Contains(neighbor))
                {
                    CanMoveCells.Add(neighbor);
                }
            }
        }
        //List<HexCell> CheckedCells = new List<HexCell>();
        //List<HexCell> ProcessingCells = new List<HexCell>();
        //ProcessingCells.Add(PlayerCore.Cell);
        //while (ProcessingCells.Count > 0)
        //{
        //    foreach(HexCell cell in ProcessingCells[0].NearbyCells)
        //    {
        //        if (cell.CanPlaceUnit(this))
        //        {
        //            if (!CanBuildCells.Contains(cell))
        //            {
        //                CanBuildCells.Add(cell);
        //            }                  
        //        }
        //        else if(cell.OwnerUnit!=null && cell.OwnerUnit.Owner==this && !ProcessingCells.Contains(cell) && !CheckedCells.Contains(cell))
        //        {
        //            ProcessingCells.Add(cell);
        //        }
        //    }
        //    CheckedCells.Add(ProcessingCells[0]);
        //    ProcessingCells.RemoveAt(0);
        //}
        //print("Canbuild: " + CanBuildCells.Count);
    }
 
    public void DrawCards(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            DrawCard();
        }
    }
    public void DrawCard()
    {
        if (InGameCardDeck.Count == 0)
        {
            RefreshInGameCardDeck();
        }

        if (InGameCardDeck.Count == 0)
        {
            return;
        }
        Card_Base card = InGameCardDeck[0];

        card.isActive = true;

        Cards.Add(card);
        card.gameObject.SetActive(true);
        UIManager.instance.AddCard(card.gameObject, camp);
        InGameCardDeck.RemoveAt(0);
    }

    public void AbandonAllCards()
    {
        for (int i = Cards.Count-1; i >=0 ; i--)
        {
            AddToUsedCards(Cards[i]);
        } 
    }
    public void RefreshInGameCardDeck()
    {
        foreach (Card_Base card in UsedCardsDeck)
        {
            InGameCardDeck.Add(card);
        }
        UsedCardsDeck.Clear();
        ShuffleCards();
    }
    public void UseCard(Card_Base card, HexCell cell)
    {
        if (card.CanUseCard(this,cell))
        {
            card.CardEffect(this,cell);
            AddToUsedCards(card);
            ActionPoint -= card.ActionPointCost();

            // 向服务器汇报使用卡的ID
            CardIDSystem.instance.GetCardID(card.CardName);
        }
    }
    public void AddToUsedCards(Card_Base card)
    {
        UsedCardsDeck.Add(card);
        card.gameObject.SetActive(false);

        card.isActive = false;

        Cards.Remove(card);
    }
    public void ShuffleCards()
    {
        InGameCardDeck = Fisher_Yates_CardDeck_Shuffle(InGameCardDeck);
    }
 
    public void OccupyCell(Unit_Base unit, HexCell cell)
    {
        OccupiedCells.Add(cell);
        NewOccupiedCells.Add(cell);
        UpdateOccupiedBoundaryLines();
    }
    public void AddNewCard(GameObject CardPrefab)
    {
        Card_Base card = Instantiate(CardPrefab).GetComponent<Card_Base>();
        UIManager.instance.AddCard(card.gameObject, camp);
        AddToUsedCards(card);
    }
    public void LostCell(HexCell cell)
    {
        OccupiedCells.Remove(cell);
        UpdateOccupiedBoundaryLines();
    }
    public void SetUnitMoveTo(HexCell TargetCell)
    {
        if (InGameManager.isGameState(GameStateType.Decision) && !isLost)
        {
            UpdateCanMoveCells();
            if (CanMoveCells.Contains(TargetCell))
            {
                BattleManager.instance.MoveAllUnitsToCell(this, TargetCell, UnitMoveSpeed);
               
            }
        }
    }
    public void CreateBuilding(GameObject prefab,HexCell cell)
    {
        GameObject g = Instantiate(prefab, cell.transform.position, Quaternion.identity);
        Building_Base building = g.GetComponent<Building_Base>();
        building.OnCreated(cell, this);
    }
    public void BuildingDestroy(Building_Base building)
    {
        Buildings.Remove(building);
    }
    public void CampLost(CardManager destroyer)
    {
        isLost = true;
        for (int i = OccupiedCells.Count-1; i >= 0; i--)
        {
            OccupiedCells[i].ChangeOwner(null);
        }

        for (int i = Units.Count-1; i >= 0; i--)
        {
            Units[i].TakeDamage(9999999, null);
        }

        InGameManager.instance.CampLost(this);
    }
    public static List<Card_Base> Fisher_Yates_CardDeck_Shuffle(List<Card_Base> aList)
    {

        System.Random _random = new System.Random();

        Card_Base myGO;

        int n = aList.Count;
        for (int i = 0; i < n; i++)
        {
            // NextDouble returns a random number between 0 and 1.
            // ... It is equivalent to Math.random() in Java.
            int r = i + (int)(_random.NextDouble() * (n - i));
            myGO = aList[r];
            aList[r] = aList[i];
            aList[i] = myGO;
        }

        return aList;
    }

    private void UpdateOccupiedBoundaryCells()
    {
        OccupiedBoundaryCells.Clear();

        foreach (var hexCell in OccupiedCells)
        {
            hexCell.UpdateEdgeIndexesOnBoundary();
            if (hexCell.EdgeIndexesOnBoundary.Count > 0)
            {
                OccupiedBoundaryCells.Add(hexCell);
            }
        }
    }

    private void UpdateOccupiedBoundaryLines()
    {
        UpdateOccupiedBoundaryCells();

        LinePool.FreeAllLines();

        foreach (var hexCell in OccupiedBoundaryCells)
        {
            foreach (var EdgeIndex in hexCell.EdgeIndexesOnBoundary)
            {
                var line = LinePool.NextFreeLineRenderer();
                Vector3 center = hexCell.transform.position;
                Color lineColor = new Color();
                switch (camp)
                {
                    case 0: lineColor = Color.cyan; break;
                    case 1: lineColor = Color.magenta; break;
                    case 2: lineColor = Color.blue; break;
                }

                line.startColor = lineColor;
                line.endColor = lineColor;

                line.material.SetColor("_Color", lineColor);

                float s1 = 0.94f;
                float s2 = 0.08f;
                Vector3 pos1 = (1.0f - s1) * center + s1 * (center + HexMetrics.corners[EdgeIndex]);
                Vector3 pos2 = (1.0f - s1) * center + s1 * (center + HexMetrics.corners[EdgeIndex + 1]);
                Vector3 extent = pos1 - pos2;
                pos1 = pos1 + extent * s2;
                pos2 = pos2 - extent * s2;

                line.SetPosition(0, pos1);
                line.SetPosition(1, pos2);
            }
        }
    }
}
