using PlayerControls;

namespace AI
{
    public class Wolf : Animal
    {
        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            _agent.SetDestination(Player.Current.transform.position);
        }
    }
}