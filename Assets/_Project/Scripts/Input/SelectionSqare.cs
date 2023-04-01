using SecondCycleGame;
using System;
using UnityEngine;

public class SelectionSqare : Singleton<SelectionSqare>
{
    [SerializeField] private Color _color;
    [SerializeField] private float _thickness;
    private Rect _rect;
    private GUIStyle _style;

    public Rect SelectionRect => _rect;

    private void Start()
    {
        _style = new GUIStyle();
        _style.normal.background = MakeTexture(2, 2, new Color(0.5f, 0.5f, 0.5f, 0.3f));
        _style.border = new RectOffset(1, 1, 1, 1);
    }

    private void OnGUI()
    {
        if(InputController.Instance.IsSelecting)
        {
            GUI.Box(_rect, "", _style);
        }
    }

    public void UpdateSelectionRect(Vector3 startPosition, Vector3 endPosition)
    {
        _rect = Utils.GetScreenRect(startPosition, endPosition);
    }

    private Texture2D MakeTexture(int width, int height, Color color)
    {
        var pix = new Color[width * height];

        for (int i = 0; i < pix.Length; i++)
        {
            pix[i] = color;
        }

        var result = new Texture2D(width, height);

        result.SetPixels(pix);
        result.Apply();

        return result;
    }
}

