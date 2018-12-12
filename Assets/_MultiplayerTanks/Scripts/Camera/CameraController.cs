using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance
    {
        get;
        private set;
    }


    public float moveLerpSpeed = 15.0f;
    public float rotateSlerpSpeed = 15.0f;
    public float zoomLerpSpeed = 5.0f;

    public UnityEvent onFocusChanged;

    private static List<VCamera> virtualCameras;
    private static int m_Index;

    private static int Index
    {
        get { return m_Index = Mathf.Clamp(m_Index, 0, virtualCameras?.Count - 1 ?? 0); }
        set { m_Index = Mathf.Clamp(value, 0, virtualCameras?.Count - 1 ?? 0); }
    }

    public static VCamera CurrentVCam
    {
        get;
        private set;
    }

    private Camera m_Camera;

    public static Camera Camera
    {
        get { return Instance?.m_Camera; }
    }

    private void Awake()
    {
        m_Camera = GetComponent<Camera>();
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            Next();
        }
        else if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            Previous();
        }
    }

    private void LateUpdate()
    {
        VCamera desired = virtualCameras?[Index];
        if (CurrentVCam == desired && CurrentVCam != null)
        {
            CurrentVCam.OnFocusUpdate();
        }
        else
        {
            CurrentVCam?.OnLoseFocus();
            desired?.OnGainFocus();

            if (CurrentVCam != (CurrentVCam = desired))
            {
                onFocusChanged.Invoke();
            }
        }

        if (CurrentVCam == null)
        {
            return;
        }

        transform.position = Vector3.Lerp(transform.position, CurrentVCam.Position, Time.deltaTime * moveLerpSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation, CurrentVCam.Rotation, Time.deltaTime * rotateSlerpSpeed);
        Camera.fieldOfView = Mathf.Lerp(Camera.fieldOfView, CurrentVCam.FieldOfView, Time.deltaTime * zoomLerpSpeed);
    }

    public static void AddVCam(VCamera vCamera)
    {
        if (virtualCameras == null)
        {
            virtualCameras = new List<VCamera>();
        }

        if (!virtualCameras.Contains(vCamera))
        {
            virtualCameras.Add(vCamera);
        }
    }

    public static void RemoveVCam(VCamera vCamera)
    {
        virtualCameras?.Remove(vCamera);
    }

    public static void Next()
    {
        Index++;
    }

    public static void Previous()
    {
        Index--;
    }

    public static void RequestFocus(VCamera vCamera)
    {
        int index = virtualCameras.IndexOf(vCamera);
        if (index > 0)
        {
            Index = index;
        }
    }
}
