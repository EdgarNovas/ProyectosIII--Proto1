using UnityEngine;
using DG.Tweening;
using Unity.Cinemachine;

public class PlayerAttackingState : PlayerBaseState
{
    float defaultFOV;
    private float duration = 0.7f; // Duración de la animación de ataque
    private EnemyStateMachine target;
    float stopDistance = 1.5f; // tweak this depending on collider sizes
    float stopHitTime = 0.1f;
    private float attackTriggerDistance = 5f;
    bool hasAttacked = false;

    public PlayerAttackingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        defaultFOV = stateMachine.camera_CM.Lens.FieldOfView;
        if (FindTarget())
        {
            
            FaceTarget(target.transform);
            EnemyManager.Instance.PrepareEnemyForHit(target);
            MoveTowardsTarget();
            
            //stateMachine.Animator.CrossFadeInFixedTime("Slash", 0.1f);
            stateMachine.Animator.CrossFadeInFixedTime("PunchFly", 0.03f);

        }
        else
        {
            // Si no hay objetivo, volver al estado anterior
            stateMachine.SwitchState( typeof( PlayerFreeLookState)); // O TargetingState
            return;
        }
        
    }

    public override void Tick(float deltaTime)
    {
        if (stateMachine.camera_CM.Lens.FieldOfView < defaultFOV + 40)
        {
            stateMachine.camera_CM.Lens.FieldOfView += 20 * deltaTime;
        }
        if(target != null)
        {
            float distance = Vector3.Distance(stateMachine.transform.position, target.transform.position);
       
            if (distance > attackTriggerDistance)
            {
                FaceTarget(target.transform);
            }
            else if (distance < attackTriggerDistance && !hasAttacked)
            {
                SoundManager.Instance.PlaySound(stateMachine.attackSound);
                int attackNum = Random.Range(0, 3);
                if(attackNum == 0)
                {
                    stateMachine.Animator.CrossFadeInFixedTime("Slash", 0.1f);
                }
                else if(attackNum == 1)
                {
                    stateMachine.Animator.CrossFadeInFixedTime("Punch", 0.1f);
                }
                else
                {
                    stateMachine.Animator.CrossFadeInFixedTime("360Slash", 0.1f);
                }

                hasAttacked = true;
            }
        }
    }

    public override void Exit()
    {
        target = null;
        hasAttacked = false;
        stateMachine.camera_CM.Lens.FieldOfView = defaultFOV;
    }

    private bool FindTarget()
    {
        // Usamos SphereCast para encontrar un enemigo en frente
        //Y que el jugador no tenga que clavar el raycast
        RaycastHit hit;
        Vector3 moveDirection = CalculateMovement();
        if (Equals( moveDirection, Vector3.zero))
        {
            EnemyStateMachine closestEnemy = null;
            float closestDistance = Mathf.Infinity;
            
            foreach(EnemyStateMachine enemy in EnemyManager.Instance.GetEnemys())
            {
                if (enemy == null) continue; // Skip destroyed enemies
                float distance = Vector3.Distance(stateMachine.transform.position, enemy.transform.position);
                if(distance < closestDistance && distance < 10f)
                {
                    closestDistance = distance;
                    closestEnemy = enemy;
                }
            }

            if (closestEnemy != null)
            {
               target = closestEnemy;
                return true;
            }


        }
        else
        {
            if (Physics.SphereCast(stateMachine.transform.position, 3f, moveDirection, out hit, 10f))
            {
                if (hit.collider.TryGetComponent<EnemyStateMachine>(out EnemyStateMachine enemy) && enemy.IsAttackable())
                {
                    target = enemy;
                    return true;
                }
            }
        }

       
        return false;
    }

    private void MoveTowardsTarget()
    {
        Vector3 directionToPlayer = (stateMachine.transform.position - target.transform.position).normalized;
        Vector3 targetPos = target.transform.position + directionToPlayer * stopDistance;
        

        stateMachine.transform.DOMove(targetPos, 15f)
            .SetSpeedBased(true)
            .SetEase(Ease.Flash)
            .OnComplete(() =>
            {
                
                stateMachine.camera_CM.Lens.FieldOfView = defaultFOV;
                Vector3 knockback = (target.transform.position - GameManager.Instance.GetPlayer().position).normalized;
                stateMachine.StartHitStop(stopHitTime);
                stateMachine.StartCameraShake(.2f);
                target.TakeDamage(1, knockback);
                DOVirtual.DelayedCall(.2f, () =>
                {
                    stateMachine.SwitchState(typeof(PlayerFreeLookState));
                });
                
            });


    }

    Vector3 CalculateMovement()
    {
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        return forward * stateMachine.InputReader.MoveVector.y + right * stateMachine.InputReader.MoveVector.x;
    }


}
