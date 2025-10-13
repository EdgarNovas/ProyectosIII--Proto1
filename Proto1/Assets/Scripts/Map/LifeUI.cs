using UnityEngine;
using UnityEngine.UI;

public class LifeUI : MonoBehaviour
{
    [Header("Configuración de Vida")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("Referencias UI")]
    public Slider healthSlider;

    [Header("Configuración")]
    public float healthStep = 10f; 

    private void Start()
    {
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

    private void Update()
    {
        // Aumentar vida si se pulsa +
        if (Input.GetKeyDown(KeyCode.Equals) && Input.GetKey(KeyCode.LeftShift))
        {
            IncreaseHealth();
        }

        // Alternativa para teclados numéricos
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            IncreaseHealth();
        }

        // Disminuir vida si se pulsa -
        if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            DecreaseHealth();
        }
    }

    private void IncreaseHealth()
    {
        currentHealth = Mathf.Min(currentHealth + healthStep, maxHealth);
        UpdateHealthBar();
    }

    private void DecreaseHealth()
    {
        currentHealth = Mathf.Max(currentHealth - healthStep, 0);
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        healthSlider.value = currentHealth;
    }
}
