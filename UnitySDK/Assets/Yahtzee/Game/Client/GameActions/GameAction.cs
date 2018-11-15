using CommonUtil;

namespace Yahtzee.Game.Client
{
    /// <summary>
    /// Represent a meaningful action in game that player can perform. e.g. roll, lock dice, submit score
    /// </summary>
    public abstract class GameAction
    {
        protected ILogger Logger
        {
            get { return ServiceFactory.GetService<ILogger>(); }
        }
        public abstract bool IsValid(Common.Game game);
        public abstract void Perform(Common.Game game);
        public abstract void Revert(Common.Game game);

        public virtual int MeanExpectation(Common.Game game)
        {
            return 0;
        }

        public virtual int MaximumPossible(Common.Game game)
        {
            return 0;
        }
    }
}