using System.Collections;
using UnityEngine;

public class JumpAttack : MonoBehaviour
{
    public JumpAttackType state = JumpAttackType.Coroutine;
    
    public Transform target;
    //Coroutine
    public float duration = .2f;
    public float height = 3f;

    //Rigidbody jump
    public float arcHeight = 5f;
    Rigidbody rb;
    public enum JumpAttackType
    {
        Coroutine,
        Rigidbody
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
       
    }

    void Start()
    {
        if (state == JumpAttackType.Coroutine)
        {
            StartCoroutine(JumpArc(transform.position, target.position, duration));
        }
        else
            Jump();


    }

    IEnumerator JumpArc(Vector3 start, Vector3 end, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float t = elapsed / duration;

            // Parabolic arc using Sin
            float yOffset = height * Mathf.Sin(Mathf.PI * t);

            transform.position = Vector3.Lerp(start, end, t) + Vector3.up * yOffset;

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = end; // Snap to exact end
    }

    void Jump()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = target.position;

        float gravity = Physics.gravity.y;

        // Split horizontal and vertical distances
        Vector3 horizontal = new Vector3(endPos.x - startPos.x, 0f, endPos.z - startPos.z);
        float heightDifference = endPos.y - startPos.y;

        float timeToApex = Mathf.Sqrt(-2f * arcHeight / gravity);
        float totalTime = timeToApex + Mathf.Sqrt(2f * (arcHeight - heightDifference) / -gravity);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2f * gravity * arcHeight);
        Vector3 velocityXZ = horizontal / totalTime;

        Vector3 finalVelocity = velocityXZ + velocityY;

        rb.linearVelocity = finalVelocity;
    }
}
