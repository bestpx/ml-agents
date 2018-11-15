namespace Yahtzee.Game.Common
{
    /// <summary>
    /// Roll represents each dice roll
    /// </summary>
    public class RollSlot
    {
        public static bool operator ==(RollSlot a, RollSlot b)
        {
            if (ReferenceEquals(null, a))
                return (ReferenceEquals(null, b));

            return a.Equals(b);
        }

        public static bool operator !=(RollSlot a, RollSlot b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            RollSlot other = obj as RollSlot;
            if (other == null)
            {
                return false;
            }
            return RollValue == other.RollValue;
        }

        public override int GetHashCode()
        {
            return RollValue;
        }

        public RollSlot(int rollValue, bool isLocked)
        {
            RollValue = rollValue;
            IsLocked = isLocked;
        }

        public void ToggleLock()
        {
            IsLocked = !IsLocked;
        }

        public int RollValue { get; }
        public bool IsLocked { get; set; }
    }
}