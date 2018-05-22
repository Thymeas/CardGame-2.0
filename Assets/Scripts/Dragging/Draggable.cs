using UnityEngine;

public class Draggable : MonoBehaviour {

    public enum StartDragBehavior
    {
        OnMouseDown, InAwake
    }

    public enum EndDragBehavior
    {
        OnMouseUp, OnMouseDown
    }

    public StartDragBehavior HowToStart = StartDragBehavior.OnMouseDown;
    public EndDragBehavior HowToEnd = EndDragBehavior.OnMouseUp;

    private bool _dragging;
    private Vector3 _pointerDisplacement;
    private float _zDisplacement;
    private DraggingActions _da;
    
    private static Draggable _draggingThis;
    public static Draggable DraggingThis
    {
        get{ return _draggingThis;}
    }
    
    void Awake()
    {
        _da = GetComponent<DraggingActions>();
    }

    void OnMouseDown()
    {
        if (_da!=null && _da.CanDrag && HowToStart == StartDragBehavior.OnMouseDown)
        {
            StartDragging();
        }

        if (_dragging && HowToEnd == EndDragBehavior.OnMouseDown)
        {
            _dragging = false;
            HoverPreview.PreviewsAllowed = true;
            _draggingThis = null;
            _da.OnEndDrag();
        }
    }
    
    void Update ()
    {
        if (_dragging)
        { 
            Vector3 mousePos = MouseInWorldCoords();
            transform.position = new Vector3(mousePos.x - _pointerDisplacement.x, mousePos.y - _pointerDisplacement.y, transform.position.z);   
            _da.OnDraggingInUpdate();
        }
    }
	
    void OnMouseUp()
    {
        if (_dragging && HowToEnd == EndDragBehavior.OnMouseUp)
        {
            _dragging = false;
            HoverPreview.PreviewsAllowed = true;
            _draggingThis = null;
            _da.OnEndDrag();
        }
    }  

    public void StartDragging()
    {
        _dragging = true;
        HoverPreview.PreviewsAllowed = false;
        _draggingThis = this;
        _da.OnStartDrag();
        _zDisplacement = -Camera.main.transform.position.z + transform.position.z;
        _pointerDisplacement = -transform.position + MouseInWorldCoords();
    }

    public void CancelDrag()
    {
        if (_dragging)
        {
            _dragging = false;
            HoverPreview.PreviewsAllowed = true;
            _draggingThis = null;
            _da.OnCancelDrag();
        }
    }

    private Vector3 MouseInWorldCoords()
    {
        var screenMousePos = Input.mousePosition;
        screenMousePos.z = _zDisplacement;
        return Camera.main.ScreenToWorldPoint(screenMousePos);
    }
        
}
