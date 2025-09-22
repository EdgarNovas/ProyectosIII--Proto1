using UnityEngine;
using DG.Tweening;

public class CombatScript : MonoBehaviour
{
    [SerializeField] InputHandler controls;
    [SerializeField] private EnemyScript currentTarget;
    private RaycastHit info;

    void Start()
    {
        
    }

    void Update()
    {
        if(controls.IsAttacking)
        {
            if (Physics.SphereCast(transform.position, 3f, CalculateMovement(), out info, 50, -1))
            {
                if (info.collider.transform.GetComponent<EnemyScript>().IsAttackable())
                    currentTarget = info.collider.transform.GetComponent<EnemyScript>();
                
                //If hit see if enemy is attackable

                //If attackable attack
            }
        }
        
        Debug.DrawRay(transform.position, CalculateMovement(),Color.red) ;
    }

    Vector3 CalculateMovement()
    {
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        return forward * controls.MoveVector.y + right * controls.MoveVector.x;
    }
}
