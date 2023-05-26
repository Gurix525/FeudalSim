namespace AI
{
    public class Boar : Animal
    {
        protected override void CreateAttitudeModels()
        {
            AddAttitude((typeof(Boar), AttitudeType.Friendly, () => 10F));
            AddAttitude((typeof(Animal), AttitudeType.Hostile, () => 100F));
        }

        protected override void CreateBehaviours()
        {
        }
    }
}