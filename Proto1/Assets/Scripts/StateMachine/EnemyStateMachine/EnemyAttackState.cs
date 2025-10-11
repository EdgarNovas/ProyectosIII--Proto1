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
        Debug.Log("Enemy Attack State");
        attackCompleted = false;
        hasAttacked = false;
        timer = windupDuration;
        stateMachine.IsInParryableWindow = false; // Asegurarse de que empieza en false
        //stateMachine.Animator.CrossFadeInFixedTime("Run", 0.1f);


    }

    public override void Tick(float deltaTime)
    {

        if (attackCompleted) { return; }

        FacePlayer();

        // --- L�NEA CLAVE FALTANTE ---
        timer -= deltaTime; // <-- A�ADE ESTA L�NEA AQU�
                            // ----------------------------

        if (timer <= 0 && !stateMachine.IsInParryableWindow)
        {
            // La preparaci�n (windup) ha terminado, empieza la ventana de parry
            stateMachine.IsInParryableWindow = true;
            timer = parryableDuration;
        }
        else if (stateMachine.IsInParryableWindow && timer <= 0)
        {
            // La ventana de parry ha terminado, el ataque se completa
            attackCompleted = true;
            stateMachine.IsInParryableWindow = false;
            // Despu�s del ataque, nos retiramos
            stateMachine.SwitchState(typeof(EnemyRetreatState));
        }

    }

    public override void Exit()
    {
        
    }

   
}
