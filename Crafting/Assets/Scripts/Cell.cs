using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Cell : MonoBehaviour, IDropHandler
{
    private RectTransform rectTransform;

    private bool IsEmpty = true;

    // Use this for initialization
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }


    // Update is called once per frame
    void Update () {
	
	}

    public void OnDrop(PointerEventData data)
    {

        if(data.pointerDrag != null)
        {
            var dropedObject = data.pointerDrag;
            Block block = dropedObject.GetComponent<Block>();
            if(!IsEmpty)
            {
                block.RetunToOldPosition();
                return;
            }

            IsEmpty = false;
            block.OnBeginDragAction += OnBeginDragBlock;


            dropedObject.transform.SetParent(transform);
            var rectTransformObj = dropedObject.GetComponent<RectTransform>();

            rectTransformObj.SetParent(rectTransform);
            rectTransformObj.position = rectTransform.position;
        }
    }

    private void OnBeginDragBlock(Block block)
    {
        block.OnBeginDragAction -= OnBeginDragBlock;
        IsEmpty = true;
    }
}
