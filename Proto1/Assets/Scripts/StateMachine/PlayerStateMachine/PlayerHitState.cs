using UnityEngine;

public class PlayerHitState : PlayerBaseState
{
    private readonly int HitHash = Animator.StringToHash("BodyBlow");
    private float stunDuration = 1f; // Stun de 1 segundo como pediste
    private float resetStunDuration = 1f; // Stun de 1 segundo como pediste

    public PlayerHitState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stunDuration = resetStunDuration; 
        stateMachine.Animator.CrossFadeInFixedTime(HitHash, 0.1f);
    }

    public override void Tick(float deltaTime)
    {
        // El único trabajo en Tick es mover al jugador con las fuerzas externas (knockback)
        // y contar el tiempo del stun.
        Move(deltaTime); // Esto aplica la fuerza del ForceReceiver

        stunDuration -= deltaTime;
        if (stunDuration <= 0f)
        {
            // Cuando el tiempo se acaba, volvemos a un estado normal
            stateMachine.SwitchState(typeof(PlayerFreeLookState));
        }
    }

    public override void Exit()
    {
        
    }



}
