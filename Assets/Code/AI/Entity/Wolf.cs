using PlayerControls;
using UnityEngine;

namespace AI
{
    public class Wolf : Entity
    {
        protected override void Awake()
        {
            base.Awake();
            MoveSpeed = MoveSpeedType.Chase;
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            if (Vector3.Distance(Spawner.transform.position, Player.Current.transform.position) < 30)
                _agent.SetDestination(Player.Current.transform.position);
            else
                _agent.SetDestination(Spawner.transform.position);
        }
    }
}