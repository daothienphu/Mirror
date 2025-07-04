using SpookyCore.Runtime.AI;
using SpookyCore.Runtime.EntitySystem;
using SpookyCore.Runtime.Systems;
using UnityEngine;

namespace _Test
{
    [CreateAssetMenu(menuName = "Test/Attack Player Behavior", fileName = "AttackPlayerBehavior")]
    public class AttackPlayerBehavior : AIBehavior
    {
        public override void OnStart(AIContext context)
        {
            RootNode = new FluentTreeBuilder()
                .Sequence("Is near player")
                .Condition("Is near player", IsNearPlayer)
                .Execution("Attack player", AttackPlayer)
                .End()
                .Build();

        }

        private NodeState AttackPlayer(AIContext ctx)
        {
            if (!GameManager.Instance.PlayerObservable.Value) return NodeState.Success;
            ctx.Entity.Get<EntityMovement>().StopAllMovement();
            var player = GameManager.Instance.PlayerObservable.Value;
            var attack = ctx.Entity.Get<EntityAttack>();
            attack.AttackTarget(player);
            return NodeState.Success;
        }

        private bool IsNearPlayer(AIContext ctx)
        {
            if (!GameManager.Instance.PlayerObservable.Value) return false;
            var playerPos = GameManager.Instance.PlayerObservable.Value.transform.position;
            var distance = (ctx.Entity.transform.position - playerPos).magnitude;
            return distance <= 2f;
        }
    }
}