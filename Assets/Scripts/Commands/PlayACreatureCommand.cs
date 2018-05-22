using UnityEngine;

public class PlayACreatureCommand : Command
{
    private CardLogic _cl;
    private int _tablePos;
    private Player _p;
    private int _creatureId;

    public PlayACreatureCommand(CardLogic cl, Player p, int tablePos, int creatureID)
    {
        this._p = p;
        this._cl = cl;
        this._tablePos = tablePos;
        this._creatureId = creatureID;
    }

    public override void StartCommandExecution()
    {
        HandVisual PlayerHand = _p.PArea.handVisual;
        GameObject card = IDHolder.GetGameObjectWithID(_cl.UniqueCardID);
        PlayerHand.RemoveCard(card);
        GameObject.Destroy(card);
        HoverPreview.PreviewsAllowed = true;
        _p.PArea.tableVisual.AddCreatureAtIndex(_cl.ca, _creatureId, _tablePos);
    }
}
