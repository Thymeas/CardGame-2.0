using UnityEngine;
using UnityEngine.UI;

public class HeroInfoPanel : MonoBehaviour {

    public PlayerPortraitVisual Portrait;
    public Button PlayButton;
    public Button BuildDeckButton;
    public PortraitMenu SelectedPortrait{ get; set;}
    public DeckIcon SelectedDeck{ get; set;}

    void Awake()
    {
        OnOpen();
    }
        
    public void OnOpen()
    {
        SelectCharacter(null);
        SelectDeck(null);
    }

    public void SelectCharacter(PortraitMenu menuPortrait)
    {
        if (menuPortrait == null || SelectedPortrait == menuPortrait)
        {
            Portrait.gameObject.SetActive(false);
            SelectedPortrait = null;
            if (BuildDeckButton!=null)
                BuildDeckButton.interactable = false;
        }
        else
        {            
            Portrait.charAsset = menuPortrait.asset;
            Portrait.ApplyLookFromAsset();
            Portrait.gameObject.SetActive(true);
            SelectedPortrait = menuPortrait;
            if (BuildDeckButton!=null)
                BuildDeckButton.interactable = true;
        }
    }

    public void SelectDeck(DeckIcon deck)
    {
        if (deck == null || SelectedDeck == deck || !deck.DeckInformation.IsComplete())
        {
            Portrait.gameObject.SetActive(false);
            SelectedDeck = null;

            if (PlayButton!=null)
                PlayButton.interactable = false;
        }
        else
        {           
            Portrait.charAsset = deck.DeckInformation.Character;
            Portrait.ApplyLookFromAsset();
            Portrait.gameObject.SetActive(true);
            SelectedDeck = deck;
            BattleStartInfo.SelectedDeck = SelectedDeck.DeckInformation;

            if (PlayButton!=null)
                PlayButton.interactable = true;
        }
    }
    public void GoToDeckbuilding()
    {
        if (SelectedPortrait == null)
            return;

        DeckBuildingScreen.Instance.BuildADeckFor(SelectedPortrait.asset);
    }
        
}
