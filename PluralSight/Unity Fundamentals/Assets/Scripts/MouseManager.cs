using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[System.Serializable]
public class EventVector3 : UnityEvent<Vector3> { }

public class MouseManager : MonoBehaviour
{
    [SerializeField] private LayerMask clickableLayer;

    [SerializeField] private Texture2D pointer;
    [SerializeField] private Texture2D target;
    [SerializeField] private Texture2D doorway;
    [SerializeField] private Texture2D combat;

    [SerializeField] private EventVector3 onClickEnvironment;
    void Update() 
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit, 50, clickableLayer.value))
        {
            // Mouse raycast
            bool door = false;
            bool item = false;
            if (hit.collider.gameObject.CompareTag("Doorway"))
            {
                Cursor.SetCursor(doorway, new Vector2(16, 16), CursorMode.Auto);
                door = true;
            }
            else if (hit.collider.gameObject.CompareTag("Item"))
            {
                Cursor.SetCursor(target, new Vector2(16, 16), CursorMode.Auto);
                item = true;
            }
            else
            {
                Cursor.SetCursor(pointer, new Vector2(16, 16), CursorMode.Auto);
            }
            
            // Move
            if (Input.GetMouseButtonDown(0))
            {
                if (door)
                {
                    var doorwayTransform = hit.collider.gameObject.transform;
                    onClickEnvironment.Invoke(doorwayTransform.position);
                }
                else if (item)
                {
                    var itemTransform = hit.collider.gameObject.transform;
                    onClickEnvironment.Invoke(itemTransform.position);
                    
                }
                else
                {
                    onClickEnvironment.Invoke(hit.point);
                }
            }
        }
        else
        {
            Cursor.SetCursor(pointer, Vector2.zero, CursorMode.Auto);
        }
    }
}
