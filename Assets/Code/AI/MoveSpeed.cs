using System;

namespace AI
{
    public struct MoveSpeed
    {
        public float Speed { get; }
        public float Acceleration { get; }

        public MoveSpeed(float speed, float acceleration)
        {
            Speed = speed;
            Acceleration = acceleration;
        }

        public override bool Equals(object obj)
        {
            return obj is MoveSpeed other &&
                   Speed == other.Speed &&
                   Acceleration == other.Acceleration;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Speed, Acceleration);
        }

        public void Deconstruct(out float speed, out float acceleration)
        {
            speed = Speed;
            acceleration = Acceleration;
        }

        public static implicit operator (float Speed, float Acceleration)(MoveSpeed value)
        {
            return (value.Speed, value.Acceleration);
        }

        public static implicit operator MoveSpeed((float Speed, float Acceleration) value)
        {
            return new MoveSpeed(value.Speed, value.Acceleration);
        }
    }
}