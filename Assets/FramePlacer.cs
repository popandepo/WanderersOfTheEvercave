using UnityEngine;

public class FramePlacer : MonoBehaviour
{
    public GameObject Frame;
    private bool _hasFrame;
    private bool _mouseOver;

    private void Update()
    {
        if (!_mouseOver)
            if (_hasFrame)
            {
                Destroy(Frame);
                _hasFrame = false;
            }

        _mouseOver = false;
    }

    private void OnMouseOver()
    {
        _mouseOver = true;
        if (Frame != null) return;
        string path = "Prefabs/Map/Visuals/Frames";

        GameObject holder = null;

        foreach (Object g in Resources.LoadAll(path, typeof(GameObject))) holder = g as GameObject;

        typeof(FramePlacer).GetField("Frame").SetValue(this, holder);

        Frame = Instantiate(Frame, transform.position, Quaternion.identity);
        _hasFrame = true;
    }
}