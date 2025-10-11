using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ForceReceiver : MonoBehaviour
{
    [SerializeField] CharacterController controller;

    [SerializeField] NavMeshAgent agent;

    [SerializeField] float drag = 0.4f;

    Vector3 dampingVelocity;

    Vector3 impact;

    float verticalVelocity;

    [SerializeField] float playerGravity;

    public Vector3 Movement => impact + Vector3.up * verticalVelocity;

    private void Update()
    {
        Debug.Log(controller.isGrounded + " Is grounded");
        if (verticalVelocity < 0f && controller.isGrounded)
        {
            verticalVelocity = playerGravity * Time.deltaTime; ;
        }
        else
        {
            verticalVelocity += playerGravity * Time.deltaTime;
        }

        impact = Vector3.SmoothDamp(impact, Vector3.zero, ref dampingVelocity, drag);
        if (agent !=  null) { 

            if(impact.sqrMagnitude <  0.2f*0.2f)
            {
                impact = Vector3.zero;
                agent.enabled = true;
            }

        }

        controller.Move(Movement);
    }

    public void AddForce(Vector3 force)
    {
        impact += force;
        if(agent != null)
        {
            agent.enabled = false;
        }
       
    }


    public void Jump(float jumpForce)
    {
        verticalVelocity += jumpForce;
    }

    internal void NewReset()
    {
        impact = Vector3.zero;
        verticalVelocity = 0f;
    }
}
