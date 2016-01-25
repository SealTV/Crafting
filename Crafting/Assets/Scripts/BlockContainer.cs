using System;
using UnityEngine;

public class BlockContainer : MonoBehaviour {
    public virtual void OnDrop(Block block)
    {
        block.ParenContainer = this;
        block.OnBeginDragAction += OnBeginDragBlock;
        block.SetToPosition(new Vector3(transform.position.x, transform.position.y, -1));
    }

    public virtual void OnDrop(Block block, Vector3 oldPosition)
    {
        this.OnDrop(block);
        block.SetToPosition(oldPosition);
    }

    protected virtual void OnBeginDragBlock(Block block)
    {
        block.OnBeginDragAction -= OnBeginDragBlock;
    }

    
}
