using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Randomizer_Test : MonoBehaviour
{

    [SerializeField] private Transform[] positions;
    public int randomicer;
    private int random;

    // Start is called before the first frame update
    void Start()
    {
        Transport();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Transport();
        }
    }

    void Transport()
    {
        if (positions.Length == 0)
        {
            Debug.LogWarning("No hay posiciones asignadas en el array.");
            return;
        }

        do
        {
            random = Random.Range(0, positions.Length);

        } while (randomicer == random);

        randomicer = random;

        transform.position = positions[randomicer].position;

    }
}
