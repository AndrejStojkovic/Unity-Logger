using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LoggerScreen
{
    public LoggerScreenPosition PrintPosition;
    public float Width = 600f;
    public Vector2 ScreenOffset = Vector2.zero;

    public Rect GetScreenPosition()
    {
        Rect rect = new Rect(0f, 0f, Width, Screen.height);
        switch(PrintPosition)
        {
            case LoggerScreenPosition.TopLeft:
                rect.x = 0f;
                rect.y = 0f;
                break;
            case LoggerScreenPosition.TopRight:
                rect.x = Screen.width - Width;
                rect.y = 0f;
                break;
            // case LoggerScreenPosition.BottomLeft:
            //     rect.x = 0f;
            //     rect.y = Screen.height;
            //     break;
            // case LoggerScreenPosition.BottomRight:
            //     rect.x = Screen.width - Width;
            //     rect.y = Screen.height;
            //     break;
        }
        rect.x += ScreenOffset.x;
        rect.y += ScreenOffset.y;
        return rect;
    }
}
