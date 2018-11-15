namespace Yahtzee.Game.Common.GameCells
{
    public abstract class GameCellFactory
    {
        public static T CreateCell<T>() where T : GameCell, new()
        {
            return new T();
        }
    }
}