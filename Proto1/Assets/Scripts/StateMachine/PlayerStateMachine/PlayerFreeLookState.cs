using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public class PlayerFreeLookState : PlayerBaseState
{
    private bool shouldFade;

    private readonly int FreeLookSpeedHash = Animator.StringToHash("FreeLookSpeed");

    private readonly int FreeLookBlendTreeHash = Animator.StringToHash("FreeLookBlendTree");


    private const float CrossFadeDuration = 0.1f;

    private const float AnimatorDampTime = 0.1f;

    public PlayerFreeLookState(PlayerStateMachine stateMachine, bool shouldFade = true) : base(stateMachine)
    { 
        this.shouldFade = shouldFade;
    }


    public override void Enter()
    {
        stateMachine.InputReader.TargetEvent += OnTarget;

        stateMachine.InputReader.JumpEvent += OnJump;

        stateMachine.InputReader.DodgeEvent += OnDodge;

        //stateMachine.Animator.SetFloat(FreeLookSpeedHash, 0);
        /*
        if (shouldFade)
        {
            stateMachine.Animator.CrossFadeInFixedTime(FreeLookBlendTreeHash, CrossFadeDuration);
        }
        else
        {
            stateMachine.Animator.Play(FreeLookBlendTreeHash);
        }

        */
    }

  

    public override void Tick(float deltaTime)
    {
        
        if (stateMachine.InputReader.IsAttacking)
        {
            stateMachine.SwitchState(typeof( PlayerAttackingState));

            return;
        }

        Vector3 movement = CalculateMovement();



        //Move(movement * stateMachine.FreeLookMovementSpeed, deltaTime);
        /*
        if (stateMachine.InputReader.MovementValue == Vector2.zero)
        {
            stateMachine.Animator.SetFloat(FreeLookSpeedHash, 0, AnimatorDampTime, deltaTime);
            return;
        }
        
        stateMachine.Animator.SetFloat(FreeLookSpeedHash, 1, AnimatorDampTime, deltaTime);
        */
        if(!Vector3.Equals(movement, Vector3.zero))
        {
            FaceMovementDirection(movement, deltaTime);
        }
        

        Move(movement * stateMachine.FreeLookMovementSpeed, deltaTime);
    }

    public override void Exit()
    {
        stateMachine.InputReader.TargetEvent -= OnTarget;

        stateMachine.InputReader.JumpEvent -= OnJump;

        stateMachine.InputReader.DodgeEvent -= OnDodge;
    }

    private void FaceMovementDirection(Vector3 movement, float deltaTime)
    {
        stateMachine.transform.rotation = Quaternion.Lerp(
            stateMachine.transform.rotation,
            Quaternion.LookRotation(movement),
            deltaTime * stateMachine.RotationDamping);
        
    }

    Vector3 CalculateMovement()
    {
        Vector3 forward = stateMachine.MainCameraTransform.forward;
        Vector3 right = stateMachine.MainCameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        return forward * stateMachine.InputReader.MoveVector.y + right * stateMachine.InputReader.MoveVector.x;
    }

    void OnTarget()
    {
       // if (!stateMachine.Targeter.SelectTarget()) { return; }

       // stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
    }

    private void OnDodge()
    {
        if (stateMachine.InputReader.MoveVector == Vector2.zero) { return; }

       //stateMachine.SwitchState(new PlayerDodgingState(stateMachine,  stateMachine.InputReader.MovementValue, true));
    }


    private void OnJump()
    {
        //Jump

        //stateMachine.SwitchState(new PlayerJumpingState(stateMachine));

    }

}
