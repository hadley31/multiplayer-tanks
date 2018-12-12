using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EntityFollowCamera : VCamera
{
    public static EntityFollowCamera Instance
    {
        get { return CameraController.CurrentVCam as EntityFollowCamera; }
    }

    public bool follow = true;
    public bool look = true;
    public float scrollSpeed = 5;
    public Vector3 offset;
    public UnityEvent onTargetsChanged;

    private readonly List<Entity> m_Targets = new List<Entity>();

    public List<Entity> Targets
    {
        get { return m_Targets; }
    }

    public Entity MainTarget
    {
        get { return m_Targets.Count > 0 ? m_Targets[0] : null; }
    }

    private void Scroll()
    {
        if (m_Targets.Count == 0)
        {
            Vector3 direction = (transform.forward * Input.GetAxisRaw("Vertical") + transform.right * Input.GetAxisRaw("Horizontal")).normalized;

            if (direction != Vector3.zero)
            {
                transform.Translate(direction * Time.deltaTime * scrollSpeed, Space.World);
            }
        }
    }

    public void OnlyFollow(params Entity[] targets)
    {
        m_Targets.Clear();

        Follow(targets);
    }

    public void Follow(params Entity[] targets)
    {
        foreach (Entity t in targets)
        {
            if (t != null && m_Targets?.Contains(t) == false)
            {
                m_Targets.Add(t);
            }
        }
        onTargetsChanged.Invoke();
    }

    public void StopFollowing(Entity target)
    {
        if (m_Targets?.Remove(target) ?? false)
        {
            onTargetsChanged.Invoke();
        }
    }

    public void ToggleFollow(Entity target)
    {
        if (m_Targets?.Contains(target) ?? false)
        {
            StopFollowing(target);
        }
        else
        {
            Follow(target);
        }
    }

    public override void OnGainFocus()
    {
        
    }

    public override void OnLoseFocus()
    {

    }

    public override void OnFocusUpdate()
    {
        if (Tank.Local == null && Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, 100) == true)
            {
                Entity ent = hitInfo.transform.GetComponent<Entity>();
                if (ent != null)
                {
                    ToggleFollow(ent);
                }
            }
        }

        Scroll();

        List<Entity> targets = m_Targets?.FindAll(x => !x.Is<Tank>() || x.GetComponent<Tank>().IsAlive);

        if (targets?.Count == 0)
        {
            return;
        }

        Vector3 middle;
        float targetCameraDistance = 0;
        if (targets.Count == 1)
        {
            middle = targets[0].transform.position;
        }
        else
        {
            Vector3 min = Vector3.positiveInfinity;
            Vector3 max = Vector3.negativeInfinity;

            foreach (Entity ent in targets)
            {
                min = Vector3.Min(min, ent.transform.position);
                max = Vector3.Max(max, ent.transform.position);
            }

            middle = Vector3.Lerp(min, max, 0.5f);

            float maxDistance = Vector3.Magnitude(min - max);

            targetCameraDistance = (maxDistance / 2 / Camera.main.aspect) / Mathf.Tan(Camera.main.fieldOfView * Mathf.Deg2Rad / 2);
        }

        if (follow)
        {
            Position = middle + offset + offset.normalized * targetCameraDistance;
        }
        if (look)
        {
            Rotation = Quaternion.LookRotation(middle - Position);
        }
    }
}
