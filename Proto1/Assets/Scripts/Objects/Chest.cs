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
    public GameObject chestUP;

    [Header("Configuración")]
    public float distance = 2f;

    [Header("Animación de apertura")]
    public float openAngle = 90f;
    public float openSpeed = 2f;
    private Quaternion chestClosedRot;
    private Quaternion chestOpenRot;
    private bool isOpening = false;

    private bool open = false;    
    
    void Start()
    {
       doorScript = FindObjectOfType<Door>();

        if (interactText != null)
            interactText.enabled = false;

        if (chestUP != null) chestClosedRot = chestUP.transform.localRotation;
        if (chestUP != null) chestClosedRot = chestUP.transform.localRotation;
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

        if (isOpening)
        {
            if (chestUP != null)
                chestUP.transform.localRotation = Quaternion.Slerp(chestUP.transform.localRotation, chestOpenRot, Time.deltaTime * openSpeed);
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

            if (chestUP != null && chestUP != null)
            {
                chestOpenRot = Quaternion.Euler(-openAngle, 0, 0) * chestClosedRot;
                isOpening = true;
            }
        }

    }
}
