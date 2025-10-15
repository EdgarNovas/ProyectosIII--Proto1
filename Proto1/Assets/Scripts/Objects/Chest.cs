using UnityEngine;
using TMPro;
using UnityEngine.UI; 


public class Chest : MonoBehaviour
{
    private Door doorScript;

    [Header("Referencias")]
    public Transform player;            
    public TMP_Text interactText;
    public InputHandler Input;  

    [Header("Configuración")]
    public float distance = 2f;         

    private bool open = false;          

    void Start()
    {
       doorScript = FindObjectOfType<Door>();

        if (interactText != null)
            interactText.enabled = false; 
    }

    private void Update()
    {
        float dist = Vector3.Distance(transform.position, player.position);

        if (!open && dist <= distance)
        {
            if (interactText != null)
                interactText.enabled = true;

        }
        else
        {
            if (interactText != null)
                interactText.enabled = false;
        }
    }

    private void OnEnable()
    {
        Input.InteractionEvent += OpenChest;
    }

    private void OnDisable()
    {
        Input.InteractionEvent -= OpenChest;
    }

    void OpenChest()
    {
        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);

        if (!open && dist <= distance)
        {
            open = true;

            if (interactText != null)
                interactText.enabled = false;

            doorScript.AddKey();

        }

    }
}
