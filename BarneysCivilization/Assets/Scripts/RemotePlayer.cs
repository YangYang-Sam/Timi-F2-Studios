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
            RemoteUseCard(UID, CardID, HexID);
        }  
    }
    public void RemoteUseCard(string uid, int cardID, int hexID)
    {
        if (cardManager.UID == uid)
        {
            GameObject prefab = CardIDSystem.instance.GetCardByID(cardID);
            GameObject cardObj = Instantiate(prefab);
            Card_Base card = cardObj.GetComponent<Card_Base>();
            HexCell cell = null;
            if (hexID >= 0)
            {
                cell = HexGrid.instance.cells[hexID];
            }
            cardManager.UseCard(card, cell);
        }
    }
    private void OnUseCard(string uid, int cardID, int hexID)
    {
        print("User:" + uid + " use " + cardID + " on " + hexID);
        UseCard = true;
        UID = uid;
        HexID = hexID;
        CardID = cardID;
    }
}
