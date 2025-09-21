using UnityEngine;
using DG.Tweening;

public class CombatScript : MonoBehaviour
{
    [SerializeField] InputHandler controls;
    private RaycastHit info;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Physics.SphereCast(transform.position,3f,CalculateMovement(),out info,50,-1))
        {
            if(info.collider)
            {
                Gizmos.DrawWireSphere(info.collider.transform.position,2);
            }
            //If hit see if enemy is attackable

                //If attackable attack
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
