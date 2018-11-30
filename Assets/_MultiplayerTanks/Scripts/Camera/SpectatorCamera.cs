using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SpectatorCamera : VCamera
{
    public static SpectatorCamera Instance
    {
        get { return CameraManager.Instance.Current as SpectatorCamera; }
    }

    public int mask
    {
        get { return LayerMask.GetMask("Tank"); }
    }

    public float scrollSpeed = 5;

    [Header("Follow")]
    public float followSpeed = 15.0f;
    public float cameraZoomSpeed = 8.0f;
    public float defaultCameraDistance = 15.0f;
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

    #region Monobehaviours

    private void Update()
    {
        if (Tank.Local == null && Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, 100, mask) == true)
            {
                Entity ent = hitInfo.transform.GetComponent<Entity>();
                if (ent != null)
                {
                    ToggleFollow(ent);
                }
            }
        }

        Scroll();
    }

    #endregion

    private void Scroll()
    {
        if (m_Targets.Count == 0)
        {
            Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

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
            if (m_Targets.Contains(t) == false)
            {
                m_Targets.Add(t);
            }
        }
        onTargetsChanged.Invoke();
    }

    public void StopFollowing(Entity target)
    {
        m_Targets.Remove(target);
    }

    public void ToggleFollow(Entity target)
    {
        if (m_Targets.Contains(target))
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
        List<Entity> targets = m_Targets?.FindAll(x => !x.Is<Tank>() || x.GetComponent<Tank>().IsAlive);

        if (targets?.Count == 0)
        {
            return;
        }

        float angle = Rotation.eulerAngles.x * Mathf.Deg2Rad;

        Vector3 offset;
        if (targets.Count == 1)
        {
            offset = new Vector3(0, defaultCameraDistance * Mathf.Sin(angle), -defaultCameraDistance * Mathf.Cos(angle));
            Position = targets[0].transform.position + offset;
            return;
        }

        Vector3 min = Vector3.positiveInfinity;
        Vector3 max = Vector3.negativeInfinity;

        foreach (Entity ent in targets)
        {
            min = Vector3.Min(min, ent.transform.position);
            max = Vector3.Max(max, ent.transform.position);
        }

        Vector3 middle = Vector3.Lerp(min, max, 0.5f);

        float maxDistance = Vector3.Magnitude(min - max);

        float targetCameraDistance = defaultCameraDistance + (maxDistance / 2 / Camera.main.aspect) / Mathf.Tan(Camera.main.fieldOfView * Mathf.Deg2Rad / 2);

        offset = new Vector3(0, targetCameraDistance * Mathf.Sin(angle), -targetCameraDistance * Mathf.Cos(angle));

        Position = middle + offset;
    }
}
