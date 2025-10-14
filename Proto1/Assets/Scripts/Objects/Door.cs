using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Door : MonoBehaviour
{
    [Header("Contadores")]
    public int enemysKilled;
    public int keys;

    [SerializeField] private int maxEnemys;
    [SerializeField] private int maxKeys;

    [Header("Referencias UI")]
    public TextMeshProUGUI textEnemy;
    public TextMeshProUGUI textKey;

    [Header("Puerta")]
    public bool open = false;
    public BoxCollider colliderDoor;
    public GameObject door;

    private int lastEnemysKilled;
    private int lastKeys;

    void Start()
    {
        lastEnemysKilled = enemysKilled;
        lastKeys = keys;

        UpdateUIText();
    }

    void Update()
    {
        if (enemysKilled != lastEnemysKilled)
        {
            lastEnemysKilled = enemysKilled;
            ChangeTextEnemy();
        }

        if (keys != lastKeys)
        {
            lastKeys = keys;
            ChangeTextKeys();
        }

        if (enemysKilled >= maxEnemys && keys >= maxKeys && !open)
        {
            OpenDoor();
        }
    }

    void ChangeTextEnemy()
    {
        if (textEnemy != null)
            textEnemy.text = "Enemigos: " + enemysKilled + " / " + maxEnemys;
    }

    void ChangeTextKeys()
    {
        if (textKey != null)
            textKey.text = "Llaves: " + keys + " / " + maxKeys;
    }

    void OpenDoor()
    {
        open = true;

        if (colliderDoor != null)
            Destroy(colliderDoor);

        if (door != null)
            Destroy(door);

        Debug.Log("¡Puerta abierta!");
    }

    void UpdateUIText()
    {
        ChangeTextEnemy();
        ChangeTextKeys();
    }

    public void AddEnemyKilled()
    {
        enemysKilled++;
    }

    public void AddKey()
    {
        keys++;
    }
}
