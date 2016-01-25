using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class TargetCell : MonoBehaviour, IDropHandler
{
    private RectTransform rectTransform;
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
            dropedObject.transform.SetParent(transform);
            var rectTransformObj = dropedObject.GetComponent<RectTransform>();

            rectTransformObj.SetParent(rectTransform);
            rectTransformObj.position = rectTransform.position;
        }
    }
}
