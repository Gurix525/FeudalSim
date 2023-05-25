namespace AI
{
    public class Boar : Animal
    {
        protected override void CreateAttitudeModels()
        {
            _attitudeModels.Add((typeof(Boar), AttitudeType.Friendly, () => 10F));
            _attitudeModels.Add((typeof(Animal), AttitudeType.Hostile, () => 100F));
        }
    }
}