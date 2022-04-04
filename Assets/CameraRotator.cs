using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotator : MonoBehaviour
{
    public float XMax;
    public float XMin;
    public float YMax;
    public float YMin;
    
    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        var angle = transform.localEulerAngles;
        angle.x += Input.GetAxis("Mouse Y");
        angle.y += Input.GetAxis("Mouse X");
        angle.x %= 360f;
        angle.y %= 360f;
        if (angle.x > 180)
        {
            angle.x -= 360;
        }

        if (angle.y > 180)
        {
            angle.y -= 360;
        }
        angle.x = Mathf.Clamp(angle.x, XMin, XMax);
        angle.y = Mathf.Clamp(angle.y, YMin, YMax);
        transform.localEulerAngles = angle;
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
    }
}
