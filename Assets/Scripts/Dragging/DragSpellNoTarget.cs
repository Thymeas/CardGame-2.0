using UnityEngine;
using DG.Tweening;

public class DragSpellNoTarget: DraggingActions{

    private int savedHandSlot;
    private WhereIsTheCardOrCreature whereIsCard;
    private OneCardManager manager;

    public override bool CanDrag
    {
        get
        { 
            return base.CanDrag && manager.CanBePlayedNow;
        }
    }

    void Awake()
    {
        whereIsCard = GetComponent<WhereIsTheCardOrCreature>();
        manager = GetComponent<OneCardManager>();
    }

    public override void OnStartDrag()
    {
        savedHandSlot = whereIsCard.Slot;

        whereIsCard.VisualState = VisualStates.Dragging;
        whereIsCard.BringToFront();

    }

    public override void OnDraggingInUpdate()
    {
        
    }

    public override void OnEndDrag()
    {  
        if (DragSuccessful())
        {
            playerOwner.PlayASpellFromHand(GetComponent<IDHolder>().UniqueID, -1);
            playerOwner.PArea.handVisual.PlayASpellFromHand(GetComponentInParent<IDHolder>().UniqueID);
        }
        else
            OnCancelDrag();
    }

    public override void OnCancelDrag()
    {
        whereIsCard.Slot = savedHandSlot;
        whereIsCard.VisualState = tag.Contains("Low") ? VisualStates.LowHand : VisualStates.TopHand;
        HandVisual PlayerHand = playerOwner.PArea.handVisual;
        Vector3 oldCardPos = PlayerHand.slots.Children[savedHandSlot].transform.localPosition;
        transform.DOLocalMove(oldCardPos, 1f);
    } 

    protected override bool DragSuccessful()
    {
        return TableVisual.CursorOverSomeTable;
    }


}
