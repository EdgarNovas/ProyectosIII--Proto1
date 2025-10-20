using UnityEngine;

public class EnemyChaseState : EnemyBaseState
{
    //private readonly int RunHash = Animator.StringToHash("Run");
    float currentWindUpTime = 0f;
    float windUpTime = 0.5f;

    public EnemyChaseState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.counterSystem.Play();
        stateMachine.IsInParryableWindow = true;
        currentWindUpTime = windUpTime;
        EnemyManager.Instance.ReportParryableStatus(true);
        stateMachine.Animator.CrossFadeInFixedTime("FlyKick", 0.1f);
    }

    public override void Tick(float deltaTime)
    {
        FacePlayer();


        currentWindUpTime -= deltaTime;
        if (currentWindUpTime > 0f) { return; }

        float distance = Vector3.Distance(stateMachine.transform.position, GameManager.Instance.GetPlayer().position);

        if (distance > stateMachine.AttackRange)
        {
            // ------------------------------------
            // Moverse hacia el jugador
            Vector3 direction = (GameManager.Instance.GetPlayer().position - stateMachine.transform.position).normalized;
            stateMachine.Controller.Move(direction * stateMachine.MovementAttackSpeed * 4 * deltaTime);
        }
        else if (distance < stateMachine.AttackRange)
        {
            TryToHitPlayer();
            //stateMachine.Animator.SetTrigger("AirPunch");
            // Después del ataque, nos retiramos
            stateMachine.SwitchState(typeof(EnemyAttackState));
        }
        else if(distance > stateMachine.DetectionRange)
        {
            stateMachine.SwitchState(typeof(EnemyIdleState));
        }
    }

    public override void Exit()
    {
        stateMachine.counterSystem.Stop();
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
