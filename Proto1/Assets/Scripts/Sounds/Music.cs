using UnityEngine;

public class Music : MonoBehaviour
{
    [field: SerializeField] public AudioClip music;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SoundManager.Instance.PlaySound(music);
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
