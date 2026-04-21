using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorUi : MonoBehaviour
{
    public Texture2D cursorTexture;
    public Vector2 cursorHotSpot=Vector2.zero;
    public CursorMode cursormod =CursorMode.Auto;

    private void Start()
    {
        Cursor.SetCursor(cursorTexture, cursorHotSpot, cursormod);
    }
}
