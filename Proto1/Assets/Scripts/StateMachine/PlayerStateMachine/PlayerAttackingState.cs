using UnityEngine;
using DG.Tweening;

public class PlayerAttackingState : PlayerBaseState
{

    private float duration = 0.7f; // Duración de la animación de ataque
    private EnemyScript target;

    public PlayerAttackingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        
        if (FindTarget())
        {
            // 2. Apuntar al objetivo y empezar la animación
            //FaceTarget(target.transform);
            //stateMachine.Animator.CrossFadeInFixedTime("AttackDash", 0.1f);
        }
        else
        {
            // 3. Si no hay objetivo, volver al estado anterior
            stateMachine.SwitchState( typeof( PlayerFreeLookState)); // O TargetingState
            return;
        }
        
    }

    public override void Tick(float deltaTime)
    {
        if (target == null) { return; }

        // Moverse hacia el objetivo durante el ataque
        MoveTowardsTarget(deltaTime);

        duration -= deltaTime;
        if (duration <= 0f)
        {
            // El ataque ha terminado, volvemos a un estado de locomoción
            //ReturnToLocomotion();
        }
    }

    public override void Exit()
    {
        
    }

    private bool FindTarget()
    {
        // Usamos SphereCast para encontrar un enemigo en frente
        RaycastHit hit;
        

        if (Physics.SphereCast(stateMachine.transform.position, 3f, CalculateMovement(), out hit, 50f))
        {
            if (hit.collider.TryGetComponent<EnemyScript>(out EnemyScript enemy) && enemy.IsAttackable())
            {
                this.target = enemy;
                return true;
            }
        }
        return false;
    }

    private void MoveTowardsTarget(float deltaTime)
    {
        stateMachine.transform.DOMove(target.transform.position, 15f)
            .SetSpeedBased(true)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                stateMachine.SwitchState(typeof(PlayerFreeLookState));
            });
    }

    Vector3 CalculateMovement()
    {
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        return forward * stateMachine.InputReader.MoveVector.y + right * stateMachine.InputReader.MoveVector.x;
    }
}
