using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DeckInScrollList : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    public Image AvatarImage;
    public Text NameText;
    public GameObject DeleteDeckButton;
    public DeckInfo savedDeckInfo;

    public void Awake()
    {
        DeleteDeckButton.SetActive(false);
    }

    public void EditThisDeck()
    {
        DeckBuildingScreen.Instance.HideScreen();
        DeckBuildingScreen.Instance.BuilderScript.BuildADeckFor(savedDeckInfo.Character);
        DeckBuildingScreen.Instance.BuilderScript.DeckName.text = savedDeckInfo.DeckName;
        foreach (CardAsset asset in savedDeckInfo.Cards)
            DeckBuildingScreen.Instance.BuilderScript.AddCard(asset);
        DecksStorage.Instance.AllDecks.Remove(savedDeckInfo);
        DeckBuildingScreen.Instance.TabsScript.SetClassOnClassTab(savedDeckInfo.Character);
        DeckBuildingScreen.Instance.CollectionBrowserScript.ShowCollectionForDeckBuilding(savedDeckInfo.Character);
        DeckBuildingScreen.Instance.ShowScreenForDeckBuilding();
    }

    public void DeleteThisDeck()
    {
        DecksStorage.Instance.AllDecks.Remove(savedDeckInfo);
        Destroy(gameObject);
    }

    public void ApplyInfo (DeckInfo deckInfo)
    {
        AvatarImage.sprite = deckInfo.Character.AvatarImage;
        NameText.text = deckInfo.DeckName;
        savedDeckInfo = deckInfo;
    }

    public void OnPointerEnter(PointerEventData data)
    {
        DeleteDeckButton.SetActive(true);
    }

    public void OnPointerExit(PointerEventData data)
    {
        DeleteDeckButton.SetActive(false);
    }
}
