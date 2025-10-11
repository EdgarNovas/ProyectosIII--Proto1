using UnityEngine;

public class LimitarDentroDeEsfera : MonoBehaviour
{
    public SphereCollider limiteEsfera;
    public CharacterController characterController;

    void Update()
    {
        Vector3 centro = limiteEsfera.transform.position + limiteEsfera.center;
        float radio = limiteEsfera.radius * Mathf.Max(
            limiteEsfera.transform.lossyScale.x,
            limiteEsfera.transform.lossyScale.y,
            limiteEsfera.transform.lossyScale.z);

        Vector3 direccion = characterController.transform.position - centro;
        float distancia = direccion.magnitude;

        if (distancia > radio)
        {
            Vector3 nuevaPos = centro + direccion.normalized * radio;

            // Solo corregir la posición horizontal, no la vertical
            nuevaPos.y = characterController.transform.position.y;

            Vector3 correccion = nuevaPos - characterController.transform.position;

            // Mover usando el CharacterController para respetar colisiones y suelo
            characterController.Move(correccion);

            Debug.Log("Jugador fuera del límite, reposicionado dentro de la esfera.");
        }
    }
}
