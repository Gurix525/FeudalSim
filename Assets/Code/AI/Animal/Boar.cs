namespace AI
{
    public class Boar : Animal
    {
        protected override void CreateAttitudeModels()
        {
            AddAttitude((typeof(Boar), AttitudeType.Friendly, (target) => 10F));
            AddAttitude((typeof(Animal), AttitudeType.Hostile, (target) => 100F));
        }
    }
}