using UnityEngine;
using System.Linq;

public class CraftCell : BlockContainer
{
    public Transform RootTransform;
    public Cell[] Cells;
    public Block[] BlocksPrefub;

    private Block generatedBlock;

    public void GenerateNewBlock()
    {
        if(Cells.Any(cell => cell.IsEmpty))
            return;
        
        if(generatedBlock != null)
            Destroy(generatedBlock.gameObject);

        System.Random random = new System.Random();
        int blockId = random.Next(0, 4);
        BlockColor color = (BlockColor)random.Next(0, 4);

        Block block = Instantiate(BlocksPrefub[blockId]);
        block.Color = color;

        block.transform.SetParent(RootTransform);
        block.transform.position = new Vector3(transform.position.x, transform.position.y, -1);

        this.OnDrop(block);
    }

    public override void OnDrop(Block block)
    {
        base.OnDrop(block);
        generatedBlock = block;
    }

    public override void OnDrop(Block block, Vector3 position)
    {
        base.OnDrop(block, position);
        generatedBlock = block;
    }

    protected override void OnBeginDragBlock(Block block)
    {
        block.OnBeginDragAction -= OnBeginDragBlock;
        generatedBlock = null;
    }
}
