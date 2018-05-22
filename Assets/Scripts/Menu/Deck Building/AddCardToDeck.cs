using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class AddCardToDeck : MonoBehaviour {

    public Text QuantityText;
    private float InitialScale;
    private float scaleFactor = 1.1f;
    private CardAsset cardAsset;

    void Awake()
    {
        InitialScale = transform.localScale.x;
    }

    public void SetCardAsset(CardAsset asset) { cardAsset = asset; } 

    void OnMouseDown()
    {
        CardAsset asset = GetComponent<OneCardManager>().CardAsset;
        if (asset == null)
            return;

        if (CardCollection.Instance.QuantityOfEachCard[cardAsset] - DeckBuildingScreen.Instance.BuilderScript.NumberOfThisCardInDeck(cardAsset) > 0)
        {
            DeckBuildingScreen.Instance.BuilderScript.AddCard(asset);
            UpdateQuantity();
        }
    }

    void OnMouseEnter()
    {        
        if (CraftingScreen.Instance.Visible)
            return;

        transform.DOScale(InitialScale*scaleFactor, 0.5f);
    }

    void OnMouseExit()
    {
        transform.DOScale(InitialScale, 0.5f);
    }

    void Update () 
    {
        if(Input.GetMouseButtonDown (1))
            OnRightClick();
    }

    void OnRightClick()
    {
        if (CraftingScreen.Instance.Visible)
            return;

        Ray clickPoint = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitPoint;
        
        if (Physics.Raycast(clickPoint, out hitPoint))
        {
            if (hitPoint.collider == this.GetComponent<Collider>())
                CraftingScreen.Instance.ShowCraftingScreen(GetComponent<OneCardManager>().CardAsset);
        }
    }

    public void UpdateQuantity()
    {
        int quantity = CardCollection.Instance.QuantityOfEachCard[cardAsset];

        if (DeckBuildingScreen.Instance.BuilderScript.InDeckBuildingMode && DeckBuildingScreen.Instance.ShowReducedQuantitiesInDeckBuilding)
            quantity -= DeckBuildingScreen.Instance.BuilderScript.NumberOfThisCardInDeck(cardAsset);
        
        QuantityText.text = "X" + quantity;
    }
}
