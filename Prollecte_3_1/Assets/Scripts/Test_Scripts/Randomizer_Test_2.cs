using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Randomizer_Test_2 : MonoBehaviour
{
    [SerializeField] private Transform[] inicialsPositions;
    [SerializeField] public GameObject[] inicialsObjects;
   

    private List<GameObject> objects;
    private List<Transform> positions;
    // Start is called before the first frame update
    void Start()
    {
        objects = new List<GameObject>(inicialsObjects);
        positions = new List<Transform>(inicialsPositions);
        Colocate();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            Reestart();
            Colocate();
        }


    }

    void Reestart()
    {
        foreach (var obj in GameObject.FindGameObjectsWithTag("Spawned"))
        {
            Destroy(obj);
        }

        objects = new List<GameObject>(inicialsObjects);
        positions = new List<Transform>(inicialsPositions);
    }

    void Colocate()
    {
        if (positions.Count == 0 || objects.Count == 0)
        {
            Debug.LogWarning("Ya no quedan objetos o posiciones disponibles");
            return;
        }

        while (positions.Count > 0 && objects.Count > 0)
        {
            int randObject = Random.Range(0, objects.Count);
            int randPosition = Random.Range(0, positions.Count);

            Instantiate(objects[randObject], positions[randPosition].position, positions[randPosition].rotation);


            objects.RemoveAt(randObject);
            positions.RemoveAt(randPosition);
        }

    }
}
