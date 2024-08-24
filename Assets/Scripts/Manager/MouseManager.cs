using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*using UnityEngine.Events;*/
using System;

/*[System.Serializable]
public class EventVector3 : UnityEvent<Vector3> { }*/

public class MouseManager : Singleton<MouseManager>
{
    public Texture2D point, doorway, attack, target, arrow;

    RaycastHit hitInfo;
    /*    public EventVector3 OnMouseClicked;*/
    public event Action<Vector3> OnMouseClicked; // event to be invoked when mouse is clicked
    public event Action<GameObject> OnEnemyClicked; // event to be invoked when enemy is clicked

    protected override void Awake()
    {
        base.Awake();
        // don't destroy this object when loading a new scene
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        SetCursurTexture();
        MouseControl();
    }

    void SetCursurTexture()
    {
        // get ray from camera to mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hitInfo))
        {
            // change mouse cursor texture
            switch (hitInfo.collider.gameObject.tag)
            {
                case "Ground":
                    Cursor.SetCursor(target, new Vector2(16, 16), CursorMode.Auto);
                    break;
                case "Enemy":
                    Cursor.SetCursor(attack, new Vector2(16, 16), CursorMode.Auto);
                    break;
                case "Attackable":
                    Cursor.SetCursor(attack, new Vector2(16, 16), CursorMode.Auto);
                    break;
                case "Portal":
                    Cursor.SetCursor(doorway, new Vector2(16, 16), CursorMode.Auto);
                    break;
            }
        }
    }

    void MouseControl()
    {
        if (Input.GetMouseButtonDown(0) && hitInfo.collider != null)
        {
            if (hitInfo.collider.gameObject.CompareTag("Ground"))
            {
                // if clicked on Ground, invoke event
                OnMouseClicked?.Invoke(hitInfo.point);
            }
            if (hitInfo.collider.gameObject.CompareTag("Enemy"))
            {
                // if clicked on Enemy, invoke event
                OnEnemyClicked?.Invoke(hitInfo.collider.gameObject);
            }
            if (hitInfo.collider.gameObject.CompareTag("Attackable"))
            {
                // if clicked on Attackable, invoke event
                OnEnemyClicked?.Invoke(hitInfo.collider.gameObject);
            }
            if (hitInfo.collider.gameObject.CompareTag("Portal"))
            {
                // if clicked on Attackable, invoke event
                OnMouseClicked?.Invoke(hitInfo.point);
            }
        }
    }

}
