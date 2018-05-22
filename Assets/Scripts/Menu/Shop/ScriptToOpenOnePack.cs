using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ScriptToOpenOnePack : MonoBehaviour {
  
    public Image GlowImage;
    public Color32 GlowColor;
    private bool allowedToOpen = false;
    private Collider col;

    void Awake()
    {
        col = GetComponent<BoxCollider>();
    }

    public void AllowToOpenThisPack()
    {
        allowedToOpen = true;
        ShopManager.Instance.OpeningArea.AllowedToDragAPack = false;
        ShopManager.Instance.OpeningArea.BackButton.interactable = false;
        if (CursorOverPack())
            GlowImage.DOColor(GlowColor, 0.5f);
    }

    private bool CursorOverPack()
    {
        var hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition), 30f);

        bool passedThroughTableCollider = false;
        foreach (RaycastHit h in hits)
        {
            if (h.collider == col)
                passedThroughTableCollider = true;
        }
        return passedThroughTableCollider;
    }

    void OnMouseEnter()
    {
        if (allowedToOpen)
            GlowImage.DOColor(GlowColor, 0.5f);
    }

    void OnMouseExit()
    {
        GlowImage.DOColor(Color.clear, 0.5f);
    }

    void OnMouseDown()
    {
        if (allowedToOpen)
        {
            allowedToOpen = false;
            Sequence s = DOTween.Sequence();
            s.Append(transform.DOLocalMoveZ(-2f, 0.5f));
            s.Append(transform.DOShakeRotation(1f, 20f, 20));

            s.OnComplete(() =>
                {
                    ShopManager.Instance.OpeningArea.ShowPackOpening(transform.position);
                    if (ShopManager.Instance.PacksCreated>0)
                        ShopManager.Instance.PacksCreated--;
                    Destroy(gameObject);
                }); 
        }
    }
}
