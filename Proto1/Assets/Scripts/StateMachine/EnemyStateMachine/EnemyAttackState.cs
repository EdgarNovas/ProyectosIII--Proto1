using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    private bool hasAttacked = false;
    public EnemyAttackState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("Enemy Attack State");

        hasAttacked = false;
        //stateMachine.Animator.CrossFadeInFixedTime("Run", 0.1f);
    }

    public override void Tick(float deltaTime)
    {
        FacePlayer();

        float distance = Vector3.Distance(stateMachine.transform.position, GameManager.Instance.GetPlayer().position);

        if (distance > stateMachine.AttackRange)
        {
            // Moverse hacia el jugador
            Vector3 direction = (GameManager.Instance.GetPlayer().position - stateMachine.transform.position).normalized;
            stateMachine.Controller.Move(direction * stateMachine.MovementSpeed * 2 * deltaTime);
        }
        else if (!hasAttacked)
        {
            // Atacar y cambiar a estado de retirada
            hasAttacked = true;
            //stateMachine.Animator.SetTrigger("AirPunch");
            // Después del ataque, nos retiramos
            //stateMachine.SwitchState(typeof(EnemyRetreatState));
        }
    }

    public override void Exit()
    {
        
    }

   
}
