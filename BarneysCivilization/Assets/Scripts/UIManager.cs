using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [SerializeField]
    private GameObject PlayerCardHolder;
    [SerializeField]
    private GameObject EnemyCardHolder;
    [SerializeField]
    private float CardAngle = 20f;
    [SerializeField]
    private Text ResourceText;

    [HideInInspector]
    public Camera MainCam;

    public CardManager playerCardManager;

    public Card_Base SelectCard = null;
    public Card_Base MouseStayCard = null;

    public HexCell SelectCell;
    public List<HexCell> InteractableCells = new List<HexCell>();
    public Vector3 MousePoint;

    [SerializeField]
    private UI_ChooseCardWidget ChooseCardWidget;
    [SerializeField]
    private UI_BattleEndPannel BattleEndPannel;
    public bool IsChoosingCard;

    [Header("Arrangment")]
    [SerializeField]
    private float ReleaseThreshold = 50;
    [SerializeField]
    private float yM = 990;
    [SerializeField]
    private float YOffset = -784;
    private void Awake()
    {
        instance = this;
        MainCam = Camera.main;
    }

    private void Update()
    {
        HexTrace();
        ArrangeCards();
        MouseUp();
    }
    private void Start()
    {
        playerCardManager = InGameManager.instance.CardManagers[UserData.instance.Camp];
        InGameManager.instance.GameStateChangeEvent += OnGameStateChange;
    }

    private void OnGameStateChange()
    {
        switch (InGameManager.CurrentGameState)
        {
            case GameStateType.Decision:
                playerCardManager.UpdateCanMoveCells();
                UpdateInteractableCells(true, null);
                break;
            case GameStateType.BeforeMove:
                UpdateInteractableCells(false, null);
                break;
        }
    }
    private void HexTrace()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit))
        {
            if (SelectCell != null)
            {
                SelectCell.SetHighLightColor(Color.white);
            }
            SelectCell = HexGrid.instance.GetCellByPosition(hit.point);
            if (SelectCell != null)
            {
                UI_ArrowMesh.instance.UpdatePosition(hit.point);
                if (UIManager.instance.InteractableCells!=null && UIManager.instance.InteractableCells.Contains(SelectCell))
                {
                    SelectCell.SetHighLightColor(Color.green);
                }
            }
        }
        else
        {
            if (SelectCell != null)
            {
                SelectCell.SetHighLightColor(Color.white);
                SelectCell = null;
            }
        }

    }

    public void ArrangeCards()
    {
        if (playerCardManager.Cards != null)
        {
            int count = playerCardManager.Cards.Count;

            for (int i = 0; i < count; i++)
            {
                float PosIndex = i - (count - 1f) / 2f;

                Vector3 position;
                Card_Base card = playerCardManager.Cards[i];

                if (card != SelectCard)
                {
                    float angle;

                    angle = PosIndex * CardAngle;
                    position = new Vector3(0, yM, 0);


                    position = Quaternion.Euler(0, 0, angle) * position;
                    position += PlayerCardHolder.transform.position + new Vector3(0, YOffset, 0);

                    card.transform.position = Vector3.Lerp(card.transform.position, position, 0.15f);
                    Quaternion q = Quaternion.Euler(0, 0, angle);
                    card.transform.rotation = Quaternion.Slerp(card.transform.rotation, q, 0.15f);
                }
                else
                {
                    position = Input.mousePosition;
                    position.z = 0;
                    card.transform.position = Vector3.Lerp(card.transform.position, position, 0.2f);
                    card.transform.rotation = Quaternion.Slerp(card.transform.rotation, Quaternion.identity, 0.15f);
                    if (Input.mousePosition.y > ReleaseThreshold)
                    {
                        if (!SelectCard.NoneTargetCard())
                        {
                            UI_ArrowMesh.instance.SetVisibility(true);
                            SelectCard.GetComponent<CardAppearence>().SetVisibility(false);
                        }
                        if (SelectCell != null)
                        {
                            //UI_ArrowMesh.instance.UpdatePosition(MousePoint);
                        }
                    }
                }
            }
        }
    }
    public void MouseDownOnCard(Card_Base card)
    {
        if (SelectCard == null && InGameManager.isGameState(GameStateType.Decision) && card.isActive)
        {
            SelectCard = card;
            UpdateInteractableCells(true, card);
            PlayerController.instance.jobType = PointerJobType.UseCard;
        }
    }
    public void MouseUp()
    {
        if (!Input.GetMouseButton(0) && InGameManager.isGameState(GameStateType.Decision))
        {
            if (PlayerController.instance.jobType == PointerJobType.UseCard)
            {
                PlayerController.instance.jobType = PointerJobType.none;
                UpdateInteractableCells(true, null);
                SelectCard.GetComponent<CardAppearence>().SetVisibility(true);
            }
            if (SelectCard != null)
            {
                if (Input.mousePosition.y > ReleaseThreshold && PlayerController.canControl)
                {
                    int cardID = CardIDSystem.instance.GetCardID(SelectCard.CardName);
                    int hexID = -2;

                    if (SelectCell != null)
                    {
                        playerCardManager.UseCard(SelectCard, PlayerController.instance.SelectCell);
                        hexID = SelectCell.HexIndex;
                    }
                    else if (SelectCard.NoneTargetCard())
                    {
                        playerCardManager.UseCard(SelectCard, PlayerController.instance.SelectCell);
                    }
                    else
                    {
                        SelectCard = null;
                        UI_ArrowMesh.instance.SetVisibility(false);
                        return;
                    }
                    if (UserData.instance.isMultiplayerGame)
                    {
                        // 向服务器汇报使用卡的ID              
                        NetTest.NetManager.instance.ReqUseCard(UserData.instance.UID, cardID, hexID);
                    }
                }
                SelectCard = null;
                UI_ArrowMesh.instance.SetVisibility(false);
            }
       
        }
    }
    public void UpdateInteractableCells(bool ShowGrid, Card_Base card)
    {
        InteractableCells = new List<HexCell>();
        if (ShowGrid)
        {
            if (card != null)
            {
                InteractableCells = card.GetCanUseCells(playerCardManager);
            }
            else
            {
                InteractableCells = playerCardManager.CanMoveCells;
            }
        }
        foreach (HexCell cell in HexGrid.instance.cells)
        {
            print(cell);
            if (InteractableCells != null)
            {
                cell.HighLightCell(InteractableCells.Contains(cell));
            }
            else
            {
                cell.HighLightCell(false);
            }
        }

    }

    public void AddCard(GameObject card, int Camp)
    {
        if (Camp == PlayerController.instance.camp)
        {
            card.transform.SetParent(PlayerCardHolder.transform);
        }
        else
        {
            card.transform.SetParent(EnemyCardHolder.transform);
            card.transform.localPosition = Vector3.zero;
        }
        card.transform.SetAsFirstSibling();
        card.transform.localScale = Vector3.one;
    }

    public void ShowChooseCardPannel(GameObject[] cards,HexCell cell)
    {
        IsChoosingCard = true;
        ChooseCardWidget.StartChooseCard(cards,cell);
    }
    public void ChooseRandomCard()
    {
        ChooseCardWidget.ChooseRandomCard();
    }
    public void CardChoosed(GameObject card , HexCell cell)
    {
        IsChoosingCard = false;
        ChooseCardWidget.ChooseFinish();
        playerCardManager.AddNewCard(card,cell);
    }
    public void EndTurnButton()
    {
        if (PlayerController.canControl)
        {
            InGameManager.instance.EndTurn();
        }
    }

    public void BattleEnd(bool isWin)
    {
        BattleEndPannel.ShowEndPannel(isWin);
    }
}
