
using UnityEngine;

public class Cell : BlockContainer
{
    public bool IsEmpty = true;

    public Block BlockPrefub;
    public Transform RootTransform;

    private void Start()
    {
        Block block = Instantiate(BlockPrefub);
        block.Color = (BlockColor)block.Type;

        block.transform.SetParent(RootTransform);
        block.transform.position = new Vector3(transform.position.x, transform.position.y, -1);

        this.OnDrop(block);
    }

    public override void OnDrop(Block block)
    {
        this.OnDrop(block, new Vector3(transform.position.x, transform.position.y, -1));
    }

    public override void OnDrop(Block block, Vector3 position)
    {
        base.OnDrop(block, position);
        if(IsEmpty)
        {
            base.OnDrop(block, position);
            IsEmpty = false;
        }
        else
        {
            block.ReturnToOldPosition();
        }
    }

    protected override void OnBeginDragBlock(Block block)
    {
        block.OnBeginDragAction -= OnBeginDragBlock;
        IsEmpty = true;
    }
}
