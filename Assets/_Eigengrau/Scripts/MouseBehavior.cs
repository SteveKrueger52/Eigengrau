using UnityEngine;
using System.Collections;

public class MouseBehavior: MonoBehaviour
{
    public Texture2D cursorTex;
    public CursorMode cMode = CursorMode.Auto;

    public void OnMouseEnter()
    {
        Debug.Log("Entering and screaming");
        Cursor.SetCursor(cursorTex, Vector2.zero, cMode);
    }
    public void OnMouseExit()
    {
        Debug.Log("Leaving and screaming");
        Cursor.SetCursor(null, Vector2.zero, cMode);
    }
}

