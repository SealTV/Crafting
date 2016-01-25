using System.Collections.Generic;
using UnityEngine;

public class Grid : BlockContainer
{
    public int Rows;
    public int Columns;

    private float CellSizeX;
    private float CellSizeY;
    private float LeftUpCellPositionX;
    private float LeftUpCellPositionY;

    private List<Block> gridBlocks = new List<Block>();
    private bool[,] GridMap;

    // Use this for initialization
    void Start()
    {
        GridMap = new bool[Columns, Rows];
        SpriteRenderer spriteRender = GetComponent<SpriteRenderer>();
        Sprite sprite = spriteRender.sprite;
        CellSizeX = sprite.rect.width * transform.localScale.x/ (sprite.pixelsPerUnit * Columns);
        CellSizeY = sprite.rect.height* transform.localScale.y / (sprite.pixelsPerUnit * Rows);

        LeftUpCellPositionX = transform.position.x - 5f * CellSizeX;
        LeftUpCellPositionY = transform.position.y + 2f * CellSizeY;
    }

    public override void OnDrop(Block block)
    {
        Vector3 dropPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        int x = Mathf.FloorToInt((dropPosition.x + (-LeftUpCellPositionX) + CellSizeX / 2) / CellSizeX);
        int y = -Mathf.FloorToInt((dropPosition.y + (-LeftUpCellPositionY) + CellSizeY / 2) / CellSizeY);

        if(!TrySetBlock(GridMap, x, y, block.Type))
        {
            block.ReturnToOldPosition();
            return;
        }
        block.I = x;
        block.J = y;
        base.OnDrop(block);
        this.gridBlocks.Add(block);
        block.SetToPosition(new Vector3(x * CellSizeX + LeftUpCellPositionX, -y * CellSizeY + LeftUpCellPositionY, -1));
    }

    public override void OnDrop(Block block, Vector3 dropPosition)
    {
        int x = Mathf.FloorToInt((dropPosition.x + (-LeftUpCellPositionX) + CellSizeX / 2) / CellSizeX);
        int y = -Mathf.FloorToInt((dropPosition.y + (-LeftUpCellPositionY) + CellSizeY / 2) / CellSizeY);

        if(!TrySetBlock(GridMap, x, y, block.Type))
        {
            block.ReturnToOldPosition();
            return;
        }

        block.I = x;
        block.J = y;
        base.OnDrop(block);
        this.gridBlocks.Add(block);
        block.SetToPosition(new Vector3(x * CellSizeX + LeftUpCellPositionX, -y * CellSizeY + LeftUpCellPositionY, -1));
    }

    protected override void OnBeginDragBlock(Block block)
    {
        base.OnBeginDragBlock(block);
        this.gridBlocks.Remove(block);

        CearMap(GridMap, block.I, block.J, block.Type);

        block.I = -1;
        block.J = -1;
    }

    public void Sort()
    {
        List<Block> blocksToOrder = new List<Block>(gridBlocks);
        blocksToOrder.Sort(delegate (Block x, Block y)
        {
            return x.Type.CompareTo(y.Type);
        });

        blocksToOrder.Reverse();

        bool[,] newGridMap = new bool[Columns, Rows];

        foreach(Block block in blocksToOrder)
        {
            bool isInserted = false;
            for(int i = 0; i < Columns - block.Type.GetWidth() - 1; i++)
            {
                for(int j = 0; j < Rows - block.Type.GetHeight() - 1; j++)
                {
                    if(TrySetBlock(newGridMap, i, j, block.Type))
                    {
                        isInserted = true;
                        block.I = i;
                        block.J = j;
                        block.SetToPosition(new Vector3(i * CellSizeX + LeftUpCellPositionX, - j * CellSizeY + LeftUpCellPositionY, -1));
                    }
                    if(isInserted)
                        break;
                }
                if(isInserted)
                    break;
            }

        }

        GridMap = newGridMap;
    }

    private bool TrySetBlock(bool[,] gridMap, int i, int j, BlockType blockType)
    {
        if(i + blockType.GetWidth() - 1 >= Columns ||
           j + blockType.GetHeight() - 1 >= Rows ||
           gridMap[i, j] ||
           gridMap[i + blockType.GetWidth() - 1, j] ||
           gridMap[i, j + blockType.GetHeight() - 1] ||
           gridMap[i + blockType.GetWidth() - 1, j + blockType.GetHeight() - 1])
        {
            return false;
        }

        gridMap[i, j] = true;
        gridMap[i + blockType.GetWidth() - 1, j] = true;
        gridMap[i, j + blockType.GetHeight() - 1] = true;
        gridMap[i + blockType.GetWidth() - 1, j + blockType.GetHeight() - 1] = true;

        return true;
    }

    private void CearMap(bool[,] gridMap, int i, int j, BlockType blockType)
    {
        gridMap[i, j] = false;
        gridMap[i + blockType.GetWidth() - 1, j] = false;
        gridMap[i, j + blockType.GetHeight() - 1] = false;
        gridMap[i + blockType.GetWidth() - 1, j + blockType.GetHeight() - 1] = false;
    }

}
