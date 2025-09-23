using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    [field: SerializeField] public InputHandler InputReader { get; private set; }

    [field: SerializeField] public EnemyScript CurrentTarget { get; private set; }

    [field: SerializeField] public CharacterController Controller { get; private set; }

    [field: SerializeField] public Animator Animator { get; private set; }

    [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }

 //   [field: SerializeField] public Targeter Targeter { get; private set; }

  //  [field: SerializeField] public Health Health { get; private set; }

    [field: SerializeField] public float FreeLookMovementSpeed { get; private set; }

    [field: SerializeField] public float TargetingMovementSpeed { get; private set; }

    [field: SerializeField] public float RotationDamping { get; private set; }

    [field: SerializeField] public float DodgeDuration { get; private set; }

    [field: SerializeField] public float DodgeLength { get; private set; }

    [field: SerializeField] public float JumpForce { get; private set; }

   // [field: SerializeField] public WeaponDamage WeaponDamage { get; private set; }

  //  [field: SerializeField] public Ragdoll Ragdoll { get; private set; }

  //  [field: SerializeField] public LedgeDetector LedgeDetector { get; private set; }

  //  [field: SerializeField] public Attack[] Attacks { get; private set; }



    public float PreviousDodgeTime { get; private set; } = Mathf.NegativeInfinity;

    public Transform MainCameraTransform { get; private set; }

  

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        GameManager.Instance.AddPlayer(transform);
        MainCameraTransform = Camera.main.transform;

        AddState(new PlayerFreeLookState(this));
        AddState(new PlayerAttackingState(this));

        SwitchState(typeof(PlayerFreeLookState));
    }

    private void OnEnable()
    {
       // Health.OnTakeDamage += HandleTakeDamage;
       // Health.OnDie += HandleDie;
    }

    private void OnDisable()
    {
      //  Health.OnTakeDamage -= HandleTakeDamage;
       // Health.OnDie -= HandleDie;
    }

    void HandleTakeDamage()
    {
        //SwitchState(new PlayerImpactState(this));
    }

    void HandleDie()
    {
       // SwitchState(new PlayerDeadState(this));
    }

}
