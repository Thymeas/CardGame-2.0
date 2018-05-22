﻿using UnityEngine;

public class DragSpellOnTarget : DraggingActions {

    public TargetingOptions Targets = TargetingOptions.AllCharacters;
    private SpriteRenderer sr;
    private LineRenderer lr;
    private WhereIsTheCardOrCreature whereIsThisCard;
    private VisualStates tempVisualState;
    private Transform triangle;
    private SpriteRenderer triangleSR;
    private GameObject Target;
    private OneCardManager manager;

    private CurvedLinePoint[] linePoints = new CurvedLinePoint[0];
    private Vector3[] linePositions = new Vector3[0];
    private Vector3[] linePositionsOld = new Vector3[0];

    public override bool CanDrag
    {
        get
        {
            return base.CanDrag && manager.CanBePlayedNow;
        }
    }

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        lr = GetComponentInChildren<LineRenderer>();
        lr.sortingLayerName = "AboveEverything";
        triangle = transform.Find("Triangle");
        triangleSR = triangle.GetComponent<SpriteRenderer>();

        manager = GetComponentInParent<OneCardManager>();
        whereIsThisCard = GetComponentInParent<WhereIsTheCardOrCreature>();
    }

    public override void OnStartDrag()
    {
        tempVisualState = whereIsThisCard.VisualState;
        whereIsThisCard.VisualState = VisualStates.Dragging;

        sr.enabled = true;
        lr.enabled = true;

        whereIsThisCard.SetHandSortingOrder();
    }

    public override void OnDraggingInUpdate()
    { 
        Vector3 notNormalized = transform.position - transform.parent.position;
        Vector3 direction = notNormalized.normalized;
        float distanceToTarget = (direction*2.3f).magnitude;
        if (notNormalized.magnitude > distanceToTarget)
        {
            linePoints = lr.gameObject.GetComponentsInChildren<CurvedLinePoint>();

            Vector3 midPoint = Vector3.Lerp(transform.parent.position, transform.position - direction*2.3f ,0.5f);
            linePositions = new Vector3[linePoints.Length];

            midPoint += new Vector3(0,0,-lr.positionCount * 0.1f);
            if(midPoint.z > 0)
                midPoint.z = 0;
            if(midPoint.z < -10f)
                midPoint.z = -10f;

            if(lr.positionCount < 2)
                midPoint.z = 0;

            linePoints[0].transform.position = transform.parent.position;
            linePoints[1].transform.position = midPoint;
            linePoints[2].transform.position = transform.position - direction*1.5f;

            linePositions[0] = linePoints[0].transform.position;
            linePositions[1] = linePoints[1].transform.position;
            linePositions[2] = linePoints[2].transform.position;
            
            if( linePositionsOld.Length != linePositions.Length )
            {
                linePositionsOld = new Vector3[linePositions.Length];
            }
            
            Vector3[] smoothedPoints = LineSmoother.SmoothLine( linePositions, 2 );

            lr.positionCount = smoothedPoints.Length;
            lr.SetPositions( smoothedPoints );

            lr.enabled = true;
            triangleSR.enabled = true;
            triangleSR.transform.position = transform.position - 1.35f*direction;

            float rot_z = Mathf.Atan2(notNormalized.y, notNormalized.x) * Mathf.Rad2Deg;
            triangleSR.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
        }
        else
        {
            lr.enabled = false;
            triangleSR.enabled = false;
        }

    }

    public override void OnEndDrag()
    {
        HandVisual PlayerHand = playerOwner.PArea.handVisual;
        Target = null;
        var hits = Physics.RaycastAll(origin: Camera.main.transform.position,
            direction: (-Camera.main.transform.position + this.transform.position).normalized,
            maxDistance: 30f);

        foreach (RaycastHit h in hits)
        {
            if (h.transform.tag.Contains("Player"))
                Target = h.transform.gameObject;
            else if (h.transform.tag.Contains("Creature"))
                Target = h.transform.parent.gameObject;
        }

        bool targetValid = false;

        if (Target != null)
        {
            int targetID = Target.GetComponent<IDHolder>().UniqueID;
            switch (Targets)
            {
                case TargetingOptions.AllCharacters:
                    playerOwner.PlayASpellFromHand(GetComponentInParent<IDHolder>().UniqueID, targetID);
                    targetValid = true;
                    playerOwner.PArea.handVisual.PlayASpellFromHand(GetComponentInParent<IDHolder>().UniqueID);
                    break;
                case TargetingOptions.AllCreatures:
                    if (Target.tag.Contains("Creature"))
                    {
                        playerOwner.PlayASpellFromHand(GetComponentInParent<IDHolder>().UniqueID, targetID);
                        targetValid = true;
                        playerOwner.PArea.handVisual.PlayASpellFromHand(GetComponentInParent<IDHolder>().UniqueID);
                    }
                    break;
                case TargetingOptions.EnemyCharacters:
                    if (Target.tag.Contains("Creature") || Target.tag.Contains("Player"))
                    {
                        // had to check that target is not a card
                        if ((tag.Contains("Low") && Target.tag.Contains("Top"))
                            || (tag.Contains("Top") && Target.tag.Contains("Low")))
                        {
                            playerOwner.PlayASpellFromHand(GetComponentInParent<IDHolder>().UniqueID, targetID);
                            targetValid = true;
                            playerOwner.PArea.handVisual.PlayASpellFromHand(GetComponentInParent<IDHolder>().UniqueID);
                        }
                    }
                    break;
                case TargetingOptions.EnemyCreatures:
                    if (Target.tag.Contains("Creature"))
                    {
                        // had to check that target is not a card or a player
                        if ((tag.Contains("Low") && Target.tag.Contains("Top"))
                            || (tag.Contains("Top") && Target.tag.Contains("Low")))
                        {
                            playerOwner.PlayASpellFromHand(GetComponentInParent<IDHolder>().UniqueID, targetID);
                            targetValid = true;
                            playerOwner.PArea.handVisual.PlayASpellFromHand(GetComponentInParent<IDHolder>().UniqueID);
                        }
                    }
                    break;
                case TargetingOptions.YourCharacters:
                    if (Target.tag.Contains("Creature") || Target.tag.Contains("Player"))
                    {
                        // had to check that target is not a card
                        if ((tag.Contains("Low") && Target.tag.Contains("Low"))
                            || (tag.Contains("Top") && Target.tag.Contains("Top")))
                        {
                            playerOwner.PlayASpellFromHand(GetComponentInParent<IDHolder>().UniqueID, targetID);
                            targetValid = true;
                            playerOwner.PArea.handVisual.PlayASpellFromHand(GetComponentInParent<IDHolder>().UniqueID);
                        }
                    }
                    break;
                case TargetingOptions.YourCreatures:
                    if (Target.tag.Contains("Creature"))
                    {
                        // had to check that target is not a card or a player
                        if ((tag.Contains("Low") && Target.tag.Contains("Low"))
                            || (tag.Contains("Top") && Target.tag.Contains("Top")))
                        {
                            playerOwner.PlayASpellFromHand(GetComponentInParent<IDHolder>().UniqueID, targetID);
                            targetValid = true;
                            playerOwner.PArea.handVisual.PlayASpellFromHand(GetComponentInParent<IDHolder>().UniqueID);
                        }
                    }
                    break;
                default:
                    Debug.LogWarning("Reached default case in DragSpellOnTarget! Suspicious behaviour!!");
                    break;
            }
        }

        if (!targetValid)
        {  
            whereIsThisCard.VisualState = tempVisualState;
            whereIsThisCard.SetHandSortingOrder();
            PlayerHand.PlaceCardsOnNewSlots();
        }
        
        transform.localPosition = new Vector3(0f, 0f, -1f);
        sr.enabled = false;
        lr.enabled = false;
        triangleSR.enabled = false;
    }
    
    protected override bool DragSuccessful()
    {
        return true;
    }

    public override void OnCancelDrag()
    {
        
    }
}