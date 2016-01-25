using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Block : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public BlocColor Color;

    public RectTransform RootTransform;
    [HideInInspector]
    public RectTransform rectTransform;

    public Action<Block> OnBeginDragAction;

    private Transform oldParent;
    private Vector3 oldPosition;

    // Use this for initialization
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        // this.rectTransform.SetParent(RootTransform);
        oldParent = this.transform.parent;
        oldPosition = this.transform.position;
        if(OnBeginDragAction != null)
        {
            OnBeginDragAction(this);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {

    }

    public void RetunToOldPosition()
    {
        this.transform.SetParent(oldParent);
        this.transform.position = oldPosition;
    }

}


