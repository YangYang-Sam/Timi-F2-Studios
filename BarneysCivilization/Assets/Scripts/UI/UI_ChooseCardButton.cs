using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ChooseCardButton : MonoBehaviour
{
    public GameObject cardPrefab;
    [SerializeField]
    private CardAppearence cardAppearence;
    public HexCell Cell;
    public void ClickButton()
    {
        ChooseCard();
    }

    public void SetCard(GameObject _cardPrefab, HexCell cell)
    {
        cardPrefab = _cardPrefab;
        Card_Base card = cardPrefab.GetComponent<Card_Base>();
        cardAppearence.RefreshAppearence(card);
        Cell = cell;
    }
    public void ChooseCard()
    {
        UIManager.instance.CardChoosed(cardPrefab, Cell);
    }
}
