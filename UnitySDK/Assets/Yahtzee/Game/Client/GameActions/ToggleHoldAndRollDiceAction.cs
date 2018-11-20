using System.Text;
using CommonUtil;

namespace Yahtzee.Game.Client.GameActions
{
    public class ToggleHoldAndRollDiceAction : GameAction
    {
        /// <summary>
        /// The toggle status of the hand
        /// </summary>
        /// <returns></returns>
        private readonly bool[] _toggle;

        /// <summary>
        /// Set hand dice lock status to _toggle
        /// </summary>
        public ToggleHoldAndRollDiceAction(bool[] toggle)
        {
            _toggle = new bool[toggle.Length];
            for (int i = 0; i < toggle.Length; i++)
            {
                _toggle[i] = toggle[i];
            }
        }

        public override bool IsValid(Common.Game game)
        {
            return game.CanToggle();
        }

        public override void Perform(Common.Game game)
        {
            Logger.Log(LogLevel.Debug, "hand: " + game.Hand.ToString() + "Toggle and before roll: " + GetToggleString());
            game.ToggleHand(_toggle);
            game.Roll();
            Logger.Log(LogLevel.Debug, "Roll after toggle: " + game.Hand.ToString());
        }

        public override void Revert(Common.Game game)
        {
            throw new System.NotImplementedException();
        }

        private StringBuilder GetToggleString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < _toggle.Length; i++)
            {
                sb.Append(_toggle[i] + ", ");
            }

            return sb;
        }
    }
}