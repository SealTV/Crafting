using UnityEngine;
using System;

public class Block : MonoBehaviour
{
    public Sprite[] Sprites;

    private BlockColor color;
    public BlockColor Color
    {
        get
        {
            return color;
        }
        set
        {
            color = value;
            var spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = Sprites[(int)value];
        }
    }

    public BlockType Type;

    public int X;
    public int Y;
    public float Speed = 5;

    [HideInInspector]
    public BlockContainer ParenContainer;
    public Action<Block> OnBeginDragAction;

    private Vector3 oldPosition;
    private BlockContainer oldParentContainer;

    private bool isSelected;
    private bool isMove;
    private Vector3 target;

    private float LeftUpX;
    private float LeftUpY;

    void Start()
    {
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        var cellWidth = sprite.rect.width * transform.localScale.x / (sprite.pixelsPerUnit * Type.GetWidth());
        var cellHeight = sprite.rect.height * transform.localScale.y / (sprite.pixelsPerUnit * Type.GetHeight());

        LeftUpX = Type.GetWidth() == 1 ? 0 : cellWidth / Type.GetWidth();
        LeftUpY = Type.GetHeight() == 1? 0 : cellHeight / Type.GetHeight();
    }

    void Update()
    {
        if(isMove && !isSelected)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, Speed * Time.deltaTime);
            if(transform.position == target)
                isMove = false;
        }
    }

    public void SetToPosition(Vector3 target)
    {
        this.target = target;
        if(ParenContainer is Grid)
        {
            this.target.x += LeftUpX;
            this.target.y -= LeftUpY;
        }

        isMove = true;
    }

    public void ReturnToOldPosition()
    {
        oldParentContainer.OnDrop(this, oldPosition);
    }

    public void OnMouseUp()
    {
        isSelected = false;

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var hits = Physics.RaycastAll(ray);
        if(hits.Length == 0)
        {
            this.ReturnToOldPosition();
            return;
        }

        foreach(var hit in hits)
        {
            var objectHit = hit.collider.gameObject;

            BlockContainer container = objectHit.GetComponent<BlockContainer>();
            container.OnDrop(this);
        }
    }

    public void OnMouseDrag()
    {
        if(!isSelected)
            return;

        var position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        position.z = -1;
        transform.position = position;
    }

    public void OnMouseDown()
    {
        isSelected = true;
        if(OnBeginDragAction != null)
            OnBeginDragAction(this);

        oldPosition = transform.position;
        oldParentContainer = ParenContainer;
    }
}


