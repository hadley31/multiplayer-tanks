using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VCamera : MonoBehaviour
{
    public bool requestFocusOnEnable;
    public float fieldOfView = 60;

    public bool IsCurrent
    {
        get { return CameraController.CurrentVCam == this; }
    }

    public virtual Vector3 Position
    {
        get { return transform.position; }
        protected set { transform.position = value; }
    }

    public virtual Quaternion Rotation
    {
        get { return transform.rotation; }
        protected set { transform.rotation = value; }
    }

    public virtual float FieldOfView
    {
        get { return fieldOfView; }
    }

    protected virtual void OnEnable()
    {
        CameraController.AddVCam(this);

        if (requestFocusOnEnable)
        {
            RequestFocus();
        }
    }

    protected virtual void OnDisable()
    {
        CameraController.RemoveVCam(this);
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(Position, Rotation * Vector3.forward);
    }

    public abstract void OnGainFocus();
    public abstract void OnLoseFocus();
    public abstract void OnFocusUpdate();

    public void RequestFocus()
    {
        CameraController.RequestFocus(this);
    }
}
