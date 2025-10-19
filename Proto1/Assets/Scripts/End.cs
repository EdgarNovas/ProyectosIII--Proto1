using UnityEngine;
using UnityEngine.SceneManagement; 

public class End : MonoBehaviour
{
    public GameObject jugador;

    public string nombreEscenaDestino = "NombreDeTuEscena";

    private Collider jugadorCollider;

    void Start()
    {
        if (jugador != null)
        {
            jugadorCollider = jugador.GetComponent<Collider>();
            if (jugadorCollider == null)
            {
                Debug.LogError("El GameObject 'jugador' no tiene un componente Collider.");
            }
        }
        else
        {
            Debug.LogError("La variable 'jugador' no está asignada en el Inspector del script " + this.name);
        }

        if (!GetComponent<Collider>().isTrigger)
        {
            Debug.LogWarning("El Box Collider del objeto " + this.name + " NO está marcado como 'Is Trigger'. Asegúrate de marcarlo para que funcione correctamente.");
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == jugadorCollider)
        {
            Debug.Log("¡Colisión detectada con el jugador! Cambiando a la escena: " + nombreEscenaDestino);

            CargarEscenaDestino();
        }
    }

    void CargarEscenaDestino()
    {
        try
        {
            SceneManager.LoadScene(nombreEscenaDestino);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error al intentar cargar la escena '" + nombreEscenaDestino + "'. Asegúrate de que el nombre es correcto y la escena está en File -> Build Settings. Error: " + e.Message);
        }
    }
}