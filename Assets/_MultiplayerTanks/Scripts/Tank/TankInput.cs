using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankInput : TankBase
{
    private const float Tank_Bottom_Height = 0.333f;
    private const float SQRT2 = 1.41421356237f;


    public Vector3 CursorPosition
    {
        get;
        set;
    }

    private void Awake()
    {
        CursorPosition = new Vector3(Screen.width / 2, Screen.height / 2);
    }


    private void Update()
    {
        if (Tank.IsLocal == false)
        {
            return;
        }

        if (Tank.IsAlive == false)
        {
            return;
        }

        if ((GamemodeControl.Current?.HasControl ?? false) == false)
        {
            return;
        }

        UpdateCursorPosition();

        Movement.SetLookTarget(GetLookTarget());
        Movement.SetTargetDirection(GetTargetDirection());
        Movement.SetBoostHeld(Input.GetButton("Boost"));

        if (Input.GetButtonDown("Shoot"))
        {
            Shooting?.Shoot();
        }

        if (Input.GetButtonDown("Landmine"))
        {
            TankLandmine?.Use();
        }
    }

    private void UpdateCursorPosition()
    {
        Vector3 tempCursorPosition = CursorPosition;

        tempCursorPosition.x += Input.GetAxis("Mouse X") * 2.0f;
        tempCursorPosition.y += Input.GetAxis("Mouse Y") * 2.0f;

        tempCursorPosition.x = Mathf.Clamp(tempCursorPosition.x, 0, Screen.width);
        tempCursorPosition.y = Mathf.Clamp(tempCursorPosition.y, 0, Screen.height);

        CursorPosition = tempCursorPosition;
    }

    public Vector3 GetLookTarget()
    {
        Ray ray = Camera.main.ScreenPointToRay(CursorPosition);

        float enterPoint;
        Plane plane = new Plane(Vector3.up, transform.position + Vector3.up * Tank_Bottom_Height);
        if (plane.Raycast(ray, out enterPoint))
        {
            return ray.GetPoint(enterPoint);
        }

        return Vector3.zero;
    }

    private Vector3 GetTargetDirection()
    {
        // Keyboard friendly input

        float vert = Input.GetAxisRaw("Vertical");
        float horiz = Input.GetAxisRaw("Horizontal");

        Vector3 input = (Camera.main.transform.forward + Camera.main.transform.up) * vert + Camera.main.transform.right * horiz;

        input = Vector3.ProjectOnPlane(input, transform.up);

        return input.normalized;
    }
}