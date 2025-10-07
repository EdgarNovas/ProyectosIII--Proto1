using UnityEngine;

public class EnemyHitState : EnemyBaseState
{
    private float stunDuration = 2f;
    private float currentTimeStun = 0f;
    private float drag = 0.7f;
    private Vector3 knockbackDirection;
    private Vector3 dampingVelocity;
    

    public EnemyHitState(EnemyStateMachine stateMachine) : base(stateMachine){ }

    public void SetKnockback(Vector3 direction)
    {
        knockbackDirection = direction;
    }

    public override void Enter()
    {
        Debug.Log("Entered hitstate");
        currentTimeStun = 0f;
        stateMachine.GetComponent<MeshRenderer>().material.color = Color.red;
        
    }

    public override void Tick(float deltaTime)
    {
        
        knockbackDirection = Vector3.SmoothDamp(knockbackDirection, Vector3.zero, ref dampingVelocity, drag);
        stateMachine.Controller.Move((knockbackDirection *10)* deltaTime);

        currentTimeStun += deltaTime;
        if (currentTimeStun >= stunDuration)
        {
            // e.g. go back to idle/patrol/whatever
            stateMachine.SwitchState(typeof(EnemyIdleState));
        }

    }

    public override void Exit()
    {
        
    }

   
}
