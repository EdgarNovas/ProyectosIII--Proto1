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
        /*
        if (lastEnemy)
        {
            GameManager.Instance.TriggerLastHitCamera
                (
                GameManager.Instance.GetPlayer(), stateMachine.transform
                );
        }

        */

        stateMachine.Animator.CrossFadeInFixedTime("Block", 0.1f);
    }

    public override void Tick(float deltaTime)
    {
        FacePlayer();
    }

    public override void Exit()
    {
        
    }


}
