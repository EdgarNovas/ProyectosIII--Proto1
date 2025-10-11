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
        
        if (GameManager.Instance.GetPlayer() == null) { return; }

        Vector3 lookPos = GameManager.Instance.GetPlayer().position - stateMachine.transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        stateMachine.transform.rotation = Quaternion.Slerp(stateMachine.transform.rotation, rotation, stateMachine.RotationSpeed * Time.deltaTime);
        
    }


}
