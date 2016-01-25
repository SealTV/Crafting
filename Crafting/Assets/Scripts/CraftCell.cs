using UnityEngine;

public class CraftCell : BlockContainer
{
    public Transform RootTransform;

    public Cell[] Cells;
    public Block[] BlocksPrefub;

    public bool IsEmpty = true;

    private Block generatedBlock;
    public void GenerateBlock()
    {
        foreach(Cell cell in Cells)
        {
            if(cell.IsEmpty)
                return;
        }
        if(!IsEmpty)
        {
            Destroy(generatedBlock.gameObject);
            IsEmpty = true;
        }

        System.Random random = new System.Random();
        int blockId = random.Next(0, 4);
        BlockColor color = (BlockColor)random.Next(0, 4);

        Block block = Instantiate(BlocksPrefub[blockId]);
        block.Color = color;

        block.transform.SetParent(RootTransform);
        block.transform.position = new Vector3(transform.position.x, transform.position.y, -1);
        IsEmpty = false;

        OnDrop(block);
    }

    public override void OnDrop(Block block)
    {
        base.OnDrop(block);
        generatedBlock = block;
        IsEmpty = false;
    }

    protected override void OnBeginDragBlock(Block block)
    {
        block.OnBeginDragAction -= OnBeginDragBlock;
        IsEmpty = true;
        generatedBlock = null;
    }
}
