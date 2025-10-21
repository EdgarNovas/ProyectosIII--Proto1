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
    public GameObject bolt;
    public GameObject doorL;
    public GameObject doorR;
    [field: SerializeField] public AudioClip openDoor;

    private int lastEnemysKilled;
    private int lastKeys;

    [Header("Animación de apertura")]
    public float openAngle = 90f;
    public float openSpeed = 2f;
    private Quaternion doorLClosedRot;
    private Quaternion doorLOpenRot;
    private Quaternion doorRClosedRot;
    private Quaternion doorROpenRot;
    private bool isOpening = false;
    

    void Start()
    {
        lastEnemysKilled = enemysKilled;
        lastKeys = keys;

        UpdateUIText();

        if (doorL != null) doorLClosedRot = doorL.transform.localRotation;
        if (doorR != null) doorRClosedRot = doorR.transform.localRotation;
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

        if (isOpening)
        {
            if (doorL != null)
                doorL.transform.localRotation = Quaternion.Slerp(doorL.transform.localRotation, doorLOpenRot, Time.deltaTime * openSpeed);

            if (doorR != null)
                doorR.transform.localRotation = Quaternion.Slerp(doorR.transform.localRotation, doorROpenRot, Time.deltaTime * openSpeed);
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

        if (bolt != null)
            Destroy(bolt);

        if (doorL != null && doorR != null)
        {
            doorLOpenRot = Quaternion.Euler(0, -openAngle, 0) * doorLClosedRot;
            doorROpenRot = Quaternion.Euler(0, openAngle, 0) * doorRClosedRot;
            SoundManager.Instance.PlaySound(openDoor);
            isOpening = true;
        }

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
