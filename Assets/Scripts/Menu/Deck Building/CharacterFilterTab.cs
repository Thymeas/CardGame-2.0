using UnityEngine;
using DG.Tweening;

public class CharacterFilterTab : MonoBehaviour {

    public CharacterAsset Asset;
    public bool ShowAllCharacters = false;

    private CharacterSelectionTabs _tabsScript;
    private readonly float _selectionTransitionTime = 0.5f;
    private readonly Vector3 _initialScale = Vector3.one;
    private readonly float _scaleMultiplier = 1.2f;

    public void TabButtonHandler()
    {
        DeckBuildingScreen.Instance.TabsScript.SelectTab(this, false);
    }

    public void Select(bool instant = false)
    {
        if (instant)
            transform.localScale = _initialScale * _scaleMultiplier;
        else
            transform.DOScale(_initialScale.x * _scaleMultiplier, _selectionTransitionTime);
    }

    public void Deselect(bool instant = false)
    {
        if (instant)
            transform.localScale = _initialScale;
        else
            transform.DOScale(_initialScale, _selectionTransitionTime);
    }
}
