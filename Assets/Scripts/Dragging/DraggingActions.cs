using UnityEngine;

public abstract class DraggingActions : MonoBehaviour {

    public abstract void OnStartDrag();

    public abstract void OnEndDrag();

    public abstract void OnDraggingInUpdate();

    public abstract void OnCancelDrag();

    public virtual bool CanDrag
    {
        get
        {            
            return GlobalSettings.Instance.CanControlThisPlayer(playerOwner);
        }
    }

    protected virtual Player playerOwner
    {
        get
        {

            if (tag.Contains("Low"))
                return GlobalSettings.Instance.LowPlayer;
            return tag.Contains("Top") ? GlobalSettings.Instance.TopPlayer : null;
        }
    }

    protected abstract bool DragSuccessful();
}
