using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    private bool hasAttacked = false;

    private float windupDuration = 0.5f;
    
    private float parryableDuration = 0.2f;
    private float timer;
    private bool attackCompleted = false;

    public EnemyAttackState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
       
        attackCompleted = false;
        hasAttacked = false;
        timer = windupDuration;
        stateMachine.IsInParryableWindow = false; // Asegurarse de que empieza en false
        EnemyManager.Instance.ReportParryableStatus(false);
        //stateMachine.Animator.CrossFadeInFixedTime("Run", 0.1f);


    }

    public override void Tick(float deltaTime)
    {
        Debug.Log("Attacked");
        FacePlayer();
        
        
        timer -= deltaTime; 
                            

        if (timer <= 0 && !stateMachine.IsInParryableWindow)
        {
            // La preparación (windup) ha terminado, empieza la ventana de parry
            stateMachine.IsInParryableWindow = true;
            EnemyManager.Instance.ReportParryableStatus(true);
            TryToHitPlayer();
            timer = parryableDuration;
        }
        else if (stateMachine.IsInParryableWindow && timer <= 0)
        {
            // La ventana de parry ha terminado, el ataque se completa
            attackCompleted = true;
            
            // Después del ataque, nos retiramos
            stateMachine.SwitchState(typeof(EnemyRetreatState));
        }

    }

    public override void Exit()
    {
        stateMachine.IsInParryableWindow = false;

        EnemyManager.Instance.ReportParryableStatus(false);

    }


    private void TryToHitPlayer()
    {
        // Primero, comprobamos si el jugador está en nuestro rango de ataque
        float distanceToPlayer = Vector3.Distance(stateMachine.transform.position, GameManager.Instance.GetPlayer().position);
        if (distanceToPlayer > stateMachine.AttackRange)
        {
            // El jugador esquivó o se alejó, el golpe falla.
            return;
        }

        // Si está en rango, le aplicamos el daño y el knockback.
        if (GameManager.Instance.GetPlayer().TryGetComponent<PlayerStateMachine>(out PlayerStateMachine player))
        {
            // 1. Calculamos la dirección del empujón (del enemigo hacia el jugador)
            Vector3 knockbackDirection = (player.transform.position - stateMachine.transform.position).normalized;
            float knockbackStrength = 5f; 

            // 2. Llamamos al método público del jugador
            player.TakeDamage(10, knockbackDirection * knockbackStrength);
            return;
        }
    }

}
