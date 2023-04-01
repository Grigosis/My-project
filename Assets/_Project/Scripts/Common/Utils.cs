using System;
using UnityEngine;

public static class Utils
{
    public static Rect GetScreenRect(Vector3 screenPosition1, Vector3 screenPosition2)
    {
        screenPosition1.y = Screen.height - screenPosition1.y;
        screenPosition2.y = Screen.height - screenPosition2.y;

        Vector3 topLeft = Vector3.Min(screenPosition1, screenPosition2);
        Vector3 bottomRight = Vector3.Max(screenPosition1, screenPosition2);

        return Rect.MinMaxRect(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y);
    }

    public static bool IsWithinSelectionBounds(GameObject gameObject, Rect rect)
    {
        var collider = gameObject.GetComponent<Collider>();
        if (collider != null)
            return rect.Contains(Camera.main.WorldToScreenPoint(collider.bounds.max));
        else
            return rect.Contains(Camera.main.WorldToScreenPoint(gameObject.transform.position));
    }

    public static void DrawScreenSpaceRect(Rect rect, Color color, float thickness)
    {
        GUI.color = color;
        GUI.Box(rect, GUIContent.none);
        GUI.color = Color.white;
    }
}

