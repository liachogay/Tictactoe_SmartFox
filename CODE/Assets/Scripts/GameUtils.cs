public static class GameUtils
{
    /// <summary>
    /// size of column is 3
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static int MatrixToIndex(int x, int y)
    {
        int ret = 0;
        ret = x * 3 + y;
        return ret;
    }
}
