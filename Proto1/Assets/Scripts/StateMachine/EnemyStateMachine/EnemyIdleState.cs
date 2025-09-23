using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    private float nextMoveTime = 0.5f;
    private Vector3 moveDirection;

    public EnemyIdleState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        
        
    }

    public override void Tick(float deltaTime)
    {
        DecideNextMove();
        
    }

    public override void Exit()
    {
        
    }

    private void DecideNextMove()
    {
        // 50% de probabilidad de moverse o quedarse quieto
        if (Random.Range(0, 2) == 1)
        {
            // Moverse a la derecha o a la izquierda
            Vector3 perpendicular = Quaternion.Euler(0, 90, 0) * (GameManager.Instance.GetPlayer().position - stateMachine.transform.position).normalized;
            moveDirection = Random.Range(0, 2) == 1 ? perpendicular : -perpendicular;
        }
        else
        {
            moveDirection = Vector3.zero;
        }

        // Establecer un temporizador para la próxima decisión
        nextMoveTime = Time.time + Random.Range(1f, 2.5f);
    }


}
