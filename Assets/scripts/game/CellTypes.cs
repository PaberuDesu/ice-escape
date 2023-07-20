public class CellTypes
{
    public const char EmptyLocator = '0';
    public const char BlueCellLocator = 'v';
    public const char RedCellLocator = 'n';
    public const char StarLocator = 's';
    public const char WallLocator = 'w';
    public const char BoxLocator = 'c';
    public const char FurnaceLocator = 'f';
    public const char DoorLocator = 'd';
    public const char IceLocator = 'i';
    public const char PermanentIceLocator = 'p';
    public const char BlueLocator = 'b';
    public const char RedLocator = 'r';
    public static char[] SolidObstacles = new char[5]{'p','w','c','v','n'};
    public static char[] DestroyableObstacles = new char[3]{'p','c','i'};
    public static char[] EnabledForBlue = new char[3]{'0', 'f', 'v'};
    public static char[] EnabledForRed = new char[4]{'0', 'f', 'b', 'n'};
}