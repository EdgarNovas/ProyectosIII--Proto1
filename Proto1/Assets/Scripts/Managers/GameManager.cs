using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [field: SerializeField] public CinemachineCamera LastHitCamera { get; private set; }
    [SerializeField] private GameObject targetFocus;
    


    private Coroutine lastHitRoutine;

    [SerializeField] private float lastHitCamDuration = 1.5f;
    [SerializeField] private int lastHitCamPriority = 20; // Prioridad alta para activarla

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



    // --- El resto de la lógica es EXACTAMENTE LA MISMA ---
    public void TriggerLastHitCamera(Transform playerTransform, Transform enemyTransform)
    {
        if ( LastHitCamera == null) return;
        if (lastHitRoutine != null) StopCoroutine(lastHitRoutine);
        LastHitCamera.Priority = lastHitCamPriority;
        targetFocus.transform.position = enemyTransform.position;

        // 4. Inicia la corrutina para resetear
        lastHitRoutine = StartCoroutine(ResetLastHitCameraRoutine());
    }

    private IEnumerator ResetLastHitCameraRoutine()
    {
        yield return new WaitForSeconds(lastHitCamDuration);

        LastHitCamera.Priority = 5;



        lastHitRoutine = null;
    }

}
