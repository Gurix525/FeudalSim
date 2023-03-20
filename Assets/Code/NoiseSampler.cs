using UnityEngine;

public static class NoiseSampler
{
    private static readonly float _detailScale = 0.007F;
    private static readonly float _frequencyFactor = 2F;
    private static readonly float _amplitudeFactor = 2F;
    private static readonly int _octaves = 4;
    private static readonly string _seed = "42";

    private static int _seedHashCode = _seed.GetHashCode();

    public static float GetNoise(float x, float y, float min = 0F, float max = 1F, float detailScale = 1F)
    {
        return OpenSimplex2S.Noise3_ImproveXZ(
            _seedHashCode,
            x * detailScale,
            _seedHashCode,
            y * detailScale)
            .Remap(0F, 1F, min, max);
    }

    public static float GetNoise(Vector3 position, float min = 0F, float max = 1F, float detailScale = 1F)
    {
        return GetNoise(position.x, position.z, min, max, detailScale);
    }

    public static float GetHeight(int x, int z)
    {
        float mediumNoise = GetMediumNoise(x, z);
        float largeNoise = GetLargeNoise(x, z);
        return Mathf.Round(Mathf.Round(Mathf.Round(mediumNoise * largeNoise * 2F) / 2F) / 4F) + 6F;
    }

    private static float GetMediumNoise(int x, int z)
    {
        float result = 0F;
        float resultDivider = 0F;
        for (int i = 0; i < _octaves; i++)
        {
            result += OpenSimplex2S.Noise3_ImproveXZ(
                _seedHashCode,
                x * _detailScale * Mathf.Pow(_frequencyFactor, i) + ((i + 1) * 42),
                _seedHashCode,
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
            _seedHashCode,
            x * _detailScale / _frequencyFactor * 2 + _seed.GetHashCode() * 100,
            _seedHashCode,
            z * _detailScale / _frequencyFactor * 2 + _seed.GetHashCode() * 100);
        float remapped = result.Remap(0F, 1F, 0F, 10F);
        return Mathf.Round(remapped);
    }
}