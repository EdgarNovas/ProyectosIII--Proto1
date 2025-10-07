using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    [field: SerializeField] public InputHandler InputReader { get; private set; }

    [field: SerializeField] public EnemyScript CurrentTarget { get; private set; }

    [field: SerializeField] public CharacterController Controller { get; private set; }

    [field: SerializeField] public Animator Animator { get; private set; }

    [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }

    [field: SerializeField] public CinemachineCamera camera_CM { get; private set; }

//   [field: SerializeField] public Targeter Targeter { get; private set; }

//  [field: SerializeField] public Health Health { get; private set; }

[field: SerializeField] public float FreeLookMovementSpeed { get; private set; }

    [field: SerializeField] public float TargetingMovementSpeed { get; private set; }

    [field: SerializeField] public float RotationDamping { get; private set; }

    [field: SerializeField] public float RotationSpeed { get; private set; } = 3f;

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

    public void StartCameraShake(float duration)
    {
        StartCoroutine(ShakeRoutine(duration));
    }

    IEnumerator HitStop(float duration)
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1f;
    }

    public void StartHitStop(float duration)
    {
        StartCoroutine(HitStop(duration));
    }


    public IEnumerator ShakeRoutine(float duration)
    {
        camera_CM.GetComponent<CinemachineBasicMultiChannelPerlin>().AmplitudeGain = 5f;
        camera_CM.GetComponent<CinemachineBasicMultiChannelPerlin>().FrequencyGain = 2f;
        float elapsed = 0f;


        // Gradually reduce shake over time
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            


            yield return null;
        }

        camera_CM.GetComponent<CinemachineBasicMultiChannelPerlin>().AmplitudeGain = 0f;
        camera_CM.GetComponent<CinemachineBasicMultiChannelPerlin>().FrequencyGain = 0f;
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
