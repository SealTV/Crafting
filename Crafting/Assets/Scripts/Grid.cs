using UnityEngine;
using UnityEngine.EventSystems;

public class Grid : MonoBehaviour, IDropHandler
{

    public int Rows;
    public int Columns;
    public float Offset;
    public float CellSize;

    private Block[] blocks;
    private RectTransform rectTransform;
    // Use this for initialization
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        CellSize = rectTransform.sizeDelta.y / Rows;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnDrop(PointerEventData data)
    {

        if(data.pointerDrag != null)
        {
            var dropedObject = data.pointerDrag;
            dropedObject.transform.SetParent(transform);
            var rectTransformObj = dropedObject.GetComponent<Transform>();

            rectTransformObj.SetParent(rectTransform);
            rectTransformObj.localPosition = new Vector3(rectTransform.position.x + CellSize, 0, 0);

            Debug.Log(data.pressPosition);
        }
    }
}
