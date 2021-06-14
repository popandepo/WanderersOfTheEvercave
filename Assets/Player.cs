using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Camera Camera;

    // Start is called before the first frame update
    private void Awake()
    {
        Camera = Camera.main;
        Vector3 mousePosition = Camera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 thisPosition = transform.position;
        
        Vector3 newCamPos = ((mousePosition+thisPosition)/2);
        newCamPos.z = -10;
        
        Camera.transform.position = newCamPos;
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
}
