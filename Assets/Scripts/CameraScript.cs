using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private Vector3 lastMouse;
    private readonly float camSens = 0.25f;
    private Vector2 snapMouse;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
        Translate();
        
    }

    void Rotate()
    {
        lastMouse = Input.mousePosition - lastMouse;
        lastMouse = new Vector3(lastMouse.y * camSens, lastMouse.x * camSens, 0);
        lastMouse = new Vector3(transform.eulerAngles.x + lastMouse.x, transform.eulerAngles.y - lastMouse.y, 0);

        if (Input.GetMouseButton(2))
            transform.eulerAngles = lastMouse;

        //if (Input.GetMouseButtonDown(1))
        //    snapMouse =
        //if (Input.GetMouseButtonUp(1))
        //    Mouse.current.WarpCursorPosition(snapMouse);

        lastMouse = Input.mousePosition;
    }

    void Translate()
    {
        transform.localPosition += transform.TransformDirection(GetDirection());
    }

    private Vector3 GetDirection()
    {
        Vector3 result = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) result += Vector3.forward;
        if (Input.GetKey(KeyCode.A)) result += Vector3.left;
        if (Input.GetKey(KeyCode.S)) result += Vector3.back;
        if (Input.GetKey(KeyCode.D)) result += Vector3.right;

        return result*0.05f;
    }
}