using UnityEngine;
using TMPro;
using UnityEngine.UI; 

public class Chest : MonoBehaviour
{
    [Header("Referencias")]
    public Transform player;            
    public TMP_Text interactText;       
    public Image rewardImage;           

    [Header("Configuración")]
    public float distance = 2f;         

    private bool open = false;          

    void Start()
    {
        if (interactText != null)
            interactText.enabled = false; 

        if (rewardImage != null)
            rewardImage.enabled = false;  
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);

        if (!open && dist <= distance)
        {
            if (interactText != null)
                interactText.enabled = true;

            if (Input.GetKeyDown(KeyCode.C))
            {
                OpenChest();
            }
        }
        else
        {
            if (interactText != null)
                interactText.enabled = false;
        }
    }

    void OpenChest()
    {
        open = true;

        if (interactText != null)
            interactText.enabled = false;

        if (rewardImage != null)
            rewardImage.enabled = true;

        Debug.Log("Cofre abierto: se muestra la imagen de recompensa.");
    }
}
