using Extensions;
using UnityEngine;

namespace World
{
    public static class NoiseSampler
    {
        private static readonly float _detailScale = 0.007F;
        private static readonly float _frequencyFactor = 2F;
        private static readonly float _amplitudeFactor = 2F;
        private static readonly int _octaves = 4;

        public static long Seed { get; set; } // 42 for testing

        public static void SetSeed(long seed)
        {
            Seed = seed;
        }

        public static float GetTreesNoise(float x, float z)
        {
            float bigPools = GetNoise(x, z, detailScale: 0.03F) * 1F;
            float smallPools1 = GetNoise(x, z, detailScale: 0.02F) * 1.84F;
            float smallPools2 = GetNoise(x, z, detailScale: 0.2F) * 4.85F;
            float smallPools3 = GetNoise(x, z, detailScale: 1F) * 1F;

            float sum = bigPools + smallPools1 + smallPools2 + smallPools3;
            float maxStrength = 1F + 1.84F + 4.85F + 1F;
            float remapped = sum.Remap(0F, maxStrength, 0F, 1F);

            System.Random random = new();
            return (float)random.NextDouble() + 0.3F > remapped ? 0 : 1;
        }

        public static float GetBouldersNoise(float x, float z)
        {
            x += 100;
            z += 100;
            float bigPools = GetNoise(x, z, detailScale: 0.02F) * 1F;
            float smallPools1 = GetNoise(x, z, detailScale: 0.03F) * 2F;
            float smallPools2 = GetNoise(x, z, detailScale: 0.1F) * 5F;
            float smallPools3 = GetNoise(x, z, detailScale: 0.2F) * 2F;

            float sum = bigPools + smallPools1 + smallPools2 + smallPools3;
            float maxStrength = 1F + 2F + 5F + 2F;
            float remapped = sum.Remap(0F, maxStrength, 0F, 1F);

            System.Random random = new();
            return (float)random.NextDouble() + 0.25F > remapped ? 0 : 1;
        }

        public static float GetNoise(float x, float y, float min = 0F, float max = 1F, float detailScale = 1F)
        {
            return OpenSimplex2S.Noise3_ImproveXZ(
                Seed,
                x * detailScale,
                Seed,
                y * detailScale)
                .Remap(0F, 1F, min, max);
        }

        public static float GetNoise(Vector3 position, float min = 0F, float max = 1F, float detailScale = 1F)
        {
            return GetNoise(position.x, position.z, min, max, detailScale);
        }

        public static int GetHeight(int x, int z)
        {
            float mediumNoise = GetMediumNoise(x, z);
            float largeNoise = GetLargeNoise(x, z);
            return Mathf.RoundToInt(Mathf.Round(Mathf.Round(mediumNoise * largeNoise * 2F) / 2F) / 4F) + 3;
        }

        private static float GetMediumNoise(int x, int z)
        {
            float result = 0F;
            float resultDivider = 0F;
            for (int i = 0; i < _octaves; i++)
            {
                result += OpenSimplex2S.Noise3_ImproveXZ(
                    Seed,
                    x * _detailScale * Mathf.Pow(_frequencyFactor, i) + ((i + 1) * 42),
                    Seed,
                    z * _detailScale * Mathf.Pow(_frequencyFactor, i) + ((i + 1) * 42))
                    / Mathf.Pow(_amplitudeFactor, i);
                resultDivider += 1F / Mathf.Pow(_amplitudeFactor, i);
            }
            result /= resultDivider;
            float remapped = result.Remap(0F, 1F, 0F, 10F);
            return Mathf.Round(remapped * 2F) / 2F;
        }

        private static float GetLargeNoise(int x, int z)
        {
            float result = OpenSimplex2S.Noise3_ImproveXZ(
                Seed,
                x * _detailScale / _frequencyFactor * 2 + Seed.GetHashCode() * 100,
                Seed,
                z * _detailScale / _frequencyFactor * 2 + Seed.GetHashCode() * 100);
            float remapped = result.Remap(0F, 1F, 0F, 10F);
            return Mathf.Round(remapped);
        }
    }
}