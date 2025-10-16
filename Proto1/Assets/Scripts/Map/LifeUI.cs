using UnityEngine;
using UnityEngine.UI;

public class LifeUI : MonoBehaviour
{
    [Header("Referencias")]
    public Slider healthSlider;

    private Health healthComponent;

    private void Start()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");

        if (playerObject == null)
        {
            Debug.LogError("No se encontró ningún GameObject con la etiqueta 'Player'. Asegúrate de que tu Player tenga esa etiqueta asignada.", this);
            return;
        }

        healthComponent = playerObject.GetComponent<Health>();

        if (healthComponent == null)
        {
            Debug.LogError("El GameObject etiquetado como 'Player' no tiene el componente Health.", this);
            return;
        }

        healthSlider.maxValue = healthComponent.MaxHealth;
        healthSlider.value = healthComponent.GetHealth();

        healthComponent.OnHealthChanged += UpdateHealthBar;

        Debug.Log("LifeUI conectado exitosamente al componente Health del Player.");
    }

    private void OnDestroy()
    {
        if (healthComponent != null)
        {
            healthComponent.OnHealthChanged -= UpdateHealthBar;
        }
    }

    private void UpdateHealthBar()
    {
        healthSlider.value = healthComponent.GetHealth();
    }
}