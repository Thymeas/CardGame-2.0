using UnityEngine;
using DG.Tweening;

public class DragPackOpening : DraggingActions 
{   
    private bool _canceling;
    private bool _movingToOrReachedOpeningSpot = false;
    private Vector3 _savedPosition;

    public override bool CanDrag
    {
        get
        { 
            return ShopManager.Instance.OpeningArea.AllowedToDragAPack && !_canceling && !_movingToOrReachedOpeningSpot;
        }
    }
        
    public override void OnStartDrag()
    {
        _savedPosition = transform.localPosition;
        ShopManager.Instance.OpeningArea.AllowedToDragAPack = false;
    }

    public override void OnDraggingInUpdate()
    {
         
    }

    public override void OnEndDrag()
    {   
        if (DragSuccessful())
        {
            transform.DOMove(ShopManager.Instance.OpeningArea.transform.position, 0.5f).OnComplete(()=>
                { 
                    GetComponent<ScriptToOpenOnePack>().AllowToOpenThisPack();
                });
        }
        else
            OnCancelDrag();
    }

    public override void OnCancelDrag()
    {
        _canceling = true;
        transform.DOLocalMove(_savedPosition, 1f).OnComplete(() =>
            {
                _canceling = false;
                ShopManager.Instance.OpeningArea.AllowedToDragAPack = true;
            });
    } 

    protected override bool DragSuccessful()
    {
        return ShopManager.Instance.OpeningArea.CursorOverArea();
    }

}
