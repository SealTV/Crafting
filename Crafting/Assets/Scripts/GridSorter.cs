using System.Collections.Generic;

public class GridSorter
{
    private int rows;
    private int columns;

    private bool[,] gridMap;

    public GridSorter(int rows, int columns)
    {
        this.rows = rows;
        this.columns = columns;
        gridMap = new bool[columns, rows];
    }

    public void SortGribObjects(IEnumerable<Block> targetBlocks)
    {
        List<Block> blocksToOrder = new List<Block>(targetBlocks);
        blocksToOrder.Sort(delegate (Block x, Block y)
        {
            return x.Type.CompareTo(y.Type);
        });

        blocksToOrder.Reverse();

        bool[,] newGridMap = new bool[columns, rows];

        foreach(Block block in blocksToOrder)
        {
            bool isBlockInsterted = false;
            for(int i = 0; i <= columns - block.Type.GetWidth(); i++)
            {
                for(int j = 0; j <= rows - block.Type.GetHeight(); j++)
                {
                    if(TrySetBlock(newGridMap, i, j, block.Type))
                    {
                        isBlockInsterted = true;
                        block.X = i;
                        block.Y = j;
                    }
                    if(isBlockInsterted)
                        break;
                }
                if(isBlockInsterted)
                    break;
            }
        }

        gridMap = newGridMap;
        targetBlocks = blocksToOrder;
    }

    public void RemoveBlockFromGrid(int i, int j, BlockType blockType)
    {
        gridMap[i, j] = false;
        gridMap[i + blockType.GetWidth() - 1, j] = false;
        gridMap[i, j + blockType.GetHeight() - 1] = false;
        gridMap[i + blockType.GetWidth() - 1, j + blockType.GetHeight() - 1] = false;
    }

    public bool TrySetBlock(int i, int j, BlockType blockType)
    {
        return this.TrySetBlock(gridMap, i, j, blockType);
    }

    private bool TrySetBlock(bool[,] map, int i, int j, BlockType blockType)
    {
        if(i + blockType.GetWidth() - 1 >= columns ||
           j + blockType.GetHeight() - 1 >= rows ||
           map[i, j] ||
           map[i + blockType.GetWidth() - 1, j] ||
           map[i, j + blockType.GetHeight() - 1] ||
           map[i + blockType.GetWidth() - 1, j + blockType.GetHeight() - 1])
        {
            return false;
        }

        map[i, j] = true;
        map[i + blockType.GetWidth() - 1, j] = true;
        map[i, j + blockType.GetHeight() - 1] = true;
        map[i + blockType.GetWidth() - 1, j + blockType.GetHeight() - 1] = true;
        return true;
    }

}
