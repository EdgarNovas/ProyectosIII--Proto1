using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string nextSceneName = "GameScene";

    private void Start()
    {
        Cursor.visible = true;

        Cursor.lockState = CursorLockMode.None;
    }
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
