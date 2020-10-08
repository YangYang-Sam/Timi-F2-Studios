using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ChooseCardWidget : MonoBehaviour
{
    public GameObject cardButtonObj;
    [SerializeField]
    private Transform cardHolderTransform;
    [SerializeField]
    private Text HintText;
    public HexCell Cell;

    private GameObject[] CardButtons;

    public void StartChooseCard(GameObject[] cards, HexCell cell)
    {
        Cell = cell;
        gameObject.SetActive(true);
        CardButtons = new GameObject[cards.Length];
        for (int i = 0; i < cards.Length; i++)
        {
            Card_Base card = cards[i].GetComponent<Card_Base>();
            GameObject g = Instantiate(cardButtonObj, cardHolderTransform);
            g.GetComponent<UI_ChooseCardButton>().SetCard(cards[i], cell);
            CardButtons[i] = g;
        }
        HintText.text = "你占领了" + ArtResourceManager.ConverCellTypeText(cell.CellType) + "\n选择一张卡牌";
    }
    public void ChooseRandomCard()
    {
        GameObject cardButton = CardButtons[Random.Range(0, CardButtons.Length)];
        UIManager.instance.CardChoosed(cardButton.GetComponent<UI_ChooseCardButton>().cardPrefab, Cell);
    }
    public void ChooseFinish()
    {
        gameObject.SetActive(false);
        for (int i = 0; i < CardButtons.Length; i++)
        {
            Destroy(CardButtons[i]);
        }
    }

}
