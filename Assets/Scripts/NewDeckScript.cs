using UnityEngine;

public class NewDeckScript : MonoBehaviour {

    public void MakeANewDeck()
    {
        DeckBuildingScreen.Instance.HideScreen();
        CharacterSelectionScreen.Instance.ShowScreen();
    }
}
