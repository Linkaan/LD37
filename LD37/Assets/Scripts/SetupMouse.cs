using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupMouse : MonoBehaviour {

    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    void OnMouseEnter()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    void OnMouseExit()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
