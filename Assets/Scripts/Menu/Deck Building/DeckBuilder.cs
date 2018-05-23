using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckBuilder : MonoBehaviour 
{
    public GameObject CardNamePrefab;
    public Transform Content;
    public InputField DeckName;

    public int SameCardLimit = 2;
    public int AmountOfCardsInDeck = 10;

    public GameObject DeckCompleteFrame;

    private readonly List<CardAsset> _deckList = new List<CardAsset>();
    private readonly Dictionary<CardAsset, CardNameRibbon> _ribbons = new Dictionary<CardAsset, CardNameRibbon>();

    public bool InDeckBuildingMode{ get; set;}
    private CharacterAsset _buildingForCharacter;

    void Awake()
    {
        DeckCompleteFrame.GetComponent<Image>().raycastTarget = false;
    }

    public void AddCard(CardAsset asset)
    {
        if (!InDeckBuildingMode)
            return;
        
        if (_deckList.Count == AmountOfCardsInDeck)
            return;

        int count = NumberOfThisCardInDeck(asset);

        int limitOfThisCardInDeck = SameCardLimit;

        if (asset.OverrideLimitOfThisCardInDeck > 0)
            limitOfThisCardInDeck = asset.OverrideLimitOfThisCardInDeck;

        if (count < limitOfThisCardInDeck)
        {
            _deckList.Add(asset);
            CheckDeckCompleteFrame();
            count++;

            if (_ribbons.ContainsKey(asset))
                _ribbons[asset].SetQuantity(count);
            else
            {
                GameObject cardName = Instantiate(CardNamePrefab, Content) as GameObject;
                cardName.transform.SetAsLastSibling();
                cardName.transform.localScale = Vector3.one;
                CardNameRibbon ribbon = cardName.GetComponent<CardNameRibbon>();
                ribbon.ApplyAsset(asset, count);
                _ribbons.Add(asset, ribbon);
            }
        }
    }

    void CheckDeckCompleteFrame()
    {
        DeckCompleteFrame.SetActive(_deckList.Count == AmountOfCardsInDeck);
    }

    public int NumberOfThisCardInDeck (CardAsset asset)
    {
        int count = 0;
        foreach (CardAsset ca in _deckList)
        {
            if (ca == asset)
                count++;
        }
        return count;
    }

    public void RemoveCard(CardAsset asset)
    {
        CardNameRibbon ribbonToRemove = _ribbons[asset];
        ribbonToRemove.SetQuantity(ribbonToRemove.Quantity-1);

        if (NumberOfThisCardInDeck(asset) == 1)
        {            
            _ribbons.Remove(asset);
            Destroy(ribbonToRemove.gameObject);
        }

        _deckList.Remove(asset);
        CheckDeckCompleteFrame();
        DeckBuildingScreen.Instance.CollectionBrowserScript.UpdateQuantitiesOnPage();
    }

    public void BuildADeckFor(CharacterAsset asset)
    {
        InDeckBuildingMode = true;
        _buildingForCharacter = asset;
        while (_deckList.Count>0)
        {
            RemoveCard(_deckList[0]);
        }

        DeckBuildingScreen.Instance.TabsScript.SetClassOnClassTab(asset);
        DeckBuildingScreen.Instance.CollectionBrowserScript.ShowCollectionForDeckBuilding(asset);

        CheckDeckCompleteFrame();
        DeckName.text = "";
    }

    public void DoneButtonHandler()
    {
        DeckInfo deckToSave = new DeckInfo(_deckList, DeckName.text, _buildingForCharacter);
        DecksStorage.Instance.AllDecks.Add(deckToSave);
        DecksStorage.Instance.SaveDecksIntoPlayerPrefs();
        DeckBuildingScreen.Instance.ShowScreenForCollectionBrowsing();
    }

    void OnApplicationQuit()
    {
        DoneButtonHandler();
    }
}
