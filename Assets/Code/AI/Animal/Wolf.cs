namespace AI
{
    public class Wolf : Animal
    {
        protected override void CreateAttitudeModels()
        {
            _attitudeModels.Add((typeof(Wolf), AttitudeType.Friendly, () => 10F));
            _attitudeModels.Add((typeof(Animal), AttitudeType.Hostile, () => 100F));
        }
    }
}