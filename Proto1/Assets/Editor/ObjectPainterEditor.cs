using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ObjectPainter))]
public class ObjectPainterEditor : Editor
{
    private ObjectPainter objectPainter;

    void OnEnable()
    {
        // Get the target component this editor is inspecting.
        objectPainter = (ObjectPainter)target;
    }

    /// <summary>
    /// This method is called by Unity whenever it renders the Scene View.
    /// This is where we'll implement our custom brush logic.
    /// </summary>
    public override void OnInspectorGUI()
    {
        // Draw the default inspector fields (prefab, radius, density, etc.)
        DrawDefaultInspector();

        // Add some helpful instructions to the Inspector window.
        EditorGUILayout.HelpBox("Hold down Left Ctrl and use the Left Mouse Button to paint objects.\nHold down Left Ctrl + Left Shift and use the Left Mouse Button to erase objects.", MessageType.Info);
    }

    void OnSceneGUI()
    {
        // Get the current event (e.g., mouse move, mouse click).
        Event currentEvent = Event.current;

        // If the current event is a mouse event and the Left Ctrl key is held down.
        if (currentEvent.isMouse && currentEvent.control)
        {
            // Prevent the default Unity scene controls (like selection) from running.
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

            // Create a ray from the mouse position into the scene.
            Ray worldRay = HandleUtility.GUIPointToWorldRay(currentEvent.mousePosition);
            RaycastHit hitInfo;

            // Perform a raycast to see if we hit any object with a collider.
            if (Physics.Raycast(worldRay, out hitInfo))
            {
                // Draw a visual representation of our brush (a circle) at the hit point.
                Handles.color = Color.cyan;
                Handles.DrawWireDisc(hitInfo.point, hitInfo.normal, objectPainter.brushRadius);
                Handles.color = new Color(0, 1, 1, 0.1f); // Semi-transparent cyan
                Handles.DrawSolidDisc(hitInfo.point, hitInfo.normal, objectPainter.brushRadius);

                // Check if the user is clicking or dragging the left mouse button.
                if ((currentEvent.type == EventType.MouseDown || currentEvent.type == EventType.MouseDrag) && currentEvent.button == 0)
                {
                    // If they are, it's time to paint!
                    PaintObjects(hitInfo.point, hitInfo.normal);
                    // Use the event so it's not processed further.
                    currentEvent.Use();
                }
            }
        }

        // Repaint the scene view to ensure the brush circle is updated in real-time.
        SceneView.RepaintAll();
    }

    /// <summary>
    /// Spawns objects within the brush radius at the given position, oriented to the surface normal.
    /// </summary>
    /// <param name="centerPosition">The center of the brush where the raycast hit.</param>
    /// <param name="surfaceNormal">The normal of the surface at the hit point.</param>
    private void PaintObjects(Vector3 centerPosition, Vector3 surfaceNormal)
    {
        // Check if a prefab has been assigned in the inspector.
        if (objectPainter.prefabToPaint == null)
        {
            Debug.LogWarning("No prefab selected to paint. Please assign a prefab in the ObjectPainter component.");
            return;
        }

        // Loop based on the density setting.
        for (int i = 0; i < objectPainter.brushDensity; i++)
        {
            // Get a random point within a circle and orient it to the surface normal.
            Quaternion surfaceRotation = Quaternion.FromToRotation(Vector3.up, surfaceNormal);
            Vector2 randomPoint2D = Random.insideUnitCircle * objectPainter.brushRadius;
            Vector3 spawnOffset = new Vector3(randomPoint2D.x, 0, randomPoint2D.y);
            Vector3 rotatedOffset = surfaceRotation * spawnOffset;
            Vector3 spawnPosition = centerPosition + rotatedOffset;

            // Raycast from above the spawn position downwards along the surface normal to ensure it spawns on the surface.
            // This is crucial for uneven terrain, walls, and ceilings.
            RaycastHit placementHit;
            if (Physics.Raycast(spawnPosition + surfaceNormal * 10f, -surfaceNormal, out placementHit, 20f))
            {
                // Instantiate the prefab. Using PrefabUtility maintains the connection to the original prefab.
                GameObject newObject = (GameObject)PrefabUtility.InstantiatePrefab(objectPainter.prefabToPaint);
                newObject.transform.position = placementHit.point;
                // Align the new object's "up" direction with the normal of the surface it's on.
                newObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, placementHit.normal);

                // Set the parent if one is assigned.
                if (objectPainter.parentTransform != null)
                {
                    newObject.transform.SetParent(objectPainter.parentTransform);
                }

                // Register the creation of the object for Undo functionality.
                Undo.RegisterCreatedObjectUndo(newObject, "Paint Object");
            }
        }
    }
}
