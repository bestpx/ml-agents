using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Random = System.Random;

namespace Yahtzee.Game.Common
{
    /// <summary>
    /// Represent a hand of given number of dice Rolls
    /// </summary>
    public class Hand
    {
        private const int HandSize = 5;
        private const int MaxRollCount = 3;
        /// <summary>
        /// size is 7, 1-indexed
        /// </summary>
        private readonly int[] _histogram = new int[7];
        private readonly RollSlot[] _rolls = new RollSlot[HandSize];

        private int _rollCount;

        /// <summary>
        /// We "draw" roll values from the "deck" every time we roll
        /// </summary>
        private readonly int[] _deck;
        
        /// <summary>
        /// Next available roll from the deck
        /// </summary>
        private int _deckIndex;

        private bool _hasChangedLockedDiceSinceLastRoll = false;

        public Hand()
        {
            var random = new Random();
            _deck = new int[20];
            for (int i = 0; i < 20; i++)
            {
                _deck[i] = random.Next(1, 7);
            }

            for (int i = 0; i < HandSize; i++)
            {
                _rolls[i] = null;
            }
        }
        
        /// <summary>
        /// Use this for test purpose
        /// </summary>
        /// <param name="injectedRolls"></param>
        public Hand(int[] injectedRolls)
        {
            _deck = new int[20];
            for (int i = 0; i < 20; i++)
            {
                _deck[i] = injectedRolls[i];
            }

            for (int i = 0; i < HandSize; i++)
            {
                _rolls[i] = null;
            }
        }

        #region Queries
        
        public bool CanRoll()
        {
            if (_rolls[0] == null)
            {
                return true;
            }

            bool isAllLocked = true;
            for (int i = 0; i < HandSize; i++)
            {
                isAllLocked = isAllLocked && _rolls[i].IsLocked;
            }
            return _rollCount < MaxRollCount && !isAllLocked;
        }
        
        public int GetNumberOfRollsOfValue(int value)
        {
            return _histogram[value];
        }

        public bool IsYahtzee()
        {
            return _rolls[0] == _rolls[1] &&
                   _rolls[1] == _rolls[2] &&
                   _rolls[2] == _rolls[3] &&
                   _rolls[3] == _rolls[4];
        }

        public bool HasThreeOfAKind()
        {
            var max = _histogram.Max();
            return max >= 3;
        }
        
        public bool HasFourOfAKind()
        {
            var max = _histogram.Max();
            return max >= 4;
        }

        public bool IsFullHouse()
        {
            // is there a 2 and a 3 in the _histogram?
            bool hasTwo = false;
            bool hasThree = false;
            for (int i = 1; i < _histogram.Length; i++)
            {
                if (_histogram[i] == 2)
                {
                    hasTwo = true;
                }
                else if (_histogram[i] == 3)
                {
                    hasThree = true;
                }
            }

            return hasTwo && hasThree;
        }

        public int GetMaxStreak()
        {
            int streak = 0;
            int maxStreak = 0;
            for (int i = 1; i < _histogram.Length; i++)
            {
                if (_histogram[i] > 0)
                {
                    streak++;
                }
                else
                {
                    maxStreak = streak > maxStreak ? streak : maxStreak;
                    streak = 0;
                }
            }
            maxStreak = streak > maxStreak ? streak : maxStreak;
            streak = 0;

            return maxStreak;
        }

        public int GetSum()
        {
            int sum = 0;
            if (HasRolled())
            {
                for (int i = 0; i < HandSize; i++)
                {
                    sum += _rolls[i].RollValue;
                }
            }

            return sum;
        }

        public bool HasRolled()
        {
            return _rolls[0] != null;
        }

        public int GetRollAt(int index)
        {
            if (!HasRolled())
            {
                return 0;
            }
            return _rolls[index].RollValue;
        }
        
        public bool IsLockedAt(int index)
        {
            if (!HasRolled())
            {
                return false;
            }
            return _rolls[index].IsLocked;
        }

        public bool IsLastRoll()
        {
            return _rollCount == MaxRollCount;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (_rolls != null && HasRolled())
            {
                for (int i = 0; i < _rolls.Length; i++)
                {
                    if (_rolls[i].IsLocked)
                    {
                        sb.Append("*");
                    }

                    sb.Append(_rolls[i].RollValue);
                    sb.Append(",");
                }
            }

            return sb.ToString();
        }

        public bool HasChangedLockedDiceSinceLastRoll => _hasChangedLockedDiceSinceLastRoll;
        public int RollCount => _rollCount;
        public int[] Deck => _deck;

        #endregion

        #region Actions
        
        public void Roll()
        {
            if (!CanRoll())
            {
                return;
            }
            
            // fill _roll with _deck
            for (int i = 0; i < HandSize; i++)
            {
                if (_rolls[i] == null || !_rolls[i].IsLocked)
                {
                    _rolls[i] = new RollSlot(_deck[_deckIndex++], false);
                }
            }
            _rollCount++;
            
            // process current roll
            for (int i = 0; i < _histogram.Length; i++)
            {
                _histogram[i] = 0;
            }
            for (int i = 0; i < HandSize; i++)
            {
                _histogram[_rolls[i].RollValue]++;
            }

            _hasChangedLockedDiceSinceLastRoll = false;
        }

        public void SetLockStatus(bool[] lockStatus)
        {
            for (int i = 0; i < HandSize; i++)
            {
                _rolls[i].IsLocked = lockStatus[i];
            }

            _hasChangedLockedDiceSinceLastRoll = true;
        }

        #endregion
    }
}