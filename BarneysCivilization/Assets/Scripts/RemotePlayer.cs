using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NetTest;
public class RemotePlayer : MonoBehaviour
{
    public CardManager cardManager;
    public string UID;
    public int CardID;
    public int HexID;
    public bool UseCard;
    void Start()
    {
        cardManager = GetComponent<CardManager>();
        CsResManager.UseCardEvent += OnUseCard;
    }
    private void Update()
    {
        if (UseCard)
        {
            UseCard = false;
            if (cardManager.UID == UID)
            {
                GameObject prefab = CardIDSystem.instance.GetCardByID(CardID);
                GameObject cardObj = Instantiate(prefab);
                Card_Base card = cardObj.GetComponent<Card_Base>();
                HexCell cell = HexGrid.instance.cells[HexID];
                cardManager.UseCard(card, cell);
            }
        }  
    }
    private void OnUseCard(string uid, int cardID, int hexID)
    {
        UseCard = true;
        UID = uid;
        HexID = hexID;
        CardID = cardID;
    }
}
