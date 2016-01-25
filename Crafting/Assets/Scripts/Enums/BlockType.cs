public enum BlockType
{
    Block1x1 = 0,
    Block1x2,
    Block2x1,
    Block2x2
}

public static class BlockTypeExtension
{
    public static int GetWidth(this BlockType type)
    {
        switch(type)
        {
            case BlockType.Block1x1:
            case BlockType.Block1x2:
                return 1;
            case BlockType.Block2x1:
            case BlockType.Block2x2:
                return 2;
        }

        return 0;
    }

    public static int GetHeight(this BlockType type)
    {
        switch(type)
        {
            case BlockType.Block1x1:
            case BlockType.Block2x1:
                return 1;
            case BlockType.Block1x2:
            case BlockType.Block2x2:
                return 2;
        }

        return 0;
    }
}