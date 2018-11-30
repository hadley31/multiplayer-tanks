using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance
    {
        get;
        private set;
    }


    public float moveLerpSpeed = 15.0f;
    public float rotateSlerpSpeed = 15.0f;
    public float zoomLerpSpeed = 5.0f;


    private List<VCamera> m_VirtualCameras;
    private int m_Index;

    private int Index
    {
        get { return m_Index = Mathf.Clamp(m_Index, 0, m_VirtualCameras?.Count ?? 0); }
        set { m_Index = Mathf.Clamp(value, 0, m_VirtualCameras?.Count ?? 0); }
    }

    public VCamera Current
    {
        get;
        private set;
    }

    public Camera Camera
    {
        get;
        private set;
    }

    private void Awake()
    {
        Camera = GetComponent<Camera>();
    }

    private void OnEnable()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDisable()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void LateUpdate()
    {
        VCamera desired = m_VirtualCameras?[Index];
        if (Current == desired && Current != null)
        {
            Current.OnFocusUpdate();
        }
        else
        {
            Current?.OnLoseFocus();
            desired?.OnGainFocus();

            Current = desired;
        }

        if (Current == null)
        {
            return;
        }

        transform.position = Vector3.Lerp(transform.position, Current.Position, Time.deltaTime * moveLerpSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation, Current.Rotation, Time.deltaTime * rotateSlerpSpeed);
        Camera.fieldOfView = Mathf.Lerp(Camera.fieldOfView, Current.FieldOfView, Time.deltaTime * zoomLerpSpeed);
    }

    public void AddVCam(VCamera vCamera)
    {
        if (m_VirtualCameras == null)
        {
            m_VirtualCameras = new List<VCamera>();
        }

        if (!m_VirtualCameras.Contains(vCamera))
        {
            m_VirtualCameras.Add(vCamera);
        }
    }

    public void RemoveVCam(VCamera vCamera)
    {
        m_VirtualCameras?.Remove(vCamera);
    }

    public void Next()
    {
        Index++;
    }

    public void Previous()
    {
        Index--;
    }

    public void RequestFocus(VCamera vCamera)
    {
        int index = m_VirtualCameras.IndexOf(vCamera);
        if (index > 0)
        {
            Index = index;
        }
    }
}
