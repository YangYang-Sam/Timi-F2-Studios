using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [SerializeField]
    private GameObject PlayerCardHolder;
    [SerializeField]
    private GameObject EnemyCardHolder;

    [SerializeField]
    private Text ResourceText;
    [SerializeField]
    private Vector3 PressedSize;

    [HideInInspector]
    public Camera MainCam;

    public CardManager playerCardManager;

    public Card_Base SelectCard = null;
    public Card_Base MouseStayCard = null;

    public HexCell SelectCell;
    public List<HexCell> InteractableCells = new List<HexCell>();
    public Vector3 MousePoint;
    public Color NormalInteractColor;
    public Color HightlightInteractColor;

    public TextMeshProUGUI CardDeckText;

    [SerializeField]
    private UI_ChooseCardWidget ChooseCardWidget;
    [SerializeField]
    private UI_BattleEndPannel BattleEndPannel;
    public bool IsChoosingCard;

    public bool InteractCellVisibility = true;

    [Header("Arrangment")]
    [SerializeField]
    private float ReleaseThreshold = 50;
    [SerializeField]
    private float CardAngle = 20f;
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
        if (playerCardManager)
        {
            CardDeckText.text = playerCardManager.InGameCardDeck.Count.ToString();
        }

    }
    private void Start()
    {
        playerCardManager = InGameManager.instance.CardManagers[UserData.instance.Camp];
        InGameManager.instance.GameStateChangeEvent += OnGameStateChange;
        InGameManager.instance.LateDecisionEvent += OnLateDecision;
    }

    private void OnLateDecision()
    {
        playerCardManager.UpdateCanMoveCells();
        UpdateInteractableCells(true, null);
    }

    private void OnGameStateChange()
    {
        switch (InGameManager.CurrentGameState)
        {
            case GameStateType.BeforeMove:
                UpdateInteractableCells(false, null);
                CancelSelet();
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
                SelectCell.SetHighLightColor(NormalInteractColor);
            }
            Debug.DrawLine(inputRay.origin, hit.point);
            SelectCell = HexGrid.instance.GetCellByPosition(hit.point);

            if (SelectCell != null)
            {
                UI_ArrowMesh.instance.UpdatePosition(hit.point);
                if (UIManager.instance.InteractableCells != null && UIManager.instance.InteractableCells.Contains(SelectCell))
                {
                    SelectCell.SetHighLightColor(HightlightInteractColor);
                }
            }
        }
        else
        {
            if (SelectCell != null)
            {
                SelectCell.SetHighLightColor(NormalInteractColor);
                SelectCell = null;
            }
        }

    }

    public void ArrangeCards()
    {
        if (playerCardManager && playerCardManager.Cards != null)
        {
            int count = playerCardManager.Cards.Count;

            for (int i = 0; i < count; i++)
            {
                float PosIndex = i - (count - 1f) / 2f;

                Vector3 position;
                Card_Base card = playerCardManager.Cards[i];

                if (card != SelectCard)
                {
                    //float angle;

                    //angle = PosIndex * CardAngle;
                    //position = new Vector3(0, yM, 0);
                    position = PlayerCardHolder.transform.position + PosIndex * (CardAngle / count) * Vector3.right;

                    //position = Quaternion.Euler(0, 0, angle) * position;
                    //position += PlayerCardHolder.transform.position + new Vector3(0, YOffset, 0);

                    //card.transform.position = Vector3.Lerp(card.transform.position, position, 0.15f);
                    //Quaternion q = Quaternion.Euler(0, 0, angle);
                    card.transform.position = Vector3.Lerp(card.transform.position, position, 0.15f);

                    //card.transform.rotation = Quaternion.Slerp(card.transform.rotation, q, 0.15f);
                    card.transform.localScale = Vector3.Lerp(card.transform.localScale, Vector3.one, 0.1f);
                }
                else
                {
                    position = Input.mousePosition;
                    position.z = 0;
                    card.transform.position = Vector3.Lerp(card.transform.position, position, 0.2f);
                    card.transform.rotation = Quaternion.Slerp(card.transform.rotation, Quaternion.identity, 0.15f);
                    card.transform.localScale = Vector3.Lerp(card.transform.localScale, PressedSize, 0.1f);
                    card.transform.SetAsLastSibling();
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
    public event System.Action<Card_Base> PlayerUseCardEvent;
    public void MouseUp()
    {
        if (!Input.GetMouseButton(0))
        {
            if (InGameManager.isGameState(GameStateType.Decision))
            {
                if (PlayerController.instance.jobType == PointerJobType.UseCard)
                {
                    PlayerController.instance.jobType = PointerJobType.none;
                    UpdateInteractableCells(true, null);
                    if (SelectCard != null)
                    {
                        SelectCard.GetComponent<CardAppearence>().SetVisibility(true);
                    }
                }
                if (SelectCard != null)
                {
                    if (Input.mousePosition.y > ReleaseThreshold && PlayerController.canControl)
                    {
                        if (!UserData.instance.isMultiplayerGame || NetTest.NetManager.socket.Connected)
                        {
                            int cardID = CardIDSystem.instance.GetCardID(SelectCard.CardName);
                            int hexID = -2;
                            //Debug.DrawLine(SelectCell.transform.position, SelectCell.transform.position + Vector3.up * 20, Color.red);
                            if (SelectCell != null)
                            {
                                playerCardManager.UseCard(SelectCard, SelectCell);
                                hexID = SelectCell.HexIndex;
                            }
                            else if (SelectCard.NoneTargetCard())
                            {
                                playerCardManager.UseCard(SelectCard, SelectCell);
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
                            if (PlayerUseCardEvent != null)
                            {
                                PlayerUseCardEvent(SelectCard);
                            }
                        }
                        else
                        {
                            UI_Warning.instance.ShowWarningText("断线重连中，请稍后操作");
                        }
                    }
                    CancelSelet();
                }

            }
        }
    }

    public void CancelSelet()
    {
        SelectCard = null;
        UI_ArrowMesh.instance.SetVisibility(false);
        for (int i = 0; i < playerCardManager.Cards.Count; i++)
        {
            playerCardManager.Cards[i].transform.SetAsFirstSibling();
        }
    }
    public void SwitchRaceTraitPannel()
    {
        UI_RaceTrait.instance.gameObject.SetActive(!UI_RaceTrait.instance.gameObject.activeSelf);
    }
    public void SwitchInteractVisible()
    {
        InteractCellVisibility = !InteractCellVisibility;
        UpdateInteractableCells(true, null);
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
        bool showGrid = InteractCellVisibility || card;
        foreach (HexCell cell in HexGrid.instance.cells)
        {
            if (InteractableCells != null)
            {
                cell.HighLightCell(InteractableCells.Contains(cell) && showGrid);
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

    public event System.Action StartChooseCardEvent;
    public void ShowChooseCardPannel(GameObject[] cards,HexCell cell)
    {
        IsChoosingCard = true;
        ChooseCardWidget.StartChooseCard(cards,cell);
        if (StartChooseCardEvent != null)
        {
            StartChooseCardEvent();
        }
    }
    public void ChooseRandomCard()
    {
        ChooseCardWidget.ChooseRandomCard();
    }
    public event System.Action ChooseCardFinish;
    public void CardChoosed(GameObject card , HexCell cell)
    {
        IsChoosingCard = false;
        ChooseCardWidget.ChooseFinish();
        playerCardManager.AddNewCard(card,cell);
        if (ChooseCardFinish != null)
        {
            ChooseCardFinish();
        }
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
