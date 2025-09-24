using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState : State
{
   protected PlayerStateMachine stateMachine;

   

    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;

    }
    
    protected void Move(Vector3 motion, float deltaTime)
    {
        stateMachine.Controller.Move((motion + stateMachine.ForceReceiver.Movement) * deltaTime);
    }
    
    //Another Move
    protected void Move(float deltaTime)
    {
        Move(Vector3.zero, deltaTime);
    }
    
    
    protected void FaceTarget(Transform target)
    {
        if(target == null) { return; }

        Vector3 enemyDirection = (target.transform.position - stateMachine.transform.position);

        enemyDirection.y = 0f;

        stateMachine.transform.rotation = Quaternion.LookRotation(enemyDirection * stateMachine.RotationSpeed);
    }

    /*
    protected void ReturnToLocomotion()
    {
        if(stateMachine.Targeter.CurrentTarget != null)
        {
            stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
        }
        else
        {
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
        }
    }
   
    */
}
