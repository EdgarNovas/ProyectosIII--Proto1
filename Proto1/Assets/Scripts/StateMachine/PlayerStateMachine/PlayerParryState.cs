using UnityEngine;

public class PlayerParryState : PlayerBaseState
{
    private float parryWindowDuration = 0.5f; // La duración que pediste
    private bool parrySuccessful = false;
    private float parryRange = 1.5f;
    public PlayerParryState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        
        parryWindowDuration = 0.5f;
        parrySuccessful = false;
        stateMachine.Animator.CrossFadeInFixedTime("Parry", 0.1f);
    }

    public override void Tick(float deltaTime)
    {
        if (parrySuccessful) { return; }

        // Comprobar si hay un enemigo "parreable"
        EnemyStateMachine parryTarget = EnemyManager.Instance.GetParryableEnemy(stateMachine.transform, 360f,parryRange); // Ángulo de parry de 360°

        if (parryTarget != null)
        {
            FaceTargetInstant(parryTarget);
            OnParrySuccess(parryTarget);
            parrySuccessful = true;
            
            // Opcional: podrías quedarte en el estado un poco más para que la animación termine
            
        }

        parryWindowDuration -= deltaTime;
        if (parryWindowDuration <= 0f)
        {
            // Se acabó el tiempo del parry y no tuvimos éxito, volvemos a la normalidad
            stateMachine.SwitchState(typeof(PlayerFreeLookState));
        }
    }


    public override void Exit()
    {
        
    }

    public void OnParrySuccess(EnemyStateMachine enemy)
    {
        // ATURDIR AL ENEMIGO ---
        // Le ordenamos al enemigo que entre en su estado de vulnerabilidad.
        // Es mucho mejor usar un 'StunnedState' que un simple 'HitState' 
        // porque la recompensa (una ventana de ataque más larga) es mayor.
        Vector3 knockBack = enemy.transform.position - stateMachine.transform.position;
        enemy.TakeDamage(0, knockBack*2);
        enemy.SwitchState(typeof(EnemyHitState));

        //FEEDBACK

        // a) Sonido: El más importante. Un "CLANG!" metálico y satisfactorio.
        // AudioManager.Instance.Play("ParrySuccessSound"); // (Si tienes un AudioManager)

        // b) Partículas: Un destello visual en el punto de impacto.

        stateMachine.ParryParticle.Play();
      

        // c) Hit Stop / Slow Motion: El truco secreto para que el impacto se sienta pesado.
        stateMachine.StartHitStopParryEffect(0.1f); // Congelamos el tiempo por 0.1 segundos.


        // --- 3. EL FLUJO: LIBERAR AL JUGADOR ---
        // Cambiamos inmediatamente al estado de locomoción para que el jugador
        // pueda moverse y contraatacar al enemigo aturdido sin demora.
        stateMachine.SwitchState(typeof(PlayerFreeLookState));
        return;
    }

   

}
