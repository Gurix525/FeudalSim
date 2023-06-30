using System;
using Extensions;

namespace PlayerControls
{
    public struct Skill : IEquatable<Skill>
    {
        #region Properties

        public float XP { get; }
        public int Level => GetLevel(XP);
        public float Modifier => GetModifier(Level);

        public float CurrentLevelXP => GetRequiredXP(Level);
        public float NextLevelXP => GetRequiredXP(Level + 1);

        public float CurrentXPSurplus => XP - CurrentLevelXP;
        public float RequiredXPToNextLevel => NextLevelXP - CurrentLevelXP;

        public static Skill Zero { get; }

        #endregion Properties

        #region Constructors

        public Skill(float xp)
        {
            int level = GetLevel(xp);
            float modifier = GetModifier(level);
            XP = xp;
        }

        #endregion Constructors

        #region Public

        public override bool Equals(object obj)
        {
            return obj is Skill skill && Equals(skill);
        }

        public bool Equals(Skill other)
        {
            return XP == other.XP;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(XP);
        }

        public Skill Add(float addend)
        {
            return this + addend;
        }

        public Skill Minus(float subtrahend)
        {
            return this - subtrahend;
        }

        public static bool operator ==(Skill left, Skill right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Skill left, Skill right)
        {
            return !(left == right);
        }

        public static Skill operator +(Skill left, float right)
        {
            return new((left.XP + right).Clamp(0F, float.MaxValue));
        }

        public static Skill operator -(Skill left, float right)
        {
            return new((left.XP - right).Clamp(0F, float.MaxValue));
        }

        #endregion Public

        #region Private

        private static int GetLevel(float xp)
        {
            return (int)MathF.Floor(MathF.Pow((xp / 10F), 2F / 3F));
        }

        private static float GetModifier(float level)
        {
            return 5F / 6F * MathF.Log10(MathF.Pow((90F * level / 100F) + 10F, 2F) / 100F);
        }

        private static float GetRequiredXP(float level)
        {
            return 10F * MathF.Pow(level, 3F / 2F);
        }

        #endregion Private
    }
}