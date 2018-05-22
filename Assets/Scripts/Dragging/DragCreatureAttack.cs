using UnityEngine;
public class DragCreatureAttack : DraggingActions {

    private SpriteRenderer _sr;
    private LineRenderer _lr;
    private WhereIsTheCardOrCreature _whereIsThisCreature;
    private Transform _triangle;
    private SpriteRenderer _triangleSr;
    private GameObject _target;
    private OneCreatureManager _manager;

    private CurvedLinePoint[] _linePoints = new CurvedLinePoint[0];
    private Vector3[] _linePositions = new Vector3[0];
    private Vector3[] _linePositionsOld = new Vector3[0];
    
    void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
        _lr = GetComponentInChildren<LineRenderer>();
        _lr.sortingLayerName = "AboveEverything";
        _triangle = transform.Find("Triangle");
        _triangleSr = _triangle.GetComponent<SpriteRenderer>();

        _manager = GetComponentInParent<OneCreatureManager>();
        _whereIsThisCreature = GetComponentInParent<WhereIsTheCardOrCreature>();
    }

    public override bool CanDrag
    {
        get
        {
            return base.CanDrag && _manager.CanAttackNow;
        }
    }

    public override void OnStartDrag()
    {
        _whereIsThisCreature.VisualState = VisualStates.Dragging;
        _sr.enabled = true;
        _lr.enabled = true;
    }

    public override void OnDraggingInUpdate()
    {
        Vector3 notNormalized = transform.position - transform.parent.position;
        Vector3 direction = notNormalized.normalized;
        float distanceToTarget = (direction*2.3f).magnitude;
        if (notNormalized.magnitude > distanceToTarget)
        {
            _linePoints = _lr.gameObject.GetComponentsInChildren<CurvedLinePoint>();
            Vector3 midPoint = Vector3.Lerp(transform.parent.position, transform.position - direction, 0.5f);
            _linePositions = new Vector3[_linePoints.Length];

            midPoint += new Vector3(0, 0, -_lr.positionCount * 0.1f);
            if (midPoint.z > 0)
                midPoint.z = 0;
            if (midPoint.z < -5f)
                midPoint.z = -5f;
            if (_lr.positionCount < 2)
                midPoint.z = 0;

            _linePoints[0].transform.position = transform.parent.position;
            _linePoints[1].transform.position = midPoint;
            _linePoints[2].transform.position = transform.position - direction * 2f;

            _linePositions[0] = _linePoints[0].transform.position;
            _linePositions[1] = _linePoints[1].transform.position;
            _linePositions[2] = _linePoints[2].transform.position;

            if (_linePositionsOld.Length != _linePositions.Length)
                _linePositionsOld = new Vector3[_linePositions.Length];
        
            Vector3[] smoothedPoints = LineSmoother.SmoothLine( _linePositions, 2 );

            _lr.positionCount = smoothedPoints.Length;
            _lr.SetPositions( smoothedPoints );
            _lr.enabled = true;
            
            _triangleSr.enabled = true;
            _triangleSr.transform.position = transform.position - 1.5f*direction;

            float rot_z = Mathf.Atan2(notNormalized.y, notNormalized.x) * Mathf.Rad2Deg;
            _triangleSr.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
        }
        else
        {
            _lr.enabled = false;
            _triangleSr.enabled = false;
        } 
    }

    public override void OnEndDrag()
    {
        _target = null;
        var hits = Physics.RaycastAll(origin: Camera.main.transform.position,
            direction: (-Camera.main.transform.position + this.transform.position).normalized,
            maxDistance: 30f);

        foreach (RaycastHit h in hits)
        {
            if ((h.transform.tag == "TopPlayer" && this.tag == "LowCreature") ||
                (h.transform.tag == "LowPlayer" && this.tag == "TopCreature"))
                _target = h.transform.gameObject;
            else if ((h.transform.tag == "TopCreature" && this.tag == "LowCreature") ||
                    (h.transform.tag == "LowCreature" && this.tag == "TopCreature"))
                _target = h.transform.parent.gameObject;
        }

        bool targetValid = false;

        if (_target != null)
        {
            int targetID = _target.GetComponent<IDHolder>().UniqueID;

            if (targetID == GlobalSettings.Instance.LowPlayer.PlayerID || targetID == GlobalSettings.Instance.TopPlayer.PlayerID)
            {
                CreatureLogic.CreaturesCreatedThisGame[GetComponentInParent<IDHolder>().UniqueID].GoFace();
                targetValid = true;
            }
            else if (CreatureLogic.CreaturesCreatedThisGame[targetID] != null)
            {
                targetValid = true;
                CreatureLogic.CreaturesCreatedThisGame[GetComponentInParent<IDHolder>().UniqueID].AttackCreatureWithID(targetID);
            }

        }

        if (!targetValid)
        {
            _whereIsThisCreature.VisualState = tag.Contains("Low") ? VisualStates.LowTable : VisualStates.TopTable;
            _whereIsThisCreature.SetTableSortingOrder();
        }
        
        transform.localPosition = Vector3.zero;
        _sr.enabled = false;
        _lr.enabled = false;
        _triangleSr.enabled = false;

    }
    protected override bool DragSuccessful()
    {
        return true;
    }

    public override void OnCancelDrag()
    {
      
    }
}
