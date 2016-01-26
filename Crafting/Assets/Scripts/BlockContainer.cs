using UnityEngine;

public class BlockContainer : MonoBehaviour
{
    public virtual void OnDrop(Block block)
    {
        OnDrop(block, new Vector3(transform.position.x, transform.position.y, -1));
    }

    public virtual void OnDrop(Block block, Vector3 position)
    {
        block.ParenContainer = this;
        block.OnBeginDragAction += OnBeginDragBlock;
        block.SetToPosition(position);
    }

    protected virtual void OnBeginDragBlock(Block block)
    {
        block.OnBeginDragAction -= OnBeginDragBlock;
    }
}
