public class Cell
{
    private float _height;
    private float _steepness;

    public float Height => _height;
    public float Steepness => _steepness;

    public Cell(float height, float steepness = 0F)
    {
        _height = height;
        _steepness = steepness;
    }

    public void SetHeight(float height)
    {
        _height = height;
    }

    public void SetSteepness(float steepness)
    {
        _steepness = steepness;
    }

    public override string ToString()
    {
        return $"{Height}";
    }
}