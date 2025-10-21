using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField]AudioSource soundSource;

    [SerializeField]AudioSource musicSource;

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

    
    void Update()
    {
        
    }


    public void PlaySound(AudioClip sound)
    {
        soundSource.PlayOneShot(sound);
    }

    public void PlayMusic(AudioClip music)
    {
        soundSource.PlayOneShot(music);
    }

}
