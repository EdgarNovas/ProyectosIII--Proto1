using UnityEngine;
using DG.Tweening;

public class PlayerAttackingState : PlayerBaseState
{

    private float duration = 0.7f; // Duración de la animación de ataque
    private EnemyScript target;
    float stopDistance = 1.5f; // tweak this depending on collider sizes
    


    public PlayerAttackingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        
        if (FindTarget())
        {
            //TODO: AVISAR AL ENEMY MANAGER DE CAMBIAR EL ESTADO 
            // DEL ENEMIGO(TARGET) A ESPERAR POR EL GOLPE DEL PLAYER
            // Apuntar al objetivo y empezar la animación
            FaceTarget(target.transform);
            MoveTowardsTarget();
            //stateMachine.Animator.CrossFadeInFixedTime("AttackDash", 0.1f);
        }
        else
        {
            // Si no hay objetivo, volver al estado anterior
            stateMachine.SwitchState( typeof( PlayerFreeLookState)); // O TargetingState
            return;
        }
        
    }

    public override void Tick(float deltaTime)
    {
       
       
    }

    public override void Exit()
    {
        target = null;
        
    }

    private bool FindTarget()
    {
        // Usamos SphereCast para encontrar un enemigo en frente
        //Y que el jugador no tenga que clavar el raycast
        RaycastHit hit;
        

        if (Physics.SphereCast(stateMachine.transform.position, 3f, CalculateMovement(), out hit, 50f))
        {
            if (hit.collider.TryGetComponent<EnemyScript>(out EnemyScript enemy) && enemy.IsAttackable())
            {
                target = enemy;
                return true;
            }
        }
        return false;
    }

    private void MoveTowardsTarget()
    {
        Vector3 directionToPlayer = (stateMachine.transform.position - target.transform.position).normalized;
        Vector3 targetPos = target.transform.position + directionToPlayer * stopDistance;
        

        stateMachine.transform.DOMove(targetPos, 15f)
            .SetSpeedBased(true)
            .SetEase(Ease.Flash)
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
