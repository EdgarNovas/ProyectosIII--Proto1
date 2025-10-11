using UnityEngine;

public class EnemyWaitForHitState : EnemyBaseState
{
    private bool lastEnemy = false;
    public EnemyWaitForHitState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.GetComponent<MeshRenderer>().material.color = Color.yellow;
        lastEnemy = EnemyManager.Instance.IsOnlyEnemy();
        if (lastEnemy)
        {
            GameManager.Instance.TriggerLastHitCamera
                (
                GameManager.Instance.GetPlayer(), stateMachine.transform
                );
        }
    }

    public override void Tick(float deltaTime)
    {

    }

    public override void Exit()
    {
        
    }


}
