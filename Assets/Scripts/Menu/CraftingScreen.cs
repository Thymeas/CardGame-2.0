using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class RarityTradingCost
{
    public RarityOptions Rarity;
    public int CraftCost;
    public int DisenchantOutcome;
}

public class CraftingScreen : MonoBehaviour {
  
    public static CraftingScreen Instance;

    public GameObject Content;

    public GameObject CreatureCard;
    public GameObject SpellCard;

    public Text CraftText;
    public Text DisenchantText;
    public Text QuantityText;

    public RarityTradingCost[] TradingCostsArray;

    public bool Visible{get{ return Content.activeInHierarchy;}}

    private CardAsset currentCard;
    private Dictionary <RarityOptions, RarityTradingCost> TradingCosts = new Dictionary<RarityOptions, RarityTradingCost>();

    void Awake()
    {
        Instance = this;
        foreach (RarityTradingCost cost in TradingCostsArray)
            TradingCosts.Add(cost.Rarity, cost);
    }

    public void ShowCraftingScreen(CardAsset cardToShow)
    {
        currentCard = cardToShow;
        GameObject cardObject;
        if (currentCard.TypeOfCard == TypesOfCards.Creature)
        {
            cardObject = CreatureCard;
            CreatureCard.SetActive(true);
            SpellCard.SetActive(false);
        }
        else
        {
            cardObject = SpellCard;
            CreatureCard.SetActive(false);
            SpellCard.SetActive(true);
        }
        OneCardManager manager = cardObject.GetComponent<OneCardManager>();
        manager.CardAsset = cardToShow;
        manager.ReadCardFromAsset();

        CraftText.text = "Craft this card for " + TradingCosts[cardToShow.Rarity].CraftCost + " dust";
        DisenchantText.text = "Disenchant to get " + TradingCosts[cardToShow.Rarity].DisenchantOutcome + " dust";

        ShopManager.Instance.DustHUD.SetActive(true);
        UpdateQuantityOfCurrentCard();
        Content.SetActive(true);
    }

    public void UpdateQuantityOfCurrentCard()
    {
        int AmountOfThisCardInYourCollection = CardCollection.Instance.QuantityOfEachCard[currentCard];
        QuantityText.text = "You have " + AmountOfThisCardInYourCollection + " of these";
        DeckBuildingScreen.Instance.CollectionBrowserScript.UpdatePage();
    }

    public void HideCraftingScreen()
    {
        ShopManager.Instance.DustHUD.SetActive(false);
        
        Content.SetActive(false);
    }

    public void CraftCurrentCard()
    {
        if (currentCard.Rarity != RarityOptions.Basic)
        {
            if (ShopManager.Instance.Dust >= TradingCosts[currentCard.Rarity].CraftCost)
            {
                ShopManager.Instance.Dust -= TradingCosts[currentCard.Rarity].CraftCost;
                CardCollection.Instance.QuantityOfEachCard[currentCard]++;
                UpdateQuantityOfCurrentCard();
            }
        }
    }

    public void DisenchantCurrentCard()
    {
        if (currentCard.Rarity != RarityOptions.Basic)
        {
            if (CardCollection.Instance.QuantityOfEachCard[currentCard] > 0)
            {
                CardCollection.Instance.QuantityOfEachCard[currentCard]--;
                ShopManager.Instance.Dust += TradingCosts[currentCard.Rarity].DisenchantOutcome;
                UpdateQuantityOfCurrentCard();
                
                foreach(DeckInfo info in DecksStorage.Instance.AllDecks)
                {
                    while (info.NumberOfThisCardInDeck(currentCard) > CardCollection.Instance.QuantityOfEachCard[currentCard])
                    {
                        info.Cards.Remove(currentCard);
                    }
                }
                
                while (DeckBuildingScreen.Instance.BuilderScript.InDeckBuildingMode &&
                       DeckBuildingScreen.Instance.BuilderScript.NumberOfThisCardInDeck(currentCard) > CardCollection.Instance.QuantityOfEachCard[currentCard])
                {
                    DeckBuildingScreen.Instance.BuilderScript.RemoveCard(currentCard);
                }
            }
        }
    }
}
