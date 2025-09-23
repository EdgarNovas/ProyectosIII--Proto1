using UnityEngine;

public class EnemyRetreatState : EnemyBaseState
{
    private float retreatDuration = 1.5f;
    public EnemyRetreatState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        
    }

    public override void Tick(float deltaTime)
    {
        FacePlayer();

        // Moverse hacia atrás
        Vector3 direction = (stateMachine.transform.position - GameManager.Instance.GetPlayer().position).normalized;
        stateMachine.Controller.Move(direction * stateMachine.MovementSpeed * deltaTime);

        retreatDuration -= deltaTime;
        if (retreatDuration <= 0f)
        {
            // Volver al estado Idle
            stateMachine.SwitchState(typeof(EnemyIdleState));
        }
    }

    public override void Exit()
    {
        
    }

  
}
