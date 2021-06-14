using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FramePlacer : MonoBehaviour
{
    public GameObject Frame;
    private bool _mouseOver = false;
    private void Update()
    {
        if (!_mouseOver && Frame!=null)
        {
            Destroy(Frame);
        }

        _mouseOver = false;
    }
    private void OnMouseOver()
    {
        _mouseOver = true;
        if (Frame != null) return;
        string path = $"Prefabs/Map/Visuals/Frames";

        GameObject holder = null;
                    
        foreach (var g in Resources.LoadAll(path,typeof(GameObject)))
        {
            holder = (g as GameObject);
        }
                    
        typeof(FramePlacer).GetField("Frame").SetValue(this,holder);

        Frame = Instantiate(Frame,transform.position,Quaternion.identity);
    }
}
