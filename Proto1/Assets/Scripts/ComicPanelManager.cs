using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;
public class ComicPanelManager : MonoBehaviour
{
    public List<Image> comicPanels = new List<Image>();

    public TMP_Text buttonText;

    public string nextSceneName = "NombreDeTuSiguienteEscena";

    private int currentPanelIndex = 0;

    void Start()
    {
        HideAllPanels();

        if (buttonText != null)
        {
            buttonText.text = "Continuar";
        }
        else
        {
            Debug.LogError("Referencia de ButtonText perdida. Asigna el componente Text del botón en el Inspector.");
        }
    }

    private void HideAllPanels()
    {
        foreach (Image panel in comicPanels)
        {
            // Accedemos al GameObject del componente Image para desactivarlo.
            panel.gameObject.SetActive(false);
        }
    }

    public void ShowNextPanel()
    {
        if (currentPanelIndex < comicPanels.Count)
        {
            comicPanels[currentPanelIndex].gameObject.SetActive(true);

            currentPanelIndex++;

            if (currentPanelIndex == comicPanels.Count)
            {
                if (buttonText != null)
                {
                    buttonText.text = "Terminar Historia";
                }
            }
        }
        else
        {
            if (!string.IsNullOrEmpty(nextSceneName))
            {
                Debug.Log($"Todas las viñetas mostradas. Cargando escena: {nextSceneName}");
                SceneManager.LoadScene(nextSceneName);
            }
            else
            {
                Debug.LogError("El nombre de la siguiente escena está vacío. Por favor, asigna un nombre de escena válido en el Inspector.");
            }
        }
    }
}
