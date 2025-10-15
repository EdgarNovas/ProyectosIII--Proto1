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
            // Atacar y cambiar a estado de retirada
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
    }


}
