using System;

namespace Assets.Code
{
    public class Dice
    {
        private int _faces;

        private static Random _random = new Random();

        public Dice(int faces)
        {
            _faces = faces;
        }

        public int Roll()
        {
            return _random.Next(1, _faces);
        }

        public static int Roll(int faces)
        {
            return _random.Next(1, faces);
        }
    }
}