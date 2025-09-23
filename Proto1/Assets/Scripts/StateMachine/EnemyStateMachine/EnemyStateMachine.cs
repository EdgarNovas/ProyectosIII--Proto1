using UnityEngine;

public class EnemyStateMachine : StateMachine
{
    // Referencias a componentes y al jugador
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public CharacterController Controller { get; private set; }
    [field: SerializeField] public float MovementSpeed { get; private set; } = 3f;
    [field: SerializeField] public float AttackRange { get; private set; } = 2f;
    [field: SerializeField] public int Health { get; private set; } = 3;


    
    void Awake()
    {
        // Creamos una instancia de cada estado que el enemigo pueda tener
        
        AddState(new EnemyIdleState(this));
        AddState(new EnemyAttackState(this));
        /*
        AddState(new EnemyRetreatState(this));
        AddState(new EnemyHitState(this));
        AddState(new EnemyDeadState(this));
        */
    }

    void Start()
    {
        EnemyManager.Instance.AddEnemy(this);
        // El estado inicial del enemigo ser� Idle
        SwitchState(typeof(EnemyIdleState));
    }

    // M�todo para que el jugador le haga da�o
    public void TakeDamage(int damage)
    {
        if (Health <= 0) { return; } // Ya est� muerto

        Health -= damage;

        if (Health > 0)
        {
            //SwitchState(typeof(EnemyHitState));
        }
        else
        {
            //SwitchState(typeof(EnemyDeadState));
        }
    }
}
