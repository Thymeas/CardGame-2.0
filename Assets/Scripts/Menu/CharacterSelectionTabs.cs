using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionTabs : MonoBehaviour 
{
    public List<CharacterFilterTab> Tabs = new List<CharacterFilterTab>();
    public CharacterFilterTab ClassTab;
    public CharacterFilterTab NeutralTabWhenCollectionBrowsing;
    private int _currentIndex;

    public void SelectTab(CharacterFilterTab tab, bool instant)
    {
        int newIndex = Tabs.IndexOf(tab);

        if (newIndex == _currentIndex)
            return;

        _currentIndex = newIndex;

        foreach (CharacterFilterTab t in Tabs)
        {
            if (t != tab)
                t.Deselect(instant);
        }

        tab.Select(instant);
        DeckBuildingScreen.Instance.CollectionBrowserScript.Asset = tab.Asset;
        DeckBuildingScreen.Instance.CollectionBrowserScript.IncludeAllCharacters = tab.ShowAllCharacters;
    }

    public void SetClassOnClassTab(CharacterAsset asset)
    {
        ClassTab.Asset = asset;
        ClassTab.GetComponentInChildren<Text>().text = asset.name;
    }
}
