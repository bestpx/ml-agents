using System.Text;
using CommonUtil;

namespace Yahtzee.Game.Client.GameActions
{
    public class ToggleHoldDiceAction : GameAction
    {
        /// <summary>
        /// The toggle status of the hand
        /// </summary>
        /// <returns></returns>
        private readonly bool[] _toggle;

        /// <summary>
        /// Set hand dice lock status to _toggle
        /// </summary>
        public ToggleHoldDiceAction(bool[] toggle)
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
            Logger.Log(LogLevel.Debug, "hand: " + game.Hand.ToString() + "Toggle: " + GetToggleString());
            game.ToggleHand(_toggle);
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