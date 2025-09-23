using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    Transform player;
    private void Awake()
    {
        if (Instance != null)
        {
            // Si ya existe un manager, este se destruye
            Destroy(gameObject);
        }
        else
        {
            // Si no existe, este se convierte en la instancia
            Instance = this;
        }
    }

    void Start()
    {
        
    }

    public void AddPlayer(Transform transform)
    {
        player = transform;
    }

    public Transform GetPlayer()
    {
        return player;
    }
}
