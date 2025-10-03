using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class ImageWorldCanvas : MonoBehaviour
{
    [SerializeField] Image myImage;

    [SerializeField] Transform target;
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    void LateUpdate()
    {
        if (target != null)
        {
            // Update the nameplate's position
            transform.position = target.position;

            transform.LookAt(Camera.main.transform.position);
        }
    }
}
