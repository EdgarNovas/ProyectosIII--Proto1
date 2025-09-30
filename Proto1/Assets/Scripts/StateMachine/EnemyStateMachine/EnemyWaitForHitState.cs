using UnityEngine;

public class EnemyWaitForHitState : EnemyBaseState
{
    public EnemyWaitForHitState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.GetComponent<MeshRenderer>().material.color = Color.yellow;
    }

    public override void Tick(float deltaTime)
    {

    }

    public override void Exit()
    {
        Debug.Log("eXITED WAITING STATE");
    }


}
