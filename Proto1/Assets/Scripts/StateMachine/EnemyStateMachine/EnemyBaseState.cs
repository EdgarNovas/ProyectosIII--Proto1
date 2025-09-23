using UnityEngine;

public abstract class EnemyBaseState : State
{
    protected EnemyStateMachine stateMachine;



    public EnemyBaseState(EnemyStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;

    }

    protected void FacePlayer()
    {
        /*
        if (stateMachine.Player == null) { return; }

        Vector3 lookPos = stateMachine.Player.position - stateMachine.transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        stateMachine.transform.rotation = Quaternion.Slerp(stateMachine.transform.rotation, rotation, 10f * Time.deltaTime);
        */
    }


}
