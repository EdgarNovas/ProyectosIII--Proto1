using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string nextSceneName = "GameScene";

    public void PlayGame()
    {
        Debug.Log("Cargando escena: " + nextSceneName);

        SceneManager.LoadScene(nextSceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Saliendo del juego...");

        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
