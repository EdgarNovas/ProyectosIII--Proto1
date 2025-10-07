using UnityEngine;
using UnityEngine.Events;

public class EnemyStateMachine : StateMachine
{
    // Referencias a componentes y al jugador
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public CharacterController Controller { get; private set; }
    [field: SerializeField] public float MovementSpeed { get; private set; } = 3f;
    [field: SerializeField] public float RotationSpeed { get; private set; } = 3f;
    [field: SerializeField] public float AttackRange { get; private set; } = 2f;
    [field: SerializeField] public float DetectionRange { get; private set; } = 6f;
    [field: SerializeField] public int Health { get; private set; } = 3;

    [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }

    [field: SerializeField] public ParticleSystem counterSystem { get; private set; }

    public bool IsInParryableWindow { get; set; } = false;

    public UnityEvent<EnemyStateMachine> OnDamage;

    void Awake()
    {
        // Creamos una instancia de cada estado que el enemigo pueda tener
        
        AddState(new EnemyIdleState(this));
        AddState(new EnemyAttackState(this));
        
        AddState(new EnemyRetreatState(this));
        AddState(new EnemyChaseState(this));
        AddState(new EnemyWaitForHitState(this));
        
        AddState(new EnemyHitState(this));
        /*
        AddState(new EnemyDeadState(this));
        */
    }

    void Start()
    {
        EnemyManager.Instance.AddEnemy(this);
        // El estado inicial del enemigo será Idle
        SwitchState(typeof(EnemyIdleState));
    }

    // Método para que el jugador le haga daño
    public void TakeDamage(int damage, Vector3 knockBack)
    {
        if (Health <= 0) { return; } // Ya está muerto

        Health -= damage;

        OnDamage?.Invoke(this);

        if (Health > 0)
        {
            EnemyHitState hitState = states[typeof(EnemyHitState)] as EnemyHitState;
            hitState.SetKnockback(knockBack.normalized);
            SwitchState(typeof(EnemyHitState));
        }
        else
        {
            EnemyManager.Instance.RemoveEnemy(this);
            //SwitchState(typeof(EnemyDeadState));
        }
    }

    public void TakeDamage(int damage)
    {
        if (Health <= 0) { return; } // Ya está muerto

        Health -= damage;

        OnDamage?.Invoke(this);

        if (Health > 0)
        {
            SwitchState(typeof(EnemyIdleState));
        }
        else
        {
            //SwitchState(typeof(EnemyDeadState));
        }
    }



    public bool IsAttackable()
    {
        return Health > 0;
    }
}
