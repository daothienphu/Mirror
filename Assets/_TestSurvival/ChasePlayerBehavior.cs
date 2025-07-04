using SpookyCore.Runtime.AI;
using SpookyCore.Runtime.EntitySystem;
using SpookyCore.Runtime.Systems;
using UnityEngine;

namespace _Test
{
    [CreateAssetMenu(menuName = "Test/Chase Player Behavior", fileName = "ChasePlayerBehavior")]
    public class ChasePlayerBehavior : AIBehavior
    {
        public override void OnStart(AIContext context)
        {
            RootNode = new FluentTreeBuilder()
                .Sequence("Player far from this")
                    .Condition("Distance check", IsPlayerFarFromThis)
                    .Execution("Move towards player", MoveToPlayer)
                .End()
                .Build();
        }

        private NodeState MoveToPlayer(AIContext ctx)
        {
            if (!GameManager.Instance.PlayerObservable.Value) return NodeState.Success;
            var playerPos = GameManager.Instance.PlayerObservable.Value.transform.position;
            ctx.Entity.Get<EntityMovement>().HeadTowardPosition(playerPos);
            return NodeState.Success;
        }

        private bool IsPlayerFarFromThis(AIContext ctx)
        {
            if (!GameManager.Instance.PlayerObservable.Value) return false;
            var playerPos = GameManager.Instance.PlayerObservable.Value.transform.position;
            var distance = (ctx.Entity.transform.position - playerPos).magnitude;
            return distance > 2f;
        }
    }
}