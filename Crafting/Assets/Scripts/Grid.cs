using System.Collections.Generic;
using UnityEngine;

public class Grid : BlockContainer
{
    public int RowsCount = 5;
    public int ColumnsCount = 11;

    private float cellWidth;
    private float cellHeight;
    private float leftUpCellX;
    private float leftUpCellY;

    private List<Block> gridBlocks = new List<Block>();

    private GridSorter gridSorter;

    // Use this for initialization
    void Start()
    {
        SpriteRenderer spriteRender = GetComponent<SpriteRenderer>();
        Sprite sprite = spriteRender.sprite;
        cellWidth = sprite.rect.width * transform.localScale.x/ (sprite.pixelsPerUnit * ColumnsCount);
        cellHeight = sprite.rect.height* transform.localScale.y / (sprite.pixelsPerUnit * RowsCount);

        leftUpCellX = transform.position.x - 5f * cellWidth;
        leftUpCellY = transform.position.y + 2f * cellHeight;

        gridSorter = new GridSorter(RowsCount, ColumnsCount);
    }

    public void SortGrid()
    {
        this.gridSorter.SortGribObjects(this.gridBlocks);
        foreach(Block block in this.gridBlocks)
            block.SetToPosition(new Vector3(block.X * cellWidth + leftUpCellX, -block.Y * cellHeight + leftUpCellY, -1));
    }

    public override void OnDrop(Block block)
    {
        Vector3 dropPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        OnDrop(block, dropPosition);
    }

    public override void OnDrop(Block block, Vector3 position)
    {
        int x = Mathf.FloorToInt((position.x + (-leftUpCellX) + cellWidth / 2) / cellWidth);
        int y = -Mathf.FloorToInt((position.y + (-leftUpCellY) + cellHeight / 2) / cellHeight);

        if(!this.gridSorter.TrySetBlock(x, y, block.Type))
        {
            block.ReturnToOldPosition();
            return;
        }

        block.X = x;
        block.Y = y;
        this.gridBlocks.Add(block);

        base.OnDrop(block, new Vector3(x * cellWidth + leftUpCellX, -y * cellHeight + leftUpCellY, -1));
    }

    protected override void OnBeginDragBlock(Block block)
    {
        base.OnBeginDragBlock(block);

        this.gridSorter.RemoveBlockFromGrid(block.X, block.Y, block.Type);
        this.gridBlocks.Remove(block);
        block.X = -1;
        block.Y = -1;
    }
}
